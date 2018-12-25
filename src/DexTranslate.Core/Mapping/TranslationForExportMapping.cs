using DexTranslate.Model.Export;
using System.Collections.Generic;
using System.Linq;

namespace DexTranslate.Core.Mapping
{
    internal static class TranslationForExportMapping
    {
        internal static ExportTranslation Map(this Model.Translation source)
        {
            return new ExportTranslation
            {
                Key = source.Key,
                LanguageKey = source.LanguageKey,
                ProjectKey = source.ProjectKey,
                Text = source.Text
            };
        }

        internal static IEnumerable<Model.Translation> Map(this IEnumerable<ExportTranslation> source) => source.Select(m => m.Map());

        internal static Model.Translation Map(this ExportTranslation source)
        {
            return new Model.Translation
            {
                Key = source.Key,
                LanguageKey = source.LanguageKey,
                ProjectKey = source.ProjectKey,
                Text = source.Text
            };
        }
    }
}