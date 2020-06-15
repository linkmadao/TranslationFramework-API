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

        public ICollection<LinhaArquivo> Linhas { get; set; }

        [IgnorarBind]
        public decimal PorcentagemTraduzida { get; set; }

        #region Foreing Key
        [Required]
        public Guid ProjetoId { get; set; }

        [IgnorarBind]
        public Projeto Projeto { get; set; }
        #endregion Foreing Key
    }
}
