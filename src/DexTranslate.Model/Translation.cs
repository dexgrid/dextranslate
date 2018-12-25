namespace DexTranslate.Model
{
    public class Translation
    {
        public int Id { get; set; }

        public string LanguageKey { get; set; }

        public int LanguageId { get; set; }

        public Language Language { get; set; }

        public string ProjectKey { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public string Key { get; set; }

        public string Text { get; set; }
    }
}