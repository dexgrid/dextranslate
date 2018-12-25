namespace DexTranslate.Caching.Keys
{
    public class TranslationCacheKey
    {
        public string LanguageKey { get; set; }

        public string ProjectKey { get; set; }

        public static TranslationCacheKey Create(string languageKey, string projectKey)
        {
            return new TranslationCacheKey { LanguageKey = languageKey, ProjectKey = projectKey };
        }

        public override string ToString() => $"{LanguageKey}_{ProjectKey}";
    }
}