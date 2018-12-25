using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Repository
{
    public interface ITranslationRepository
    {
        Task Add(Translation translation);

        Task Delete(string languageKey, string projectName, string key);

        Task<IEnumerable<Translation>> GetAll(string language, string projectKey);

        Task<Translation> GetByKey(string languageKey, string projectName, string key);

        Task Update(Translation value);

        Task<bool> ExistsAsync(string languageKey, string projectKey, string key);

        Task DeleteAll(string languageKey, string projectKey);
    }
}