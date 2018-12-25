using DexTranslate.Api.Filters;
using DexTranslate.Api.MiddleWare;
using DexTranslate.Api.Options;
using DexTranslate.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;

namespace DexTranslate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TranslationContext>(options => options.UseSqlite(connectionString,
                x => x.MigrationsAssembly("DexTranslate.Data.SQLiteMigrations")));

            services.AddTranslationServices();

            services.Configure<CustomAuthenticationOptions>(options => Configuration.GetSection("Authentication").Bind(options));

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = "api";
            }).AddScheme<CustomAuthenticationOptions, TokenHandler>("api", "DEX Translate API authentication scheme", o => { });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "DEX TRANSLATE", Version = "v1" });
                c.AddSecurityDefinition("ApiKey", new ApiKeyScheme { In = "header", Description = "Enter your api key", Name = "ApiKey", Type = "apiKey" });
                c.AddSecurityDefinition("ApiSecret", new ApiKeyScheme { In = "header", Description = "Enter your api secret", Name = "ApiSecret", Type = "apiKey" });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "ApiKey", Enumerable.Empty<string>() },
                    { "ApiSecret", Enumerable.Empty<string>() }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DexTranslate API V1");
            });
            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}