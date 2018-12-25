using System.ComponentModel.DataAnnotations;

namespace DexTranslate.ApiContract.v1
{
    public class Translation
    {
        [Required]
        public string LanguageKey { get; set; }

        [Required]
        public string ProjectKey { get; set; }

        [Required]
        public string Key { get; set; }

        public string Text { get; set; }
    }
}