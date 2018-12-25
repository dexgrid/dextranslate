using DexTranslate.ApiContract.v1;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DexTranslate.ApiClient.Internal
{
    internal class TranslationClient : BaseDexApiClient
    {
        public TranslationClient(HttpClient client, string baseUrl) : base(client, baseUrl)
        {
        }

        protected override string Url => BaseUrl + nameof(Translation);

        private string GetUrl(string languageKey, string projectKey) => $"{Url}/{languageKey}/{projectKey}/";

        public async Task AddTranslation(Translation translation)
        {
            var response = await Client.PostAsync(GetUrl(translation.LanguageKey, translation.ProjectKey), GetJsonContent(translation));
            await EnsureSuccess(response);
        }

        public async Task UpdateTranslation(Translation translation)
        {
            var response = await Client.PutAsync(GetUrl(translation.LanguageKey, translation.ProjectKey), GetJsonContent(translation));
            await EnsureSuccess(response);
        }

        public async Task<IEnumerable<Translation>> GetTranslations(string languageKey, string projectKey)
        {
            string json = await Client.GetStringAsync(GetUrl(languageKey, projectKey));
            return JsonConvert.DeserializeObject<IEnumerable<Translation>>(json);
        }

        public async Task DeleteTranslation(string languageKey, string projectKey, string key)
        {
            var response = await Client.DeleteAsync(GetUrl(languageKey, projectKey) + "/" + key);
            await EnsureSuccess(response);
        }
    }
}