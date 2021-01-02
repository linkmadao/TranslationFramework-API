using Microsoft.EntityFrameworkCore;
using System.Linq;
using Toolbelt.ComponentModel.DataAnnotations;
using TranslationFramework.Modelos;

namespace TranslationFramework.Dados
{
    public class AplicacaoContexto : DbContext
    {
        public AplicacaoContexto(DbContextOptions<AplicacaoContexto> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.BuildIndexesFromAnnotations();
        }

        #region Arquivos
        public DbSet<Arquivo> Arquivos { get; set; }
        public DbSet<LinhaArquivo> LinhasArquivo { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        #endregion Arquivos
    }
}
