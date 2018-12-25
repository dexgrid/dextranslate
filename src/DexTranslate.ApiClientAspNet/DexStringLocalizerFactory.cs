using DexTranslate.ApiClient;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

namespace DexTranslate.ApiClientAspNet
{
    public class DexStringLocalizerFactory : IStringLocalizerFactory, IDisposable
    {
        private static readonly ConcurrentDictionary<string, IStringLocalizer> _resourceLocalizations = new ConcurrentDictionary<string, IStringLocalizer>();
        private readonly ILogger<DexStringLocalizer> _logger;
        private readonly IOptions<DexTranslateOptions> _options;
        private readonly IDexTranslateApiClient _client;

        public DexStringLocalizerFactory(ILogger<DexStringLocalizer> logger, IOptions<DexTranslateOptions> options)
        {
            _logger = logger;
            _options = options;
            _client = new DexTranslateApiClient(options.Value.BaseUrl, options.Value.ApiKey, options.Value.ApiSecret);
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            if (_resourceLocalizations.Keys.Contains(resourceSource.FullName))
            {
                return _resourceLocalizations[resourceSource.FullName];
            }

            var localizer = new DexStringLocalizer(_client, _logger, _options, resourceSource.FullName);
            return _resourceLocalizations.GetOrAdd(resourceSource.FullName, localizer);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            string key = baseName + location;

            if (_resourceLocalizations.ContainsKey(key))
            {
                return _resourceLocalizations[key];
            }

            var localizer = new DexStringLocalizer(_client, _logger, _options, key);
            return _resourceLocalizations.GetOrAdd(key, localizer);
        }

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