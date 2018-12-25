using DexTranslate.Abstractions.Repository;
using DexTranslate.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DexTranslate.Data
{
    public class TranslationRepository : ITranslationRepository
    {
        private readonly TranslationContext _context;

        public TranslationRepository(TranslationContext context)
        {
            _context = context;
        }

        public async Task Add(Translation translation)
        {
            await PopulateProjectAndLanguageId(translation);
            await _context.Translations.AddAsync(translation);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string languageKey, string projectName, string key)
        {
            var item = await _context.Translations.SingleOrDefaultAsync(m => m.ProjectKey == projectName && m.LanguageKey == languageKey && m.Key == key);
            _context.Translations.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string languageKey, string projectKey, string key)
        {
            return await _context.Translations.AnyAsync(m => m.LanguageKey == languageKey && m.ProjectKey == projectKey && m.Key == key);
        }

        public async Task<IEnumerable<Translation>> GetAll(string language, string projectKey)
        {
            return await _context.Translations.Where(m => m.ProjectKey == projectKey && m.LanguageKey == language).ToListAsync();
        }

        public async Task<Translation> GetByKey(string languageKey, string projectName, string key)
        {
            return await _context.Translations.SingleOrDefaultAsync(m => m.LanguageKey == languageKey && m.ProjectKey == projectName && m.Key == m.Key);
        }

        public async Task Update(Translation value)
        {
            var id = await GetIdByKey(value.LanguageKey, value.ProjectKey, value.Key);
            var source = await _context.Translations.FindAsync(id);
            source.Text = value.Text;
            _context.Update(source);
            await _context.SaveChangesAsync();
        }

        private async Task PopulateProjectAndLanguageId(Translation value)
        {
            if (value.ProjectId == default(int))
            {
                value.ProjectId = await GetProjectIdByKey(value.ProjectKey);
            }

            if (value.LanguageId == default(int))
            {
                value.LanguageId = await GetLanguageIdByKey(value.LanguageKey);
            }
        }

        private async Task<int> GetIdByKey(string languageKey, string projectKey, string key)
        {
            return await _context.Translations.Where(m => m.LanguageKey == languageKey && m.ProjectKey == projectKey && m.Key == key).Select(m => m.Id).SingleOrDefaultAsync();
        }

        private async Task<int> GetProjectIdByKey(string key) => await _context.Projects.Where(p => p.Key == key).Select(p => p.Id).SingleAsync();

        private async Task<int> GetLanguageIdByKey(string key) => await _context.Languages.Where(p => p.Key == key).Select(p => p.Id).SingleAsync();

        public async Task DeleteAll(string languageKey, string projectKey)
        {
            var entities = await _context.Translations.Where(translation => translation.LanguageKey == languageKey &&
            translation.ProjectKey == projectKey).ToListAsync();
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}