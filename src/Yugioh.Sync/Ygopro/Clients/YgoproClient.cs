using Newtonsoft.Json;
using Serilog;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Yugioh.Sync.Ygopro.Responses;

namespace Yugioh.Sync.Ygopro.Clients
{
    public class YgoproClient : IYgoproClient
    {
        public const string BaseUrl = "https://db.ygoprodeck.com";
        public const string ArtworkUrl = "https://storage.googleapis.com/ygoprodeck.com/pics_artgame";

        private readonly string _apiVersion;
        private readonly HttpClient _httpClient;
        private readonly string _cachePath;
        public YgoproClient(HttpClient httpClient, string cachePath)
        {
            _httpClient = httpClient;
            _apiVersion = "v7";
            _cachePath = cachePath;
        }

        public YgoproClient(HttpClient httpClient, string apiVersion, string cachePath)
        {
            _httpClient = httpClient;
            _apiVersion = apiVersion;
            _cachePath = cachePath;
        }

        public async Task<GetCardsResponse> GetCardsAsync()
        {
            string url = $"{BaseUrl}/api/{_apiVersion}/cardinfo.php";
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
            HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;

            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    var responseJson = await httpResponseMessage.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<GetCardsResponse>(responseJson);
                default:
                    throw new Exception($"Received response code: {httpStatusCode}");
            }
        }

        public async Task<Image> GetImageAsync(int imageId)
        {
            var url = $"{ArtworkUrl}/{imageId}.jpg";
            var rawFilePath = Path.Combine(_cachePath, $"Artworks/Raw/{imageId}.jpg");
            var formattedFilePath = Path.Combine(_cachePath, $"Artworks/Formatted/{imageId}.jpg");

            // Download raw file if we don't have it
            if (!File.Exists(rawFilePath))
            {
                // Below code can be used to download the image
                var response = await _httpClient.GetAsync(url);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var imageBytes = await response.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes(rawFilePath, imageBytes);
                        
                        break;
                    case HttpStatusCode.NotFound:
                        Log.Warning($"Couldn't find image {imageId}");
                        break;
                    default:
                        break;
                }         
            }

            // Use the formatted file if we have it
            if (File.Exists(formattedFilePath))
            {
                return Image.FromFile(formattedFilePath);
            }  
            else if (File.Exists(rawFilePath))
            {
                Log.Information($"Must format image {rawFilePath} and move to {formattedFilePath}");
                /*              
                    # To format the image using BIMP, a Gimp plugin for batch image processing
                    # 1st step: plug-in-gmic-qt 
                    #   input layers mode: 1
                    #   output mode: 0
                    #   command: iain_fast_denoise_p 0,0,1,0.8,0,0,1
                    # 2nd step: gimp-levels
                    #   HISTOGRAM-VALUE 15,255,1,0,255
                 */    
            }

            return null;
        }
    }
}
