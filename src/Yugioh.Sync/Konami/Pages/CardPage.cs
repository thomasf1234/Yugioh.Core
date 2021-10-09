using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Yugioh.Sync.Konami.Constants;
using Yugioh.Sync.Konami.Entities;

namespace Yugioh.Sync.Konami.Pages
{
    public class CardPage
    {
        public int CardId { get; set; }
        public int ArtworkCount { get; set; }
        public List<CardSetInfoEntity> CardSetInfoEntities { get; set; } 

        public CardPage(IWebDriver driver)
        {
            var imageThumbnailElements = driver.FindElement(By.Id("thumbnail")).FindElements(By.XPath("./img"));
            ArtworkCount = imageThumbnailElements.Count;

            CardSetInfoEntities = new List<CardSetInfoEntity>();

            var cardSetInfoElements = driver.FindElements(By.XPath("//div[@id='pack_list']/table/tbody/tr[@class='row']"));

            for (int i = 0; i < cardSetInfoElements.Count; ++i)
            {
                var cardSetInfoElement = cardSetInfoElements[i];
                
                string productRequestPath = cardSetInfoElement.FindElement(By.XPath("./td/input[@class='link_value']")).GetAttribute("value");
                string productUrl = UrlConstants.BaseUrl + productRequestPath;
                var productUri = new Uri(productUrl);
                string productId = HttpUtility.ParseQueryString(productUri.Query).Get("pid");

                var productColumns = cardSetInfoElement.FindElements(By.XPath("./td"));

                string launchDateString = productColumns[0].Text.Trim();
                string cardNumber = productColumns[1].Text.Trim().ToUpper();
                
                // Wierd issue were it's incorrect in the official db
                if (cardNumber == "JMP-EN008") { cardNumber = "JUMP-EN008"; }
                if (cardNumber == "JMP-EN007") { cardNumber = "JUMP-EN007"; }
                if (cardNumber == "JMP-EN006") { cardNumber = "JUMP-EN006"; }

                string productTitle = productColumns[2].Text.Trim().ToUpper();

                var productEntity = new ProductEntity()
                {
                    Url = productUrl,
                    ProductId = productId,
                    Title = productTitle
                };

                if (!string.IsNullOrEmpty(launchDateString))
                {
                    productEntity.LaunchDate = DateTime.Parse(launchDateString);
                }

                string rarity = "Common";
                try
                {
                    var rarityImageElement = productColumns[3].FindElement(By.XPath("./img"));
                    rarity = Path.GetFileNameWithoutExtension(rarityImageElement.GetAttribute("alt").Trim());
                }
                catch (NoSuchElementException) { }

                

                var cardSetInfoEntity = new CardSetInfoEntity()
                {
                    ProductEntity = productEntity,
                    Number = cardNumber,
                    Rarity = rarity
                };

                CardSetInfoEntities.Add(cardSetInfoEntity);
            }
        }
    }
}
