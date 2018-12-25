using DexTranslate.ApiClient;
using DexTranslate.ApiContract.v1;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DexTranslate.ApiClientAspNet
{
    public class DexStringLocalizer : IStringLocalizer
    {
        private readonly IDexTranslateApiClient _client;
        private readonly ILogger<DexStringLocalizer> _logger;
        private readonly IOptions<DexTranslateOptions> _options;
        private readonly string _key;
        private readonly ConcurrentDictionary<string, IEnumerable<Translation>> _translationCache = new ConcurrentDictionary<string, IEnumerable<Translation>>();

        public DexStringLocalizer(IDexTranslateApiClient client, ILogger<DexStringLocalizer> logger, IOptions<DexTranslateOptions> options, string key)
        {
            _client = client;
            _logger = logger;
            _options = options;
            _key = key;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetTranslationTextByKey(name);
                return new LocalizedString(name, value, false, string.Empty);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                return this[name];
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return Enumerable.Empty<LocalizedString>();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new DexStringLocalizer(_client, _logger, _options, _key);
        }

        private string GetTranslationTextByKey(string localKey)
        {
            var key = GetTranslationKey(localKey);
            var projectKey = _options.Value.ProjectKey;
            var culture = CultureInfo.CurrentCulture.ToString();

            //TODO: there is still a lot to do here:
            // - Use a real caching mechanism
            // - adding new items should probably happen conditionally
            // - No cache expiration
            if (!_translationCache.ContainsKey(_key))
            {
                var translations = Task.Run(() => _client.GetTranslations(culture, projectKey)).Result;
                _translationCache.TryAdd(_key, translations);
            }

            var translation = _translationCache[_key]?.SingleOrDefault(m => m.Key == key)?.Text;

            if (translation == null)
            {
                try
                {
                    Task.Run(() => _client.AddTranslation(
                        new Translation
                        {
                            Key = key,
                            LanguageKey = culture,
                            ProjectKey = projectKey,
                            Text = localKey,
                        })).Wait();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "failed to insert translation", key);
                }
            }
            return translation ?? localKey;
        }

        private string GetTranslationKey(string name) => _key + "_" + name;
    }
}