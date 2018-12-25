using System.Collections.Generic;
using System.Linq;

namespace DexTranslate.Api.Mapping
{
    public static class LanguageMapping
    {
        public static IEnumerable<ApiContract.v1.Language> ToContract(this IEnumerable<Model.Language> source) =>
            source.Select(m => m.ToContract());

        public static ApiContract.v1.Language ToContract(this Model.Language source) => new ApiContract.v1.Language
        {
            Key = source.Key,
            Name = source.Name
        };

        public static IEnumerable<Model.Language> FromContract(this IEnumerable<ApiContract.v1.Language> source) =>
            source.Select(m => m.FromContract());

        public static Model.Language FromContract(this ApiContract.v1.Language source) => new Model.Language
        {
            Key = source.Key,
            Name = source.Name
        };
    }
}