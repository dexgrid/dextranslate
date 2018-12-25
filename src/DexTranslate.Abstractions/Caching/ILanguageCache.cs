using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Caching
{
    public interface ILanguageCache
    {
        Task<IEnumerable<Language>> Get();

        void Clear();
    }
}