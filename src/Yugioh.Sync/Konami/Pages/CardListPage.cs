using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Web;
using Yugioh.Sync.Konami.Entities;
using Yugioh.Sync.Konami.Constants;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace Yugioh.Sync.Konami.Pages
{
    public class CardListPage
    {
        public List<CardEntity> CardEntities { get; set; }
        public CardListPage(IWebDriver driver)
        {
            CardEntities = new List<CardEntity>();

            // Get cards list
            var cardElements = driver.FindElements(By.XPath("//div[@class='list_style']/ul/li"));

            foreach (var cardElement in cardElements)
            {
                string cardRequestPath = cardElement.FindElement(By.XPath("./input[@class='link_value']")).GetAttribute("value");

                string cardUrl = UrlConstants.BaseUrl + cardRequestPath;
                var uri = new Uri(cardUrl);
                var query = HttpUtility.ParseQueryString(uri.Query);
                int cardId = Convert.ToInt32(query.Get("cid"));

                var cardEntity = new CardEntity()
                {
                    CardId = cardId,
                    Url = cardUrl
                };

                var nameElement = cardElement.FindElement(By.XPath("./dl/dt[@class='box_card_name']"));
                string name = Regex.Replace(nameElement.Text, " \\(Updated from:.+$", "").Trim();
                cardEntity.Name = name;

                var attributeElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_spec']/span[@class='box_card_attribute']"));
                string attribute = attributeElement.Text.Trim();
                cardEntity.Attribute = attribute;

                var descriptionElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_text']"));
                string description = Regex.Replace(descriptionElement.Text, "Notes*(\n|\r\n)Card Name updated from.*$", "").Trim();
                cardEntity.Description = description;

                if (attribute == "SPELL" || attribute == "TRAP")
                {
                    string property = "Normal";
                    try
                    {
                        var propertyElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_spec']/span[@class='box_card_effect']"));
                        property = propertyElement.Text.Trim();
                    }
                    catch (NoSuchElementException) { }

                    cardEntity.Property = property;
                }
                else
                {
                    try
                    {
                        var levelElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_spec']/span[@class='box_card_level_rank level']"));
                        cardEntity.Level = Convert.ToInt32(Regex.Match(levelElement.Text.Trim(), @"\d+").Value);
                    }
                    catch (NoSuchElementException) { }

                    try
                    {
                        var rankElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_spec']/span[@class='box_card_level_rank rank']"));
                        cardEntity.Rank = Convert.ToInt32(Regex.Match(rankElement.Text.Trim(), @"\d+").Value);
                    }
                    catch (NoSuchElementException) { }

                    try
                    {
                        var pendulumElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_pen_info']"));

                        var pendulumScaleElement = pendulumElement.FindElement(By.XPath("./span[@class='box_card_pen_scale']"));
                        int pendulumScale = Convert.ToInt32(pendulumScaleElement.Text.Trim());
                        cardEntity.PendulumScale = pendulumScale;

                        var pendulumEffectElement = pendulumElement.FindElement(By.XPath("./span[@class='box_card_pen_effect']"));
                        string pendulumDescription = pendulumEffectElement.Text.Trim();
                        cardEntity.PendulumDescription = pendulumDescription;
                    }
                    catch (NoSuchElementException) { }
                    
                    try
                    {
                        var linkElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_spec']/span[@class='box_card_linkmarker']"));
                        cardEntity.Link = Convert.ToInt32(Regex.Match(linkElement.Text.Trim(), @"\d+").Value);

                        var linkMarkersElement = linkElement.FindElement(By.XPath("./img[@title='Link']"));
                        string linkMarkerIndexes = Regex.Match(Path.GetFileNameWithoutExtension(linkMarkersElement.GetAttribute("src")), @"\d+").Value;

                        var linkMarkers = new List<LinkMarker>();
                        foreach (char linkMarkerIndex in linkMarkerIndexes.ToCharArray())
                        {
                            LinkMarker linkMarker = (LinkMarker)int.Parse(linkMarkerIndex.ToString());
                            linkMarkers.Add(linkMarker);
                        };

                        cardEntity.LinkMarkers = linkMarkers;
                    }
                    catch (NoSuchElementException) { }
                    

                    var monsterTypesElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_spec']/span[@class='card_info_species_and_other_item']"));
                    cardEntity.MonsterTypes = monsterTypesElement.Text.Trim().Replace("[", "").Replace("]", "").Split('/').Select(mt => mt.Trim()).ToList();

                    var atkElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_spec']/span[@class='atk_power']"));
                    string atk = Regex.Match(atkElement.Text.Trim(), @"[^ ]+$").Value;
                    cardEntity.Atk = atk;

                    if (cardEntity.Link == null)
                    {
                        var defElement = cardElement.FindElement(By.XPath("./dl/dd[@class='box_card_spec']/span[@class='def_power']"));
                        string def = Regex.Match(defElement.Text.Trim(), @"[^ ]+$").Value;
                        cardEntity.Def = def;
                    }
                }

                CardEntities.Add(cardEntity);
            }
        }
    }
}
