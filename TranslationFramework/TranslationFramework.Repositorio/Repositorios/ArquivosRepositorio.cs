using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranslationFramework.Dados.Handlers;
using TranslationFramework.DTO;
using TranslationFramework.Modelos;

namespace TranslationFramework.Dados.Repositorios
{
    public class ArquivosRepositorio : BaseRepositorio
    {

        public ArquivosRepositorio(AplicacaoContexto contexto) : base(contexto)
        {
        }
        
        public async Task CadastrarEditar(ArquivoDto arquivoDTO, bool salvarContexto = true)
        {
            var arquivo = await Contexto.Arquivos
                .FirstOrDefaultAsync(o =>
                    o.Id.Equals(arquivoDTO.Id) || 
                    o.NomeArquivo.Equals(arquivoDTO.NomeArquivo) &&
                    o.Caminho.Equals(arquivoDTO.Caminho));

            var resultado = arquivoDTO.ConverterParaModel();
            resultado.DataUltimaAlteracao = DateTime.Now;
            resultado.Linhas = arquivoDTO.Linhas
                .Select(o => o.ConverterParaModel())
                .ToList();

            if (arquivo is null)
            {
                resultado.Id = Guid.NewGuid();
                await Contexto.Arquivos.AddAsync(resultado);
            }
            else
            {
                Contexto.Entry(resultado).State = EntityState.Modified;
            }

            if (salvarContexto)
            {
                await Contexto.SaveChangesAsync();
            }
        }

        public async Task<ArquivoDto> Obter(Guid id)
        {
            var resultado = await Contexto.Arquivos
                .Where(o => o.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (resultado == null)
            {
                return null;
            }

            var arquivoDTO = resultado.ConverterParaDTO();

            var linhas = await Contexto.LinhasArquivo
                .Where(o => o.ArquivoId.Equals(resultado.Id))
                .OrderBy(o => o.Coluna)
                .ThenBy(o => o.Linha)
                .ToListAsync();

            arquivoDTO.Linhas = linhas
                .Select(f => f.ConverterParaDTO())
                .ToList();

            return arquivoDTO;
        }

        public Microsoft.EntityFrameworkCore.Query
            .IIncludableQueryable<Arquivo, ICollection<LinhaArquivo>> QueryBase()
        {
            return Contexto.Arquivos
                .Include(f => f.Linhas);
        }
    }
}
