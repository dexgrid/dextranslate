using DexTranslate.Model;
using Microsoft.EntityFrameworkCore;

namespace DexTranslate.Data
{
    public class TranslationContext : DbContext
    {
        public TranslationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Translation> Translations { get; set; }
    }
}