using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Yugioh.Sync.YugiohFandom.Entities;

namespace Yugioh.Sync.YugiohFandom.Clients
{
    public class YugiohFandomClient : IYugiohFandomClient
    {
        public const string BaseUrl = "https://yugioh.fandom.com";

        private readonly HttpClient _httpClient;
        private readonly string _cachePath;
        public YugiohFandomClient(HttpClient httpClient, string cachePath)
        {
            _httpClient = httpClient;
            _cachePath = cachePath;
        }

        public async Task<CardEntity> GetCardAsync(string passcode)
        {
            var url = $"{BaseUrl}/wiki/{passcode}";
            var filePath = Path.Combine(_cachePath, $"YugiohFandom/{passcode}.html");

            // Download file if we don't have it
            if (!File.Exists(filePath))
            {
                var response = await _httpClient.GetAsync(url);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var fileContent = await response.Content.ReadAsStringAsync();
                        using (var writer = new StreamWriter(filePath))
                        {
                            writer.Write(fileContent);
                        }
                        break;
                    case HttpStatusCode.NotFound:
                        break;
                    default:
                        break;
                }
            }

            if (!File.Exists(filePath))
            {
                // Card not found
                return null;
            }

            var cardEntity = new CardEntity();
            var doc = new HtmlDocument();
            doc.Load(filePath);

            var rows = doc.DocumentNode.SelectNodes("//tr[@class='cardtablerow']");
            foreach (var row in rows)
            {
                var headerNode = row.SelectSingleNode(".//th[@class='cardtablerowheader']");
                var cardDescriptionNode = row.SelectSingleNode(".//td[@class='cardtablespanrow']");

                if (headerNode != null)
                {
                    var headerText = headerNode.InnerText.Trim().Replace(" ", "");

                    if (headerText == "ATK/DEF")
                    {
                        var dataNode = row.SelectSingleNode(".//td[@class='cardtablerowdata']");
                        var dataText = dataNode.InnerText.Trim().Replace(" ", "");
                        string atk = dataText.Split('/').First();
                        string def = dataText.Split('/').Last();

                        cardEntity.Atk = atk;
                        cardEntity.Def = def;
                    }

                    if (headerText == "Types")
                    {
                        var dataNode = row.SelectSingleNode(".//td[@class='cardtablerowdata']");
                        var dataText = dataNode.InnerText.Trim().Replace(" ", "");
                        var types = dataText.Split('/');
                        cardEntity.MonsterTypes = types;
                    }
                }
                else if (cardDescriptionNode != null && cardDescriptionNode.FirstChild.InnerText == "Card descriptions")
                {
                    var tableNodes = cardDescriptionNode.SelectNodes(".//table");

                    foreach (var tableNode in tableNodes)
                    {
                        var language = HttpUtility.HtmlDecode(tableNode.SelectSingleNode(".//th").InnerText).Trim();

                        if (language == "English")
                        {
                            var description = HttpUtility.HtmlDecode(tableNode.SelectSingleNode(".//td[@class='navbox-list']").InnerText).Trim();

                            if (description.StartsWith("Pendulum Effect:"))
                            {
                                string[] d = Regex.Split(description, "Monster Effect:");

                                var pendulumEffect = d.First().Replace("Pendulum Effect:", "").Trim(); 
                                var monsterEffect = d.Last().Trim();

                                cardEntity.Description = monsterEffect;
                                cardEntity.PendulumDescription = pendulumEffect;
                            }
                            else
                            {
                                cardEntity.Description = description;
                            }                    
                        }
                    }
                }
            }

            return cardEntity;
        }
    }
}
