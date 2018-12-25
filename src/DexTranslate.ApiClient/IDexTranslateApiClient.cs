using DexTranslate.ApiContract.v1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.ApiClient
{
    public interface IDexTranslateApiClient
    {
        Task AddLanguage(Language language);

        Task AddProject(Project project);

        Task AddTranslation(Translation translation);

        Task DeleteLanguage(string key);

        Task DeleteProject(string key);

        Task DeleteTranslation(string languageKey, string projectKey, string key);

        void Dispose();

        Task<IEnumerable<Language>> GetLanguages();

        Task<IEnumerable<Project>> GetProjects();

        Task<IEnumerable<Translation>> GetTranslations(string languageKey, string projectKey);

        Task UpdateLanguage(Language language);

        Task UpdateProject(Project project);

        Task UpdateTranslation(Translation translation);
    }
}