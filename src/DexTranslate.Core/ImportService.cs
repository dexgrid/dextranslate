using CsvHelper;
using CsvHelper.Configuration;
using DexTranslate.Abstractions.Service;
using DexTranslate.Core.Mapping;
using DexTranslate.Model.Export;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DexTranslate.Core
{
    public class ImportService : IImportService
    {
        private readonly ITranslationService _service;

        public ImportService(ITranslationService service)
        {
            _service = service;
        }

        public async Task ImportTranslations(string languageKey, bool deleteMissingTranslations, Stream stream)
        {
            using (var sr = new StreamReader(stream))
            using (var reader = new CsvReader(sr, new Configuration { HasHeaderRecord = true, Delimiter = CultureInfo.CurrentUICulture.TextInfo.ListSeparator }))
            {
                var rawRecords = reader.GetRecords<ExportTranslation>().ToList();
                var records = rawRecords.Map();

                if (!records.Any())
                {
                    throw new ApplicationException("There are no records in this file that could be imported");
                }

                if (!_service.RecordsAreValid(records))
                {
                    throw new ApplicationException("Some records could not be parsed");
                }

                if (records.Any(translation => translation.LanguageKey != languageKey))
                {
                    throw new ApplicationException($"Some of the translations in the CSV file are not for the expected language: \"{languageKey}\" ");
                }

                var firstItem = records.First();
                await _service.PurgeRecordsAsync(firstItem.LanguageKey, firstItem.ProjectKey);

                foreach (var item in records)
                {
                    if (await _service.ExistsAsync(item.LanguageKey, item.ProjectKey, item.Key))
                    {
                        await _service.Update(item);
                    }
                    else
                    {
                        await _service.Add(item);
                    }
                }
            }
        }
    }
}