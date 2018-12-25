using System.IO;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Service
{
    public interface IImportService
    {
        Task ImportTranslations(string languageKey, bool deleteMissingTranslations, Stream stream);
    }
}