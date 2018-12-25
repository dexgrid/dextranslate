using DexTranslate.ApiClient.Internal;
using DexTranslate.ApiContract.v1;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DexTranslate.ApiClient
{
    // Define other methods and classes here
    public class DexTranslateApiClient : IDisposable, IDexTranslateApiClient
    {
        private readonly HttpClient _client;

        private readonly LanguageClient _languageClient;

        private readonly ProjectClient _projectClient;

        private readonly TranslationClient _translationClient;

        public DexTranslateApiClient(string baseUrl, string apiKey, string apiSecret) : this(new HttpClient(), baseUrl, apiKey, apiSecret)
        {
        }

        public DexTranslateApiClient(HttpClient client, string baseUrl, string apiKey, string apiSecret)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("ApiKey", apiKey);
            _client.DefaultRequestHeaders.Add("ApiSecret", apiSecret);

            _languageClient = new LanguageClient(_client, baseUrl);
            _projectClient = new ProjectClient(_client, baseUrl);
            _translationClient = new TranslationClient(_client, baseUrl);
        }

        public async Task AddLanguage(Language language) => await _languageClient.AddLanguage(language);

        public async Task UpdateLanguage(Language language) => await _languageClient.UpdateLanguage(language);

        public async Task<IEnumerable<Language>> GetLanguages() => await _languageClient.GetLanguages();

        public async Task DeleteLanguage(string key) => await _languageClient.DeleteLanguage(key);

        public async Task AddProject(Project project) => await _projectClient.AddProject(project);

        public async Task UpdateProject(Project project) => await _projectClient.UpdateProject(project);

        public async Task<IEnumerable<Project>> GetProjects() => await _projectClient.GetProjects();

        public async Task DeleteProject(string key) => await _projectClient.DeleteProject(key);

        public async Task AddTranslation(Translation translation) => await _translationClient.AddTranslation(translation);

        public async Task UpdateTranslation(Translation translation) => await _translationClient.UpdateTranslation(translation);

        public async Task<IEnumerable<Translation>> GetTranslations(string languageKey, string projectKey) => await _translationClient.GetTranslations(languageKey, projectKey);

        public async Task DeleteTranslation(string languageKey, string projectKey, string key) => await _translationClient.DeleteTranslation(languageKey, projectKey, key);

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
            }
        }
    }
}