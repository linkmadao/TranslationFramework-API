using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranslationFramework.Dados.Handlers;
using TranslationFramework.DTO;
using TranslationFramework.Modelos;

namespace TranslationFramework.Dados.Repositorios
{
    public class ProjetosRepositorio : BaseRepositorio
    {
        public ProjetosRepositorio(AplicacaoContexto contexto) : base(contexto)
        {
        }

        public async Task<ProjetoDto> Obter(Guid id)
        {
            var resultado = await Contexto.Projetos
                .Where(o => o.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (resultado == null)
            {
                return null;
            }

            var projetoDto = resultado.ConverterParaDto();

            var arquivos = await Contexto.Arquivos
                .Where(o => o.ProjetoId.Equals(resultado.Id))
                .OrderBy(o => o.Caminho)
                .ToListAsync();

            projetoDto.Arquivos = arquivos
                .Select(f => f.ConverterParaDTO())
                .ToList();

            return projetoDto;
        }

        public Microsoft.EntityFrameworkCore.Query
            .IIncludableQueryable<Projeto, ICollection<Arquivo>> QueryBase()
        {
            return Contexto.Projetos
                .Include(f => f.Arquivos);
        }
    }
}
