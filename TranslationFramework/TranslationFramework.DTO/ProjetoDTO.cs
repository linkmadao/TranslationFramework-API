using System;
using System.Collections.Generic;

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

        public ICollection<ArquivoDTO> Arquivos { get; set; }
    }
}
