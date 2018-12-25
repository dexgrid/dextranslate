using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Repository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAll();

        Task Add(Project project);

        Task Update(Project value);

        Task Delete(string key);

        Task<bool> ExistsAsync(string key);

        Task<List<string>> GetLanguagesForProject(int id);
    }
}