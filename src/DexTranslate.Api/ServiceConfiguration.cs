using DexTranslate.Abstractions.Caching;
using DexTranslate.Abstractions.Repository;
using DexTranslate.Abstractions.Service;
using DexTranslate.Caching;
using DexTranslate.Core;
using DexTranslate.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DexTranslate.Api
{
    public static class ServiceConfiguration
    {
        public static void AddTranslationServices(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddCaches();
            services.AddServices();
        }

        private static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<ITranslationRepository, TranslationRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
        }

        private static void AddCaches(this IServiceCollection services)
        {
            services.AddScoped<ILanguageCache, LanguageCache>();
            services.AddScoped<ITranslationCache, TranslationCache>();
            services.AddScoped<IProjectCache, ProjectCache>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<IImportService, ImportService>();
        }
    }
}