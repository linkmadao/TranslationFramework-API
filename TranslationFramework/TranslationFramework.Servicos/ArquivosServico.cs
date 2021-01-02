using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;
using TranslationFramework.Dados.Repositorios;
using TranslationFramework.DTO;
using TranslationFramework.Enumeradores;

namespace TranslationFramework.Servicos
{
    public class ArquivosServico
    {
        private readonly ArquivosRepositorio _arquivosRepositorio;

        public ArquivosServico(ArquivosRepositorio arquivosRepositorio)
        {
            _arquivosRepositorio = arquivosRepositorio;
        }

        /// <summary>
        /// Obtem o arquivo gravado no banco
        /// </summary>
        /// <param name="id">Id do arquivo cadastrado no banco</param>
        /// <returns>Retorna um JSON com os dados desse arquivo</returns>
        public async Task<ArquivoDto> Obter(Guid id)
        {
            var arquivo = await _arquivosRepositorio.Obter(id);

            if (arquivo == null)
                throw new RegraDeNegocioException(MensagensSistema.ArquivoInexistente);

            foreach (var linha in arquivo.Linhas)
            {
                linha.OriginalDecodificada = linha.Original.ConvertUtf8ToString();
                linha.Original = null;

                linha.TraducaoDecodificada = linha.Traducao.ConvertUtf8ToString();
                linha.Traducao = null;
            }

            arquivo.StreamArquivo = null;
            arquivo.PorcentagemTraduzida = CalcularPorcentagemTraducao(arquivo);

            return arquivo;
        }

        public async Task<bool> Atualizar(ArquivoDto arquivoDTO)
        { 
            var arquivo = await _arquivosRepositorio.Obter(arquivoDTO.Id);

            if(arquivo == null)
            {
                throw new RegraDeNegocioException(MensagensSistema.AtualizarArquivoInexistente);
            }

            var arquivoModificado = false;

            foreach (var linha in arquivoDTO.Linhas)
            {
                var linhaAtual = arquivo.Linhas.FirstOrDefault(o => o.Traducao.ConvertUtf8ToString().Equals(linha.TraducaoDecodificada));

                if (!arquivoModificado && linhaAtual == null)
                {
                    arquivoModificado = true;
                }

                linha.Original = linha.OriginalDecodificada.ConvertStringToUtf8();
                linha.OriginalDecodificada = null;

                linha.Traducao = linha.TraducaoDecodificada.ConvertStringToUtf8();
                linha.TraducaoDecodificada = null;
            }

            if (arquivoModificado)
            {
                await _arquivosRepositorio.CadastrarEditar(arquivoDTO);
            }
            
            return arquivoModificado;
        }

        /// <summary>
        /// Lê o arquivo excel informado e grava no banco de dados os dados desse arquivo
        /// </summary>
        /// <param name="arquivo">Informações do arquivo excel</param>
        /// <returns></returns>
        private async Task Cadastrar(ArquivoDto arquivo)
        {
            using (var pck = new ExcelPackage(arquivo.StreamArquivo))
            {
                var ws = pck.Workbook.Worksheets[0];

                var totalColunas = ws.ObterDimensoes().End.Column;
                var totalLinhas = ws.ObterDimensoes().End.Row;

                for (var row = 2; row <= totalLinhas; row++)
                {
                    var cadastrar = true;

                    var linha = new LinhaArquivoDto(Guid.NewGuid(), arquivo.Id);

                    for (var col = 1; col <= totalColunas; col++)
                    {
                        switch (totalColunas)
                        {
                            case 3:
                                linha.Linha = row - 1;

                                switch (col)
                                {
                                    case 1:
                                        linha.Offset = ws.Cells[row, col].Value.ToString();
                                        break;
                                    case 2:
                                        linha.Original = ws.Cells[row, col].Value.ConvertStringToUtf8();
                                        break;
                                    case 3:
                                        linha.Traducao = ws.Cells[row, col].Value.ConvertStringToUtf8();
                                        break;
                                }
                                break;

                            case 4:
                                if (Enum.TryParse(ws.Cells[row, 1].Value.ToString(), true, out YakuzaKiwamiColunasPermitidas _))
                                {
                                    switch (col)
                                    {
                                        case 1:
                                            linha.Coluna = ws.Cells[row, col].Value.ToString();
                                            break;
                                        case 2:
                                            linha.Linha = int.Parse(ws.Cells[row, col].Value.ToString()!) + 1;
                                            break;
                                        case 3:
                                            linha.Original = ws.Cells[row, col].Value.ConvertStringToUtf8();
                                            break;
                                        case 4:
                                            linha.Traducao = ws.Cells[row, col].Value.ConvertStringToUtf8();
                                            break;
                                    }
                                }
                                else
                                {
                                    cadastrar = false;
                                }
                                break;
                        }
                    }

                    if(cadastrar)
                    {
                        arquivo.Linhas.Add(linha);
                    }
                }
            }

            await _arquivosRepositorio.CadastrarEditar(arquivo);
        }

        public async Task CarregaArquivos(CarregaArquivosDto arquivos)
        {
            if (arquivos.ProjetoId.Equals(Guid.Empty))
            {
                throw new RegraDeNegocioException(MensagensSistema.IdDoProjetoNaoInformado);
            }

            if (string.IsNullOrEmpty(arquivos.Caminho))
            {
                throw new RegraDeNegocioException(MensagensSistema.CaminhoInexistente);
            }

            if (!arquivos.Arquivos.Any())
            {
                throw new RegraDeNegocioException(MensagensSistema.NenhumArquivoFoiSelecionado);
            }

            foreach (var arquivo in arquivos.Arquivos)
            {
                if (arquivo.Length <= 0) continue;

                var stream = new MemoryStream();
                await arquivo.CopyToAsync(stream);

                await Cadastrar(new ArquivoDto(arquivos.Caminho, arquivo.FileName, arquivos.ProjetoId, stream));
            }
        }

        private decimal CalcularPorcentagemTraducao(ArquivoDto arquivo)
        {
            decimal totalLinhas = arquivo.Linhas.Count;
            decimal linhasModificadas = 0;

            foreach (var linha in arquivo.Linhas)
            {
                if ((!string.IsNullOrEmpty(linha.OriginalDecodificada) &&
                    !string.IsNullOrEmpty(linha.TraducaoDecodificada)) &&
                    !linha.OriginalDecodificada.Equals(linha.TraducaoDecodificada))
                {
                    linhasModificadas += 1;
                }
            }

            return linhasModificadas == 0 
                ? 0 
                : Math.Round(linhasModificadas * 100 / totalLinhas, 2);
        }
    }
}
