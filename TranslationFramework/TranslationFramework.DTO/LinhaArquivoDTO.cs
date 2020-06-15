using System;

namespace TranslationFramework.DTO
{
    public class LinhaArquivoDTO
    {
        public Guid Id { get; set; }

        public int Linha { get; set; }

        public string Offset { get; set; }

        public string Original { get; set; }

        public string Traducao { get; set; }

        public Guid ArquivoId { get; set; }
    }
}
