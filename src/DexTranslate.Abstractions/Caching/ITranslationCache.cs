using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Caching
{
    public interface ITranslationCache
    {
        Task<IEnumerable<Translation>> Get(string languageKey, string projectKey);

        void Clear(string languageKey, string projectKey);
    }
}