using DexTranslate.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Abstractions.Service
{
    public interface IProjectService
    {
        Task<bool> ExistsAsync(string key);

        Task<IEnumerable<Project>> GetAll();

        Task Add(Project project);

        Task Update(Project value);

        Task Delete(string key);

        Task<List<string>> GetLanguagesForProjectsAsync(int id);
    }
}