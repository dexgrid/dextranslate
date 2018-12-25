using System.ComponentModel.DataAnnotations;

namespace DexTranslate.ApiContract.v1
{
    public class Language
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }
    }
}