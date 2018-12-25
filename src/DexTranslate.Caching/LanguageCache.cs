using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DexTranslate.Caching
{
    public class LanguageCache : ILanguageCache
    {
        private readonly ILanguageRepository _repository;
        private readonly IMemoryCache _cache;

        private const string LanguageCacheKey = "DexTranslate_LanguageCacheKey";

        public LanguageCache(ILanguageRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public void Clear()
        {
            _cache.Remove(LanguageCacheKey);
        }

        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        public async Task<IEnumerable<Language>> Get()
        {
            IEnumerable<Language> languages = null;

            if (_cache.TryGetValue(LanguageCacheKey, out languages))
            {
                return languages;
            }

            await semaphoreSlim.WaitAsync();
            try
            {
                if (_cache.TryGetValue(LanguageCacheKey, out languages))
                {
                    return languages.ToList();
                }

                languages = await _repository.GetAll();

                var cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddHours(4);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                _cache.Set(LanguageCacheKey, languages, cacheExpirationOptions);
                return languages.ToList(); ;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}