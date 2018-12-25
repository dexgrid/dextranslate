namespace DexTranslate.Api.Mapping
{
    public static class TranslationMapping
    {
        public static ApiContract.v1.Translation ToContract(this Model.Translation source) => new ApiContract.v1.Translation
        {
            LanguageKey = source.LanguageKey,
            Key = source.Key,
            ProjectKey = source.ProjectKey,
            Text = source.Text
        };

        public static Model.Translation FromContract(this ApiContract.v1.Translation source) => new Model.Translation
        {
            LanguageKey = source.LanguageKey,
            Key = source.Key,
            ProjectKey = source.ProjectKey,
            Text = source.Text
        };
    }
}