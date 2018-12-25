using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Service
{
    public interface ITranslationService
    {
        Task<IEnumerable<Translation>> GetAll(string languageKey, string projectKey);

        Task<Translation> GetByKey(string languageKey, string projectKey, string key);

        Task Add(Translation translation);

        Task Update(Translation value);

        Task DeleteAsync(string languageKey, string projectKey, string key);

        Task<bool> ExistsAsync(string languageKey, string projectKey, string key);

        bool RecordsAreValid(IEnumerable<Translation> records);

        Task PurgeRecordsAsync(string languageKey, string projectKey);
    }
}