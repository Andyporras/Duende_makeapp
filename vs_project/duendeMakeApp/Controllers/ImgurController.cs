using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace duendeMakeApp.Controllers
{
    public class ImgurController
    {
        private static ImgurController instance = null;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _clienteId;

        private ImgurController(IHttpClientFactory httpClientFactory)
        {
            _clientFactory = httpClientFactory;
            _clienteId = "78aeee7d43a72ee";
        }

        public static ImgurController GetInstance(IHttpClientFactory httpClientFactory)
        {
            return instance ?? (instance = new ImgurController(httpClientFactory));
        }

        public async Task<string> SubirImagen(IFormFile imageFile)
        {
            using (HttpClient httpClient = _clientFactory.CreateClient())
            {
                httpClient.BaseAddress = new Uri("https://api.imgur.com/3/image");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Client-ID {_clienteId}");

                using (Stream stream = imageFile.OpenReadStream())
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(stream), "image", imageFile.FileName);

                    using (HttpResponseMessage response = await httpClient.PostAsync("image", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();
                            int startIndex = responseData.IndexOf("\"link\":\"") + 8;
                            int endIndex = responseData.IndexOf("\"", startIndex);
                            if (startIndex >= 8 && endIndex > startIndex)
                            {
                                string enlaceImagen = responseData.Substring(startIndex, endIndex - startIndex);
                                return enlaceImagen;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
