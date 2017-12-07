using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;


namespace TopDogBot
{
    public class WebClientBase
    {
        private string _urlConfigKey;

        public WebClientBase(string urlConfigKey)
        {
            _urlConfigKey = urlConfigKey;
        }

        protected HttpClient CreateClient()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(ConfigurationManager.AppSettings[_urlConfigKey]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(1, 0, 0, 0);

            return client;
        }

        protected T Get<T>(string uri)
        {
            using (HttpClient client = CreateClient())
            {
                var response = client.GetAsync(uri).Result;

                if (!response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(content);
                }

                return ReadResult<T>(response.Content.ReadAsStreamAsync().Result);
            }
        }

        private static T ReadResult<T>(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}