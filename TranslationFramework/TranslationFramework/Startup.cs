using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TranslationFramework.Dados;
using TranslationFramework.Dados.Repositorios;
using TranslationFramework.Servicos;

namespace TranslationFramework.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InstanciarInjecaoDependeciaRepositorios(services);
            InstanciarInjecaoDependenciaServicos(services);

            services.AddMvc();

            ConfigurarSwagger(services);
            ConfigurarAutorizacao(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslationFramework.API V1");
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            AtualizarBancoAutomaticamente(app);
        }

        private static void AtualizarBancoAutomaticamente(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var context = serviceScope.ServiceProvider.GetService<AplicacaoContexto>();
            context?.Database?.Migrate();
        }

        private static void ConfigurarAutorizacao(IServiceCollection services)
        {
            var chave = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e");
            services.AddAuthentication(t =>
            {
                t.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                t.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(t =>
            {
                t.RequireHttpsMetadata = false;
                t.SaveToken = true;
                t.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(chave),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        private void ConfigurarSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TranslationFramework API",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Tiago Silva Miguel",
                        Url = new Uri(Configuration["URLs:Linkedin"])
                    },
                    License = new OpenApiLicense
                    {
                        Name = "GNU General Public License v2.0",
                        Url = new Uri(Configuration["URLs:Github"]),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private void InstanciarInjecaoDependeciaRepositorios(IServiceCollection services)
        {
            services.AddDbContext<AplicacaoContexto>(options => options.UseMySql(Configuration["StringConexao:Padrao"], ServerVersion.AutoDetect(Configuration["StringConexao:Padrao"])));

            services.AddTransient<ArquivosRepositorio, ArquivosRepositorio>();
            services.AddTransient<ProjetosRepositorio, ProjetosRepositorio>();
        }

        private static void InstanciarInjecaoDependenciaServicos(IServiceCollection services)
        {
            services.AddTransient<ArquivosServico, ArquivosServico>();
            services.AddTransient<ProjetosServico, ProjetosServico>();
        }
    }
}
