using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Service
{
    public interface IExportService
    {
        /// <summary>
        /// Retreives all the language records as csv for a language/project.
        /// This is a rather ineficient implementation that constructs the byte array in memory.
        /// This can be improved by directly writing to the output stream.
        /// </summary>
        /// <param name="languageKey"></param>
        /// <param name="projectKey"></param>
        /// <returns></returns>
        Task<byte[]> GetFileExportAsync(string languageKey, string projectKey);
    }
}