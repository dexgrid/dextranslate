using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Abstractions.Service;
using DexTranslate.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DexTranslate.Core
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageCache _cache;
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageCache cache, ILanguageRepository languageRepository)
        {
            _cache = cache;
            _languageRepository = languageRepository;
        }

        public async Task Add(Language value)
        {
            await _languageRepository.Add(value);
            _cache.Clear();
        }

        public async Task Delete(string languageKey)
        {
            await _languageRepository.Delete(languageKey);
            _cache.Clear();
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return (await GetAll()).Any(m => m.Key == key);
        }

        public async Task<IEnumerable<Language>> GetAll()
        {
            return await _cache.Get();
        }

        public async Task Update(Language value)
        {
            await _languageRepository.Update(value);
            _cache.Clear();
        }
    }
}