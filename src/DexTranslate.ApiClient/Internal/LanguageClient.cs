using DexTranslate.ApiContract.v1;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DexTranslate.ApiClient.Internal
{
    internal class LanguageClient : BaseDexApiClient
    {
        protected override string Url => BaseUrl + nameof(Language);

        public LanguageClient(HttpClient client, string baseUrl) : base(client, baseUrl)
        {
        }

        public async Task AddLanguage(Language language)
        {
            var response = await Client.PostAsync(Url, GetJsonContent(language));
            await EnsureSuccess(response);
        }

        public async Task UpdateLanguage(Language language)
        {
            var response = await Client.PutAsync(Url, GetJsonContent(language));
            await EnsureSuccess(response);
        }

        public async Task<IEnumerable<Language>> GetLanguages()
        {
            string json = await Client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Language>>(json);
        }

        public async Task DeleteLanguage(string key)
        {
            var response = await Client.DeleteAsync(Url + "/" + key);
            await EnsureSuccess(response);
        }
    }
}