using Microsoft.Data.Sqlite;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yugioh.Data.Data;
using Yugioh.Data.Entities;
using Yugioh.Data.Repositories;
using Yugioh.Draw.Repositories;
using Yugioh.Sync.Extensions;
using Yugioh.Sync.Factories;
using Yugioh.Sync.Konami.Pages;
using Yugioh.Sync.Ygopro.Clients;
using Yugioh.Sync.Ygopro.Responses;
using Yugioh.Sync.Yugipedia.Clients;

namespace Yugioh.Sync
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("consoleapp.log")
                .WriteTo.Console()
                .CreateLogger();

            // Paths
            string projectPath = Path.GetFullPath("../..");
            string cachePath = Path.Combine(projectPath, "Cache");
            string dbPath = Path.Combine(projectPath, "Yugioh.sqlite");
            string geckoDriverDirectory = Path.Combine(projectPath, "Drivers");

            // Initialise web driver (Firefox must be installed)
            IWebDriver driver = new FirefoxDriver(geckoDriverDirectory);
            int explicitTimeoutSeconds = 2;

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                // Update database schema
                var schemaMigration = new SchemaMigration(connection);
                await schemaMigration.ApplyAsync();

                // Initialise data repositories
                var cardRepository = new CardRepository();
                var artworkRepository = new ArtworkRepository();
                var productRepository = new ProductRepository();
                var productCardRepository = new ProductCardRepository();

                // Initialise factories
                var resourceRepository = new ResourceRepository();
                var cardBitmapFactory = new CardBitmapFactory(resourceRepository);
                var cardFactory = new CardFactory();

                var httpClient = new HttpClient();
                var ygoproClient = new YgoproClient(httpClient, cachePath);
                var yugipediaClient = new YugipediaClient(httpClient, cachePath);
                
                GetCardsResponse getCardsResponse = await ygoproClient.GetCardsAsync();
                var ygoproCardEntities = getCardsResponse.Data.OrderBy(c => c.Name).ToList();

                driver.Url = "https://www.db.yugioh-card.com/yugiohdb/card_search.action?request_locale=en";

                try
                {
                    var acceptCookiesElement = driver.FindElement(By.Id("privacy-information-cookie-notice-opt-in"), explicitTimeoutSeconds);
                    acceptCookiesElement.Click();
                }
                catch (NoSuchElementException) { }

                // Click the search box
                ((IJavaScriptExecutor)driver).ExecuteScript("Search();");

                // Find element to ensure page loads first before changing driver url
                driver.FindElement(By.Id("dk_container_rp"), explicitTimeoutSeconds);
                // Change the search to 100 cards per page
                driver.Url = driver.Url + "&rp=100";

                bool hasReachedEnd = false;
                int page = 1;
                int index = 0;
                while (!hasReachedEnd)
                {
                    // TODO : TEMPORARY SO I CAN BUILD IMAGES
                    /*hasReachedEnd = true;
                    continue;*/

                    // Store current url
                    string pageUrl = driver.Url;

                    var cardListPage = new CardListPage(driver);

                    foreach (var konamiCardEntity in cardListPage.CardEntities)
                    {
                        ++index;

                        try
                        {
                            using (var transaction = connection.BeginTransaction())
                            {
                                CardEntity cardEntity = await cardRepository.FindCardAsync(konamiCardEntity.CardId, connection, transaction);

                                if (cardEntity != null)
                                {
                                    //Log.Warning($"Skipping {cardEntity.Name} - already exists");
                                    continue;
                                }

                                Log.Information($"### {index} : Processing {konamiCardEntity.CardId} ({konamiCardEntity.Name})");

                                // 1) Navigate to the card page
                                driver.Url = konamiCardEntity.Url;
                                var cardPage = new CardPage(driver);

                                // 2) Determine the passcode                             
                                Ygopro.Entities.CardEntity ygoproCardEntity = null;
                                Yugipedia.Entities.CardEntity yugipediaCardEntity = null;
                                string passcode = null;

                                var firstCardNumber = cardPage.CardSetInfoEntities.First().Number;

                                try
                                {
                                    var firstYgoproCardSetInfoEntity = await ygoproClient.GetCardSetInfoAsync(firstCardNumber);
                                    if (firstYgoproCardSetInfoEntity != null && firstYgoproCardSetInfoEntity.Id != 0)
                                    {
                                        int ygoproCardImageId = firstYgoproCardSetInfoEntity.Id;
                                        ygoproCardEntity = ygoproCardEntities.Find(ce => ce.CardImageEntities.Select(cie => cie.Id).Contains(ygoproCardImageId));
                                        passcode = konamiCardEntity.Attribute == "DIVINE" || ygoproCardEntity.Id.ToString().Length > 8 ? null : ygoproCardEntity.Id.ToString().PadLeft(8, '0');
                                    }
                                }
                                catch (Exception e) {}

                                if (ygoproCardEntity == null)
                                {
                                    Log.Warning($"Failed to find YGOPRO CardSetInfo for {firstCardNumber}. Attempting Yugipedia");

                                    try
                                    {
                                        yugipediaCardEntity = await yugipediaClient.GetCardAsync(firstCardNumber);

                                        var passcodeRegex = new Regex(@"^\d$");

                                        if (passcodeRegex.IsMatch(yugipediaCardEntity.Passcode))
                                        {
                                            passcode = yugipediaCardEntity.Passcode.PadLeft(8, '0');
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Log.Warning($"Skipping {konamiCardEntity.Name} ({konamiCardEntity.CardId}) - Failed to find Yugipedia CardSetInfo for {firstCardNumber}");
                                    }
                                }

                                // 4) insert card
                                cardEntity = cardFactory.CreateCard(konamiCardEntity, passcode);
                                await cardRepository.InsertCardAsync(cardEntity, connection, transaction);
                                Log.Information($"Inserting Card {passcode}");


                                // 5) Synchronizing Images 	
                                // Create the cache artwork folders if necessary
                                string konamiArtworkDirectory = Path.Combine(cachePath, $"Konami\\Artworks\\{konamiCardEntity.CardId}");
                                if (!Directory.Exists(konamiArtworkDirectory))
                                {
                                    Directory.CreateDirectory(konamiArtworkDirectory);
                                }

                                // Save the image from YGOPRO
                                if (ygoproCardEntity != null)
                                {
                                    int ygoproCardImageCount = ygoproCardEntity.CardImageEntities.Count;
                                    for (int a = 0; a < ygoproCardImageCount; ++a)
                                    {
                                        var ygoproCardImageEntity = ygoproCardEntity.CardImageEntities[a];
                                        Image image = await ygoproClient.GetImageAsync(ygoproCardImageEntity.Id);

                                        if (image == null)
                                        {
                                            Log.Warning($"Skipping YGOPRO ImageId {ygoproCardImageEntity.Id} - image is null");
                                            continue;
                                        }
                                    }
                                }
                                
                                // Insert the artwork records if we have them under the "Cache\Konami\Artworks\{CardId}" folder
                                int artworksInserted = 0;
                                for (int b = 0; b < cardPage.ArtworkCount; ++b)
                                {
                                    int ordinal = b + 1;

                                    // Skip if we have the artwork record
                                    if (await artworkRepository.HasArtworkAsync(konamiCardEntity.CardId, ordinal, connection, transaction))
                                    {
                                        continue;
                                    }
                                    
                                    string konamiArtworkPath = Path.Combine(konamiArtworkDirectory, $"{ordinal}.jpg");

                                    if (!File.Exists(konamiArtworkPath))
                                    {
                                        Log.Warning($"Image {konamiArtworkPath} not found");
                                        continue;
                                    }

                                    var artworkImage = Image.FromFile(konamiArtworkPath);

                                    var artworkEntity = new ArtworkEntity()
                                    {
                                        CardId = konamiCardEntity.CardId,
                                        Ordinal = ordinal,
                                        Image = artworkImage
                                    };

                                    await artworkRepository.InsertArtworkAsync(artworkEntity, connection, transaction);
                                    Log.Information($"Inserted Artwork {artworkEntity.CardId}/{artworkEntity.Ordinal}");

                                    ++artworksInserted;
                                }

                                // Rollback if not all of the artworks are present
                                if (artworksInserted < cardPage.ArtworkCount)
                                {
                                    Log.Warning($"Skipping processing of card {konamiCardEntity.CardId} ({konamiCardEntity.Name}) - Incomplete artworks {artworksInserted}/{cardPage.ArtworkCount}");
                                    transaction.Rollback();
                                    continue;
                                }

                                // 6) Synchronise card sets
                                foreach (var konamiCardSetInfoEntity in cardPage.CardSetInfoEntities)
                                {
                                    var konamiProductEntity = konamiCardSetInfoEntity.ProductEntity;

                                    // Find or insert product
                                    var productEntity = await productRepository.FindProductAsync(konamiProductEntity.ProductId, connection, transaction);
                                    if (productEntity == null)
                                    {
                                        driver.Url = konamiProductEntity.Url;
                                        var productPage = new ProductPage(driver);

                                        productEntity = new ProductEntity()
                                        {
                                            ProductId = konamiProductEntity.ProductId,
                                            Title = konamiProductEntity.Title,
                                            SetSize = productPage.SetSize,
                                            LaunchDate = konamiProductEntity.LaunchDate
                                        };

                                        await productRepository.InsertProductAsync(productEntity, connection, transaction);
                                        Log.Information($"Inserted Product {productEntity.Title}");
                                    }

                                    var productCardEntity = await productCardRepository.FindProductCardAsync(konamiCardSetInfoEntity.Number, connection, transaction);
                                    if (productCardEntity == null)
                                    {
                                        productCardEntity = new ProductCardEntity()
                                        {
                                            Code = konamiCardSetInfoEntity.Number,
                                            ProductId = productEntity.ProductId,
                                            CardId = cardEntity.CardId,
                                            Rarity = konamiCardSetInfoEntity.Rarity.Replace(" ", "")
                                        };

                                        // We have to manually determine the artworks used when there are alts available
                                        if (cardPage.ArtworkCount == 1)
                                        {
                                            productCardEntity.ArtworkOrdinal = 1;
                                        }

                                        await productCardRepository.InsertProductCardAsync(productCardEntity, connection, transaction);
                                        Log.Information($"Inserting Set Number {konamiCardSetInfoEntity.Number}");
                                    }
                                }

                                transaction.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, $"Error ocurred synchronising card {konamiCardEntity.Name}");
                        }
                    }

                    // Reload the card list page if we've moved off 
                    if (driver.Url != pageUrl)
                    {
                        driver.Url = "https://www.db.yugioh-card.com/yugiohdb/card_search.action?request_locale=en";

                        try
                        {
                            var acceptCookiesElement = driver.FindElement(By.Id("privacy-information-cookie-notice-opt-in"), explicitTimeoutSeconds);
                            acceptCookiesElement.Click();
                        }
                        catch (NoSuchElementException) { }

                        // Click the search box
                        ((IJavaScriptExecutor)driver).ExecuteScript("Search();");

                        // Find element to ensure page loads first before changing driver url
                        driver.FindElement(By.Id("dk_container_rp"), explicitTimeoutSeconds);
                        // Change the search to 100 cards per page
                        driver.Url = driver.Url + "&rp=100";
                    }

                    page += 1;
                    ((IJavaScriptExecutor)driver).ExecuteScript($"ChangePage({page});");
                    try
                    {
                        var noDataElement = driver.FindElement(By.XPath("//div[@class='no_data']"), explicitTimeoutSeconds);
                        hasReachedEnd = true;
                    }
                    catch (NoSuchElementException) 
                    {
                        int firstSearchIndex = ((page - 1) * 100) + 1;
                        driver.FindElement(By.XPath($"//div[@class='page_num_title']/div/strong[contains(text(),'Search Results: {String.Format("{0:n0}", firstSearchIndex)}')]"), explicitTimeoutSeconds);
                    }
                }


                string productsDirectory = "Products";
                if (!Directory.Exists(productsDirectory))
                {
                    Directory.CreateDirectory(productsDirectory);
                }

                List<ProductEntity> productEntities = await productRepository.AllProductsAsync(connection, null);

                // Add here when need to redraw cards
                var redrawCardIdList = new List<int>() { };

                foreach (var productEntity in productEntities)
                {
                    List<ProductCardEntity> productCardEntities = await productCardRepository.FindProductCardsAsync(productEntity.ProductId, connection, null);

                    if (productEntity.SetSize > productCardEntities.Count)
                    {
                        Log.Warning($"Skipping product {productEntity.Title} - Incomplete card list artwork {productCardEntities.Count}/{productEntity.SetSize}");
                        continue;
                    }

                    string productDirectory = Path.Combine(productsDirectory, productEntity.ProductId);
                    if (!Directory.Exists(productDirectory))
                    {
                        Directory.CreateDirectory(productDirectory);
                    }

                    foreach (var productCardEntity in productCardEntities)
                    {
                        string outPath = Path.Combine(productDirectory, $"{productCardEntity.Code}.jpg");

                        if (File.Exists(outPath) && !redrawCardIdList.Contains(productCardEntity.CardId))
                        {
                            continue;
                        }

                        var cardEntity = await cardRepository.FindCardAsync(productCardEntity.CardId, connection, null);
                        var artworkEntity = await artworkRepository.FindArtworkAsync(cardEntity.CardId, productCardEntity.ArtworkOrdinal, connection, null);

                        if (artworkEntity == null)
                        {
                            Log.Warning($"Could not draw {cardEntity.Name} ({cardEntity.CardId}) - Missing artwork {productCardEntity.ArtworkOrdinal}");
                            continue;
                        }

                        Log.Information($"Drawing {productEntity.Title}/{productCardEntity.Code}");
                        Bitmap cardBitmap = await cardBitmapFactory.CreateCardBitmapAsync(cardEntity, productCardEntity.Code, productCardEntity.Passcode, artworkEntity);
                        if (cardBitmap == null)
                        {
                            Log.Warning($"Could not draw {productEntity.Title}/{productCardEntity.Code} - Card Bitmap is NULL");
                            continue;
                        }

                        cardBitmap.Save(outPath, ImageFormat.Jpeg);
                    }
                }
            }

            // TODO : Use waifu to get higher quality link arrows
            // TODO : Cards without artworks
            // TODO : E STAR Hero redo title
            // TODO : Find packs with incomplete set


            // TODO* : Abyss Actor leading Lady needs bullet points instead of a--
            // TODO* : Beatrice card description needs changing in the database to put newline after material 27552504
            // TODO* : Blazing Soul has empty description
            // TODO* : God Neos all on one line
            // TODO* : More fusions on one line
            // TODO* : Remove (Updated from) from card names
            // TODO* : Remove Notes from descriptions
            // TODO* : Replace Exodia artworks with english
            // TODO* : Replace God artworks with bigger cropped ones
            // TODO* : Replace Spirit Message artworks to FINAL
            // TODO* : WingedBeast and SeaSerpent should have space
            // TODO* : @Ignister
            // TODO* : Red Dark Magician Passcode
            // TODO* : Magician's Unite Art Japanese
        }
    }
}
