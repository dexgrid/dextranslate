using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Abstractions.Service;
using DexTranslate.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DexTranslate.Core
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectCache _cache;
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectCache cache, IProjectRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public async Task Add(Project project)
        {
            await _repository.Add(project);
            _cache.Clear();
        }

        public async Task Delete(string key)
        {
            await _repository.Delete(key);
            _cache.Clear();
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return (await _cache.Get()).Any(m => m.Key == key);
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            return await _cache.Get();
        }

        public async Task<List<string>> GetLanguagesForProjectsAsync(int id)
        {
            return await _repository.GetLanguagesForProject(id);
        }

        public async Task Update(Project value)
        {
            await _repository.Update(value);
            _cache.Clear();
        }
    }
}