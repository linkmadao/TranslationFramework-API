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
        
        public async Task CadastrarEditar(ArquivoDTO arquivoDTO, bool salvarContexto = true)
        {
            var arquivo = await _contexto.Arquivos
                .FirstOrDefaultAsync(o =>
                    o.NomeArquivo.Equals(arquivoDTO.NomeArquivo) &&
                    o.Caminho.Equals(arquivoDTO.Caminho));

            if (arquivo == null)
            {
                arquivo = arquivoDTO.ConverterParaModel();
                arquivo.DataUltimaAlteracao = DateTime.Now;
                arquivo.ProjetoId = Guid.Parse("b847ec64-c1ce-4d96-b5ac-00b77849318a");
                arquivo.Linhas = arquivoDTO.Linhas
                    .Select(o => o.ConverterParaModel())
                    .ToList();

                _contexto.Arquivos.Add(arquivo);
            }
            else
            {
                arquivoDTO.Id = arquivo.Id;
                arquivo = arquivoDTO.ConverterParaModel();
                arquivo.DataUltimaAlteracao = DateTime.Now;

                _contexto.Entry(arquivo).State = EntityState.Modified;
            }

            if (salvarContexto)
            {
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<ArquivoDTO> Obter(Guid id)
        {
            var resultado = await _contexto.Arquivos
                .Where(o => o.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (resultado != null)
            {
                resultado.Linhas = await _contexto.LinhasArquivo
                    .Where(o => o.ArquivoId.Equals(resultado.Id))
                    .OrderBy(o => o.Coluna)
                        .ThenBy(o => o.Linha)
                    .ToListAsync();
            }

            return resultado.ConverterParaDTO();
        }

        public Microsoft.EntityFrameworkCore.Query
            .IIncludableQueryable<Arquivo, ICollection<LinhaArquivo>> QueryBase()
        {
            return _contexto.Arquivos
                .Include(f => f.Linhas);
        }
    }
}
