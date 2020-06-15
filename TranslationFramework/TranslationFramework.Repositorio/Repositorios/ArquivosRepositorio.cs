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
                arquivo.Id = Guid.NewGuid();
                arquivo.DataUltimaAlteracao = DateTime.Now;

                _contexto.Arquivos.Add(arquivo);
            }
            else
            {
                arquivoDTO.Id = arquivo.Id;
                arquivo = arquivoDTO.ConverterParaModel();

                _contexto.Entry(arquivo).State = EntityState.Modified;
            }

            if (salvarContexto)
            {
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<ArquivoDTO> Obter(Guid id)
        {
            return await QueryBase()
                .Where(o => o.Id.Equals(id))
                .Select(f => f.ConverterParaDTO())
                .FirstOrDefaultAsync();
        }

        public Microsoft.EntityFrameworkCore.Query
            .IIncludableQueryable<Arquivo, ICollection<LinhaArquivo>> QueryBase()
        {
            return _contexto.Arquivos
                .Include(f => f.Linhas);
        }
    }
}
