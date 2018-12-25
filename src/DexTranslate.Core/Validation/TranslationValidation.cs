using DexTranslate.Model;

namespace DexTranslate.Core.Validation
{
    public static class TranslationValidation
    {
        public static bool IsValidTranslation(Translation model) =>
            !string.IsNullOrWhiteSpace(model?.LanguageKey) &&
            !string.IsNullOrWhiteSpace(model?.ProjectKey) &&
            !string.IsNullOrWhiteSpace(model?.Key);
    }
}