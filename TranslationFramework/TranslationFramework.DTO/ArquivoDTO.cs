using System;
using System.Collections.Generic;

namespace TranslationFramework.DTO
{
    public class ArquivoDTO
    {
        public ArquivoDTO()
        {
            Linhas = new List<LinhaArquivoDTO>();
        }

        public Guid Id { get; set; }

        public DateTime DataUltimaAlteracao { get; set; }

        public string NomeArquivo { get; set; }

        public string Caminho { get; set; }

        public ICollection<LinhaArquivoDTO> Linhas { get; set; }

        public decimal PorcentagemTraduzida { get; set; }

        public Guid ProjetoId { get; set; }
    }
}
