using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Caching
{
    public interface IProjectCache
    {
        Task<IEnumerable<Project>> Get();

        void Clear();
    }
}