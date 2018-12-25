using System.ComponentModel.DataAnnotations;

namespace DexTranslate.ApiContract.v1
{
    public class Project
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Title { get; set; }
    }
}