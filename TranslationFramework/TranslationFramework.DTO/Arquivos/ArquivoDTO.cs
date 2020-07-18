using System;
using System.Collections.Generic;
using System.IO;
using TranslationFramework.Comum;

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

        [IgnorarBind]
        public Stream StreamArquivo { get; set; }

        public string NomeArquivo { get; set; }

        public string Caminho { get; set; }

        [IgnorarBind]
        public ICollection<LinhaArquivoDTO> Linhas { get; set; }

        [IgnorarBind]
        public decimal PorcentagemTraduzida { get; set; }

        public Guid ProjetoId { get; set; }
    }
}
