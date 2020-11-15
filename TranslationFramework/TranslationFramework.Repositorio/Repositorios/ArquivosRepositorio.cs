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
                    o.Id.Equals(arquivoDTO.Id) || 
                    (o.NomeArquivo.Equals(arquivoDTO.NomeArquivo) &&
                    o.Caminho.Equals(arquivoDTO.Caminho)));

            bool cadastro = false;
            
            if (arquivo is null)
            {
                cadastro = true;
            }
            else
            {
                _contexto.Entry(arquivo).State = EntityState.Detached;
            }

            arquivo = arquivoDTO.ConverterParaModel();
            arquivo.DataUltimaAlteracao = DateTime.Now;
            arquivo.Linhas = arquivoDTO.Linhas
                .Select(o => o.ConverterParaModel())
                .ToList();

            if (cadastro)
            {
                arquivo.Id = Guid.NewGuid();
                _contexto.Arquivos.Add(arquivo);
            }
            else
            {
                _contexto.Entry(arquivo).State = EntityState.Modified;
            }



            if (salvarContexto)
            {
                await _contexto.SaveChangesAsync();
                _contexto.Entry(arquivo).State = EntityState.Detached;
            }
        }

        public async Task<ArquivoDTO> Obter(Guid id)
        {
            var resultado = await _contexto.Arquivos
                .Where(o => o.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (resultado == null)
            {
                return null;
            }

            _contexto.Entry(resultado).State = EntityState.Detached;

            var arquivoDTO = resultado.ConverterParaDTO();

            var linhas = await _contexto.LinhasArquivo
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
            return _contexto.Arquivos
                .Include(f => f.Linhas);
        }
    }
}
