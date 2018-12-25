using DexTranslate.Abstractions.Repository;
using DexTranslate.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DexTranslate.Data
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly TranslationContext _context;

        public LanguageRepository(TranslationContext context)
        {
            _context = context;
        }

        public async Task Add(Language language)
        {
            _context.Languages.Add(language);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string key)
        {
            var language = await GetByKey(key);
            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Language>> GetAll()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task Update(Language value)
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
            return await _context.Languages.Where(m => m.Key == key).Select(m => m.Id).SingleOrDefaultAsync();
        }

        private async Task<Language> GetByKey(string key)
        {
            return await _context.Languages.SingleOrDefaultAsync(m => m.Key == key);
        }
    }
}