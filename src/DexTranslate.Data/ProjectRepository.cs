using DexTranslate.Abstractions.Repository;
using DexTranslate.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DexTranslate.Data
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TranslationContext _context;

        public ProjectRepository(TranslationContext context)
        {
            _context = context;
        }

        public async Task Add(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string key)
        {
            var project = await GetByKey(key);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _context.Projects.AnyAsync(m => m.Key == key);
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task Update(Project value)
        {
            if (value.Id == 0)
            {
                value.Id = await GetIdByKey(value.Key);
            }
            _context.Update(value);
            await _context.SaveChangesAsync();
        }

        private async Task<int> GetIdByKey(string key)
        {
            return await _context.Projects.Where(m => m.Key == key).Select(m => m.Id).SingleOrDefaultAsync();
        }

        private async Task<Project> GetByKey(string key)
        {
            return await _context.Projects.SingleOrDefaultAsync(m => m.Key == key);
        }

        public async Task<List<string>> GetLanguagesForProject(int id)
        {
            return await _context.Translations.Where(m => m.ProjectId == id).Select(m => m.Language.Key).Distinct().ToListAsync();
        }
    }
}