using System;
using System.Collections.Generic;
using TranslationFramework.Comum;
using TranslationFramework.Enumeradores;

namespace TranslationFramework.DTO
{
    public class ProjetoDto
    {
        public ProjetoDto()
        {
            Arquivos = new List<ArquivoDto>();
        }

        public Guid Id { get; set; }

        public string Nome { get; set; }

        public TipoProjeto TipoProjeto { get; set; }

        public string SteamId { get; set; }

        [IgnorarBind]
        public ICollection<ArquivoDto> Arquivos { get; set; }
    }
}
