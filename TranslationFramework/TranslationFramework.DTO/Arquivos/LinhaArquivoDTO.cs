using System;
using TranslationFramework.Comum;

namespace TranslationFramework.DTO
{
    public class LinhaArquivoDTO
    {
        public Guid Id { get; set; }

        public int Linha { get; set; }

        public string Coluna { get; set; }

        public string Offset { get; set; }

        public byte[] Original { get; set; }

        [IgnorarBind]
        public string OriginalDecodificada { get; set; }

        public byte[] Traducao { get; set; }

        [IgnorarBind]
        public string TraducaoDecodificada { get; set; }

        public Guid ArquivoId { get; set; }
    }
}
