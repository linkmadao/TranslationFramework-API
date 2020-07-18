using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslationFramework.DTO
{
    public class CarregaArquivosDTO
    {
        [Required]
        public Guid ProjetoId { get; set; }

        [Required]
        public string Caminho { get; set; }

        public IEnumerable<IFormFile> Arquivos { get; set; }
    }
}
