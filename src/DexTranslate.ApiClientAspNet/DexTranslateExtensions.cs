using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;

namespace DexTranslate.ApiClientAspNet
{
    public static class DexTranslateExtensions
    {
        public static IServiceCollection AddDexLocalization(
          this IServiceCollection services,
          IConfiguration configuration,
          Action<DexTranslateOptions> setupAction = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure<DexTranslateOptions>(options =>
                configuration.GetSection("DexTranslate").Bind(options));

            services.TryAdd(new ServiceDescriptor(
                typeof(IStringLocalizerFactory),
                typeof(DexStringLocalizerFactory),
                ServiceLifetime.Singleton));

            services.TryAdd(new ServiceDescriptor(
                typeof(IStringLocalizerFactory),
                typeof(DexStringLocalizerFactory),
                ServiceLifetime.Singleton));

            services.TryAdd(new ServiceDescriptor(
                typeof(IStringLocalizer),
                typeof(DexStringLocalizer),
                ServiceLifetime.Transient));

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
            return services;
        }
    }
}