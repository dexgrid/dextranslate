using CsvHelper;
using CsvHelper.Configuration;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Abstractions.Service;
using DexTranslate.Core.Mapping;
using DexTranslate.Model.Export;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DexTranslate.Core
{
    public class ExportService : IExportService
    {
        private readonly ITranslationRepository _repository;

        public ExportService(ITranslationRepository repository)
        {
            _repository = repository;
        }

        public async Task<byte[]> GetFileExportAsync(string languageKey, string projectKey)
        {
            var translations = await _repository.GetAll(languageKey, projectKey);
            var translationRows = translations.Select(m => m.Map()).ToList();

            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            using (var csvWriter = new CsvWriter(writer, new Configuration { Delimiter = CultureInfo.CurrentUICulture.TextInfo.ListSeparator }))
            {
                writer.WriteLine("sep=" + CultureInfo.CurrentUICulture.TextInfo.ListSeparator);
                csvWriter.WriteHeader<ExportTranslation>();
                csvWriter.NextRecord();

                foreach (var item in translationRows)
                {
                    csvWriter.WriteRecord(item);
                    csvWriter.NextRecord();
                }

                csvWriter.Flush();

                await writer.FlushAsync();
                return ms.ToArray();
            }
        }
    }
}