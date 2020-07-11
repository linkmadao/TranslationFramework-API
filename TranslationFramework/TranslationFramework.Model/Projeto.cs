using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranslationFramework.Modelos
{
    [Table("Projetos")]
    public class Projeto
    {
        public Projeto()
        {
            Arquivos = new List<Arquivo>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Nome { get; set; }
        
        public string SteamId { get; set; }

        public ICollection<Arquivo> Arquivos { get; set; }
    }
}
