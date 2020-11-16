using System;
using System.Collections.Generic;
using System.IO;
using TranslationFramework.Comum;

namespace TranslationFramework.DTO
{
    public class ArquivoDto
    {
        public ArquivoDto()
        {
            Linhas = new List<LinhaArquivoDto>();
        }

        public ArquivoDto(string caminho, string nomeArquivo, Guid projetoId, Stream stream) =>
            (Caminho, NomeArquivo, ProjetoId, StreamArquivo) = (caminho, nomeArquivo, projetoId, stream);

        public Guid Id { get; set; }

        public DateTime DataUltimaAlteracao { get; set; }

        [IgnorarBind]
        public Stream StreamArquivo { get; set; }

        public string NomeArquivo { get; set; }

        public string Caminho { get; set; }

        [IgnorarBind]
        public ICollection<LinhaArquivoDto> Linhas { get; set; }

        [IgnorarBind]
        public decimal PorcentagemTraduzida { get; set; }

        public Guid ProjetoId { get; set; }
    }
}
