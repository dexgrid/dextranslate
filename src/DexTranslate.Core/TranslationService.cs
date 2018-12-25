using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Abstractions.Service;
using DexTranslate.Core.Validation;
using DexTranslate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DexTranslate.Core.Validation.TranslationValidation;

namespace DexTranslate.Core
{
    public class TranslationService : ITranslationService
    {
        private readonly ILanguageService _languageService;
        private readonly IProjectService _projectService;
        private readonly ITranslationCache _cache;
        private readonly ITranslationRepository _repository;

        public TranslationService(ILanguageService languageService, IProjectService projectService, ITranslationCache cache, ITranslationRepository repository)
        {
            _languageService = languageService;
            _projectService = projectService;
            _cache = cache;
            _repository = repository;
        }

        public async Task Add(Translation value)
        {
            await ValidateTranslation(value);
            await _repository.Add(value);
            _cache.Clear(value.LanguageKey, value.ProjectKey);
        }

        public async Task DeleteAsync(string languageKey, string projectKey, string key)
        {
            await _repository.Delete(languageKey, projectKey, key);
            _cache.Clear(languageKey, projectKey);
        }

        public async Task<bool> ExistsAsync(string languageKey, string projectKey, string key)
        {
            return (await _cache.Get(languageKey, projectKey)).Any(m => m.Key == key);
        }

        public async Task<IEnumerable<Translation>> GetAll(string languageKey, string projectKey)
        {
            return await _cache.Get(languageKey, projectKey);
        }

        public async Task<Translation> GetByKey(string languageKey, string projectKey, string key)
        {
            return (await _cache.Get(languageKey, projectKey)).SingleOrDefault(m => m.Key == key);
        }

        public async Task PurgeRecordsAsync(string languageKey, string projectKey)
        {
            await _repository.DeleteAll(languageKey, projectKey);
            _cache.Clear(languageKey, projectKey);
        }

        public bool RecordsAreValid(IEnumerable<Translation> records) => records.All(TranslationValidation.IsValidTranslation);

        public async Task Update(Translation value)
        {
            await ValidateTranslation(value);
            await _repository.Update(value);
            _cache.Clear(value.LanguageKey, value.ProjectKey);
        }

        private async Task ValidateTranslation(Translation value)
        {
            if (!IsValidTranslation(value))
            {
                throw new InvalidOperationException("Translation is invalid");
            }

            if (!await _projectService.ExistsAsync(value.ProjectKey) || !await _languageService.ExistsAsync(value.LanguageKey))
            {
                throw new ApplicationException("Language or Project does not exist");
            }
        }
    }
}