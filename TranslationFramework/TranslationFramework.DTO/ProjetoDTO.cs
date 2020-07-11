using System;
using System.Collections.Generic;
using TranslationFramework.Comum;

namespace TranslationFramework.DTO
{
    public class Projeto
    {
        public Projeto()
        {
            Arquivos = new List<ArquivoDTO>();
        }

        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string SteamId { get; set; }

        [IgnorarBind]
        public ICollection<ArquivoDTO> Arquivos { get; set; }
    }
}
