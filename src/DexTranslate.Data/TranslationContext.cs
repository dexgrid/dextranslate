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
            modelBuilder.Entity<Project>().HasIndex(project => project.Key).IsUnique();
            modelBuilder.Entity<Language>().HasIndex(language => language.Key).IsUnique();
            modelBuilder.Entity<Translation>().HasIndex(translation => new { translation.LanguageKey, translation.ProjectKey, translation.Key }).IsUnique();
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Translation> Translations { get; set; }
    }
}