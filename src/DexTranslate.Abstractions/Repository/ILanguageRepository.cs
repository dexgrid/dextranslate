using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Repository
{
    public interface ILanguageRepository
    {
        Task Add(Language language);

        Task Delete(string key);

        Task<IEnumerable<Language>> GetAll();

        Task Update(Language value);
    }
}