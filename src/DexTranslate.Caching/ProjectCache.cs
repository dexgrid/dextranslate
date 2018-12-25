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
    public class ProjectCache : IProjectCache
    {
        private readonly IProjectRepository _repository;
        private readonly IMemoryCache _cache;

        private const string ProjectCacheKey = "DexTranslate_ProjectCacheKey";

        public ProjectCache(IProjectRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public void Clear()
        {
            _cache.Remove(ProjectCacheKey);
        }

        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        public async Task<IEnumerable<Project>> Get()
        {
            IEnumerable<Project> projects = null;

            if (_cache.TryGetValue(ProjectCacheKey, out projects))
            {
                return projects;
            }

            await semaphoreSlim.WaitAsync();
            try
            {
                if (_cache.TryGetValue(ProjectCacheKey, out projects))
                {
                    return projects.ToList();
                }

                projects = await _repository.GetAll();

                var cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddHours(4);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                _cache.Set(ProjectCacheKey, projects, cacheExpirationOptions);
                return projects.ToList(); ;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}