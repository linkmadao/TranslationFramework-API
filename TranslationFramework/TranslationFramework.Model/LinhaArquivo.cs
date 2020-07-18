using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranslationFramework.Modelos
{
    [Table("LinhasArquivo")]
    public class LinhaArquivo
    {
        [Key]
        public Guid Id { get; set; }

        public int Linha { get; set; }

        public string Coluna { get; set; }

        public string Offset { get; set; }

        public byte[] Original { get; set; }

        public byte[] Traducao { get; set; }

        #region Foreing Key
        [Required]
        public Guid ArquivoId { get; set; }
        #endregion Foreing Key
    }
}
