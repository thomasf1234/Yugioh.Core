using Microsoft.Data.Sqlite;
using Serilog;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Yugioh.Data.Data;
using Yugioh.Data.Entities;
using Yugioh.Data.Repositories;
using Yugioh.Draw.Repositories;
using Yugioh.Sync.Factories;
using Yugioh.Sync.Utils;
using Yugioh.Sync.Ygopro.Clients;
using Yugioh.Sync.Ygopro.Responses;
using Yugioh.Sync.Yugipedia.Clients;

namespace Yugioh.Sync
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("consoleapp.log")
                .WriteTo.Console()
                .CreateLogger();

            string projectPath = Path.GetFullPath("../..");
            string cachePath = Path.Combine(projectPath, "Cache");
            string dbPath = Path.Combine(projectPath, "Yugioh.sqlite");

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                var schemaMigration = new SchemaMigration(connection);
                await schemaMigration.ApplyAsync();

                var cardRepository = new CardRepository();
                var artworkRepository = new ArtworkRepository();
                var cardSetRepository = new CardSetRepository();

                var resourceRepository = new ResourceRepository();
                var cardBitmapFactory = new CardBitmapFactory(resourceRepository);

                var httpClient = new HttpClient();
                var ygoproClient = new YgoproClient(httpClient, cachePath);
                GetCardsResponse getCardsResponse = await ygoproClient.GetCardsAsync();
                var ygoproCardEntities = getCardsResponse.Data.OrderBy(c => c.Name).ToList();
                var cardCount = ygoproCardEntities.Count();

                for (int i=0; i<cardCount; ++i)
                {
                    var ygoproCardEntity = ygoproCardEntities[i];
                    string passcode = ygoproCardEntity.Id.ToString().PadLeft(8, '0');

                    CardEntity cardEntity = await cardRepository.FindCardAsync(ygoproCardEntity.Id, connection);

                    bool newCard = cardEntity == null;

                    if (newCard)
                    {
                        Log.Information($"### Synchronizing CardId {ygoproCardEntity.Id} - {i}/{cardCount} \"{ygoproCardEntity.Name}\"");

                        switch (ygoproCardEntity.Type)
                        {
                            case "Spell Card":
                                cardEntity = new CardEntity()
                                {
                                    CardId = ygoproCardEntity.Id,
                                    Type = Data.Entities.Type.Spell,
                                    Name = ygoproCardEntity.Name,
                                    Attribute = Data.Entities.Attribute.Spell,
                                    Property = (Data.Entities.Property)Enum.Parse(typeof(Data.Entities.Property), ygoproCardEntity.Race.Replace('-', '_')),
                                    Description = ygoproCardEntity.Desc,
                                    Passcode = passcode
                                };

                                break;
                            case "Trap Card":
                                cardEntity = new CardEntity()
                                {
                                    CardId = ygoproCardEntity.Id,
                                    Type = Data.Entities.Type.Trap,
                                    Name = ygoproCardEntity.Name,
                                    Attribute = Data.Entities.Attribute.Trap,
                                    Property = (Data.Entities.Property)Enum.Parse(typeof(Data.Entities.Property), ygoproCardEntity.Race.Replace('-', '_')),
                                    Description = ygoproCardEntity.Desc,
                                    Passcode = passcode
                                };

                                break;
                            default:
                                var yugipediaClient = new YugipediaClient(httpClient, cachePath);
                                var yugipediaCardEntity = await yugipediaClient.GetCardAsync(passcode);

                                // We can't ascertain the monster types from ygopro API alone so skip
                                if (yugipediaCardEntity == null)
                                {
                                    Log.Warning($"Skipping {passcode} - yugipediaCardEntity is null");
                                    break;
                                }

                                // Default MonsterTypes
                                if (yugipediaCardEntity.MonsterTypes == null)
                                {
                                    yugipediaCardEntity.MonsterTypes = new string[] { "Normal" };
                                }

                                cardEntity = new CardEntity()
                                {
                                    CardId = ygoproCardEntity.Id,
                                    Type = Data.Entities.Type.Monster,
                                    Name = ygoproCardEntity.Name,
                                    Attribute = (Data.Entities.Attribute)Enum.Parse(typeof(Data.Entities.Attribute), StringUtil.Capitalize(ygoproCardEntity.Attribute.ToLower())),
                                    Race = ygoproCardEntity.Race,
                                    MonsterTypes = 0,
                                    Abilities = 0,
                                    Attack = yugipediaCardEntity.Atk,
                                    Passcode = passcode
                                };

                                if (yugipediaCardEntity.Level != null)
                                {
                                    cardEntity.Level = Convert.ToByte(yugipediaCardEntity.Level);
                                    cardEntity.Defense = yugipediaCardEntity.Def;
                                }
                                else if (yugipediaCardEntity.Rank != null)
                                {
                                    cardEntity.Rank = Convert.ToByte(yugipediaCardEntity.Rank);
                                    cardEntity.Defense = yugipediaCardEntity.Def;
                                }
                                else if (yugipediaCardEntity.Link != null)
                                {
                                    cardEntity.LinkRating = Convert.ToByte(yugipediaCardEntity.Link);
                                    cardEntity.LinkArrows = 0;

                                    // TODO : Fetch link arrows from yugipedia?
                                    foreach (var linkMarker in ygoproCardEntity.LinkMarkers)
                                    {
                                        cardEntity.LinkArrows = cardEntity.LinkArrows | (Data.Entities.LinkArrows)Enum.Parse(typeof(Data.Entities.LinkArrows), linkMarker.Replace("-", ""));
                                    }
                                }

                                if (yugipediaCardEntity.PendulumScale != null)
                                {
                                    cardEntity.PendulumScale = Convert.ToByte(yugipediaCardEntity.PendulumScale);
                                    cardEntity.Description = yugipediaCardEntity.Description;
                                    cardEntity.PendulumDescription = yugipediaCardEntity.PendulumDescription == null ? "" : yugipediaCardEntity.PendulumDescription;
                                }
                                else
                                {
                                    cardEntity.Description = ygoproCardEntity.Desc.Trim();
                                }

                                foreach (string monsterType in Enum.GetNames(typeof(Data.Entities.MonsterTypes)))
                                {
                                    if (yugipediaCardEntity.MonsterTypes.Contains(monsterType))
                                    {
                                        cardEntity.MonsterTypes = cardEntity.MonsterTypes | (Data.Entities.MonsterTypes)Enum.Parse(typeof(Data.Entities.MonsterTypes), monsterType);
                                    }
                                }

                                foreach (string ability in Enum.GetNames(typeof(Data.Entities.Abilities)))
                                {
                                    if (yugipediaCardEntity.MonsterTypes.Contains(ability))
                                    {
                                        cardEntity.Abilities = cardEntity.Abilities | (Data.Entities.Abilities)Enum.Parse(typeof(Data.Entities.Abilities), ability);
                                    }
                                }

                                if (cardEntity.Description.StartsWith("FLIP:"))
                                {
                                    cardEntity.Abilities = cardEntity.Abilities | Data.Entities.Abilities.Flip;
                                }

                                break;
                        }

                        if (cardEntity != null)
                        {
                            await cardRepository.InsertCardAsync(cardEntity, connection);
                            Log.Information($"Inserting Card {passcode}");
                        }
                    }

                    if (cardEntity != null)
                    {
                        for (int j = 0; j < ygoproCardEntity.CardImageEntities.Count; ++j)
                        {
                            var cardImageEntity = ygoproCardEntity.CardImageEntities[j];

                            bool hasExistingArtwork = await artworkRepository.HasArtworkAsync(cardImageEntity.Id, connection);

                            if (hasExistingArtwork)
                            {
                                ArtworkEntity _artworkEntity = await artworkRepository.FindArtworkAsync(cardImageEntity.Id, connection);

                                // TODO : MOVE
                                string outPath = $"Output\\{cardEntity.CardId}_{_artworkEntity.ArtworkId}.jpg";

                                if (!File.Exists(outPath))
                                {
                                    Bitmap cardBitmap = await cardBitmapFactory.CreateCardBitmapAsync(cardEntity, _artworkEntity);
                                    if (cardBitmap == null || cardEntity.Name.Contains("@"))
                                    {
                                        Log.Warning($"Could not draw {cardEntity.CardId} - {cardEntity.Name}");
                                    }
                                    else
                                    {
                                        // TODO : @ in the card name
                                        // TODO : Fusion materials on one line
                                        cardBitmap.Save(outPath, ImageFormat.Jpeg);
                                    }
                                }
                                
                                continue;
                            }

                            Log.Information($"Synchronizing ImageId {cardImageEntity.Id}");

                            bool isAlternate = j > 0;
                            Image image = await ygoproClient.GetImageAsync(cardImageEntity.Id);

                            if (image == null)
                            {
                                Log.Warning($"Skipping ImageId {cardImageEntity.Id} - image is null");
                                continue;
                            }

                            var artworkEntity = new ArtworkEntity()
                            {
                                ArtworkId = cardImageEntity.Id,
                                CardId = cardEntity.CardId,
                                Alternate = isAlternate,
                                Image = image
                            };

                            await artworkRepository.InsertArtworkAsync(artworkEntity, connection);
                        }


                        // TMP to draw all cards


                        /*if (ygoproCardEntity.CardSetEntities != null)
                        {
                            var cardSetEntities = new List<CardSetEntity>();

                            foreach (var ygoproCardSetEntity in ygoproCardEntity.CardSetEntities)
                            {
                                var cardSetEntity = new CardSetEntity()
                                {
                                    Number = ygoproCardSetEntity.SetCode,
                                    CardId = cardEntity.CardId,
                                    ArtworkId = ygoproCardEntity.CardImageEntities[0].Id
                                };

                                if (!cardSetEntities.Select(cse => cse.Number).Contains(cardSetEntity.Number))
                                {
                                    cardSetEntities.Add(cardSetEntity);
                                }   
                            }

                            foreach (var cardSetEntity in cardSetEntities)
                            {
                                await cardSetRepository.InsertCardSetAsync(cardSetEntity, connection);
                                Console.WriteLine($"{DateTime.Now} :: INSERTED {passcode}/{cardSetEntity.Number}");
                            }                         
                        }*/
                    }    
                }
            }
        }
    }
}
