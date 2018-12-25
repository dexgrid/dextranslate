using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Service
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetAll();

        Task Add(Language translation);

        Task Update(Language value);

        Task Delete(string languageKey);

        Task<bool> ExistsAsync(string key);
    }
}