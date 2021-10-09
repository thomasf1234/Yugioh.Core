using HtmlAgilityPack;
using Serilog;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Yugioh.Sync.Yugipedia.Entities;

namespace Yugioh.Sync.Yugipedia.Clients
{
    public class YugipediaClient
    {
        public const string BaseUrl = "https://yugipedia.com";

        private readonly HttpClient _httpClient;
        private readonly string _cachePath;
        public YugipediaClient(HttpClient httpClient, string cachePath)
        {
            _httpClient = httpClient;
            _cachePath = cachePath;
        }

        public async Task<CardEntity> GetCardAsync(string identifier)
        {
            var url = $"{BaseUrl}/wiki/{identifier}";
            var filePath = Path.Combine(_cachePath, $"Yugipedia/{identifier}.html");

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
                        Log.Warning($"Couldn't find card at {url}");
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

            var rows = doc.DocumentNode.SelectSingleNode("//table[@class='innertable']/tbody").ChildNodes;

            foreach (var row in rows)
            {
                var headerNode = row.SelectSingleNode(".//th");
                var cardDescriptionNode = row.SelectSingleNode(".//div[@class='lore']");

                if (headerNode != null)
                {
                    var headerText = HttpUtility.HtmlDecode(headerNode.InnerText).Trim().Replace(" ", "");

                    if (headerText == "Types")
                    {
                        var dataNode = row.SelectSingleNode(".//td");
                        var dataText = HttpUtility.HtmlDecode(dataNode.InnerText).Trim().Replace(" ", "");
                        var types = dataText.Split('/');
                        cardEntity.MonsterTypes = types;
                    }

                    if (headerText == "Level")
                    {
                        var dataNode = row.SelectSingleNode(".//td");
                        var dataText = HttpUtility.HtmlDecode(dataNode.InnerText).Trim().Replace(" ", "");
                        var level = dataText;
                        cardEntity.Level = level;
                    }

                    if (headerText == "Rank")
                    {
                        var dataNode = row.SelectSingleNode(".//td");
                        var dataText = HttpUtility.HtmlDecode(dataNode.InnerText).Trim().Replace(" ", "");
                        var rank = dataText;
                        cardEntity.Rank = rank;
                    }

                    if (headerText == "PendulumScale")
                    {
                        var dataNode = row.SelectSingleNode(".//td");
                        var dataText = HttpUtility.HtmlDecode(dataNode.InnerText).Trim().Replace(" ", "");
                        var pendulumScale = dataText;
                        cardEntity.PendulumScale = pendulumScale;
                    }

                    if (headerText == "ATK/DEF")
                    {
                        var dataNode = row.SelectSingleNode(".//td");
                        var dataText = HttpUtility.HtmlDecode(dataNode.InnerText).Trim().Replace(" ", "");
                        string atk = dataText.Split('/').First();
                        string def = dataText.Split('/').Last();

                        cardEntity.Atk = atk;
                        cardEntity.Def = def;
                    }

                    if (headerText == "ATK/LINK")
                    {
                        var dataNode = row.SelectSingleNode(".//td");
                        var dataText = HttpUtility.HtmlDecode(dataNode.InnerText).Trim().Replace(" ", "");
                        string atk = dataText.Split('/').First();
                        string link = dataText.Split('/').Last();

                        cardEntity.Atk = atk;
                        cardEntity.Link = link;
                    }

                    if (headerText == "Password")
                    {
                        var dataNode = row.SelectSingleNode(".//td");
                        var dataText = HttpUtility.HtmlDecode(dataNode.InnerText).Trim().Replace(" ", "");
                        var passcode = dataText;
                        cardEntity.Passcode = passcode;
                    }
                }
                else if (cardDescriptionNode != null)
                {
                    var description = HttpUtility.HtmlDecode(cardDescriptionNode.InnerText).Trim();
                   
                    if (description.StartsWith("Pendulum Effect\n"))
                    {
                        string[] d = Regex.Split(description, "Monster Effect\n");

                        var pendulumEffect = d.First().Replace("Pendulum Effect\n", "").Trim();
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

            return cardEntity;
        }
    }
}
