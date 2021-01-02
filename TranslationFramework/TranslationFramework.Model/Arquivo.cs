using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranslationFramework.Comum;

namespace TranslationFramework.Modelos
{
    [Table("Arquivos")]
    public class Arquivo
    {
        public Arquivo()
        {
            Linhas = new List<LinhaArquivo>();
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime DataUltimaAlteracao { get; set; }

        public string NomeArquivo { get; set; }

        public string Caminho { get; set; }

        [IgnorarBind]
        public ICollection<LinhaArquivo> Linhas { get; set; }

        #region Foreing Key
        [Required]
        public Guid ProjetoId { get; set; }
        #endregion Foreing Key
    }
}
