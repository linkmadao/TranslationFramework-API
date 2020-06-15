using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InstanciarInjecaoDependeciaRepositorios(services);
            InstanciarInjecaoDependenciaServicos(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        private void InstanciarInjecaoDependeciaRepositorios(IServiceCollection services)
        {
            services.AddDbContext<AplicacaoContexto>(options => options.UseMySql(Configuration["StringConexao:Padrao"]));

            services.AddTransient<ArquivosRepositorio, ArquivosRepositorio>();
        }

        private void InstanciarInjecaoDependenciaServicos(IServiceCollection services)
        {
            services.AddTransient<ArquivosServico, ArquivosServico>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
