using System.Collections.Generic;
using System.Linq;

namespace DexTranslate.Api.Mapping
{
    public static class ProjectMapping
    {
        public static IEnumerable<ApiContract.v1.Project> ToContract(this IEnumerable<Model.Project> source) =>
          source.Select(m => m.ToContract());

        public static ApiContract.v1.Project ToContract(this Model.Project source) => new ApiContract.v1.Project
        {
            Key = source.Key,
            Title = source.Title
        };

        public static IEnumerable<Model.Project> FromContract(this IEnumerable<ApiContract.v1.Project> source) =>
            source.Select(m => m.FromContract());

        public static Model.Project FromContract(this ApiContract.v1.Project source) => new Model.Project
        {
            Key = source.Key,
            Title = source.Title
        };
    }
}