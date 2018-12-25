using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DexTranslate.ApiClient.Internal
{
    internal abstract class BaseDexApiClient
    {
        public BaseDexApiClient(HttpClient client, string baseUrl)
        {
            Client = client;
            BaseUrl = baseUrl;
        }

        protected async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.Conflict)
            {
                throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }

        public string BaseUrl { get; }

        protected abstract string Url { get; }

        protected HttpClient Client { get; }

        protected StringContent GetJsonContent(object source)
        {
            string json = JsonConvert.SerializeObject(source, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }
    }
}