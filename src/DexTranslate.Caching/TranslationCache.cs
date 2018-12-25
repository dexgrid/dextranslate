using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Caching.Keys;
using DexTranslate.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DexTranslate.Caching
{
    public class TranslationCache : ITranslationCache
    {
        private static SemaphoreSlim s_semaphoreSlim = new SemaphoreSlim(1);

        private readonly ITranslationRepository _repository;
        private readonly IMemoryCache _cache;

        public TranslationCache(ITranslationRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<IEnumerable<Translation>> Get(string languageKey, string projectKey)
        {
            var key = TranslationCacheKey.Create(languageKey, projectKey);
            IEnumerable<Translation> translations = null;

            if (_cache.TryGetValue(key.ToString(), out translations))
            {
                return translations;
            }

            await s_semaphoreSlim.WaitAsync();
            try
            {
                if (_cache.TryGetValue(key.ToString(), out translations))
                {
                    return translations.ToList();
                }

                translations = await _repository.GetAll(languageKey, projectKey);

                var cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddHours(4);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                _cache.Set(key.ToString(), translations, cacheExpirationOptions);
                return translations.ToList();
            }
            finally
            {
                s_semaphoreSlim.Release();
            }
        }

        public void Clear(string languageKey, string projectKey)
        {
            var key = TranslationCacheKey.Create(languageKey, projectKey);
            _cache.Remove(key);
        }
    }
}