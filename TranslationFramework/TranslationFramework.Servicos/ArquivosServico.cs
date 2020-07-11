using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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
        public async Task<ArquivoDTO> Obter(Guid id)
        {
            var arquivo = await _arquivosRepositorio.Obter(id);

            if (arquivo != null)
            {
                foreach (var linha in arquivo.Linhas)
                {
                    linha.Original = ProcessaTexto(linha.Original, false);
                    linha.Traducao = ProcessaTexto(linha.Traducao, false);
                }

                arquivo.PorcentagemTraduzida = CalcularPorcentagemTraducao(arquivo);
            }

            return arquivo;
        }

        /// <summary>
        /// Lê o arquivo excel informado e grava no banco de dados os dados desse arquivo
        /// </summary>
        /// <param name="arquivo">Informações do arquivo excel</param>
        /// <returns></returns>
        public async Task<Guid> GravarPlanilha(ArquivoDTO arquivo)
        {
            var byteArray = File.ReadAllBytes(VerificarExistencia(arquivo));
            Stream stream = new MemoryStream(byteArray);

            arquivo.Caminho = arquivo.Caminho.Substring(27);
            if (string.IsNullOrEmpty(arquivo.Caminho))
            {
                arquivo.Caminho = @"\";
            }

            arquivo.Id = Guid.NewGuid();

            using (var pck = new ExcelPackage(stream))
            {
                var ws = pck.Workbook.Worksheets[0];

                int totalColunas = ws.Dimension.End.Column;
                int totalLinhas = ws.Dimension.End.Row;

                for (int row = 2; row <= totalLinhas; row++)
                {
                    bool cadastrar = true;

                    var linha = new LinhaArquivoDTO()
                    {
                        Id = Guid.NewGuid(),
                        ArquivoId = arquivo.Id,
                    };

                    for (int col = 1; col <= totalColunas; col++)
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
                                        linha.Original = ProcessaTexto(ws.Cells[row, col].Value.ToString());
                                        break;
                                    case 3:
                                        linha.Traducao = ProcessaTexto(ws.Cells[row, col].Value.ToString());
                                        break;
                                }
                                break;

                            case 4:
                                YakuzaKiwamiColunasPermitidas colunasPermitidas;
                                if (Enum.TryParse(ws.Cells[row, 1].Value.ToString(), true, out colunasPermitidas))
                                {
                                    switch (col)
                                    {
                                        case 1:
                                            linha.Coluna = ws.Cells[row, col].Value.ToString();
                                            break;
                                        case 2:
                                            linha.Linha = int.Parse(ws.Cells[row, col].Value.ToString()) + 1;
                                            break;
                                        case 3:
                                            linha.Original = ProcessaTexto(ws.Cells[row, col].Value.ToString());
                                            break;
                                        case 4:
                                            linha.Traducao = ProcessaTexto(ws.Cells[row, col].Value.ToString());
                                            break;
                                    }
                                    break;
                                }
                                else
                                {
                                    cadastrar = false;
                                    break;
                                }
                        }
                    }

                    if(cadastrar)
                    {
                        arquivo.Linhas.Add(linha);
                    }
                }
            }

            await _arquivosRepositorio.CadastrarEditar(arquivo);

            return arquivo.Id;
        }

        private decimal CalcularPorcentagemTraducao(ArquivoDTO arquivo)
        {
            decimal totalLinhas = arquivo.Linhas.Count;
            decimal linhasModificadas = 0;

            foreach (var linha in arquivo.Linhas)
            {
                if ((!string.IsNullOrEmpty(linha.Original) &&
                    !string.IsNullOrEmpty(linha.Traducao)) &&
                    !linha.Original.Equals(linha.Traducao))
                {
                    linhasModificadas += 1;
                }
            }

            if (linhasModificadas == 0)
            {
                return 0;
            }

            return Math.Round(linhasModificadas * 100 / totalLinhas, 2);
        }

        private string ProcessaTexto(string texto, bool entrada = true)
        {
            if (!string.IsNullOrEmpty(texto))
            { 
                var dicionario = new Dictionary<string, string>();

                #region Numeros e Caracteres
                dicionario.Add("%", "&#37;");
                dicionario.Add("<", "&#60;");
                dicionario.Add(">", "&#62;");
                dicionario.Add("\n", "&#92;n");
                dicionario.Add("\r", "&#92;r");
                dicionario.Add(@"\", "&#92;");
                dicionario.Add("○", "&#9675;");
                dicionario.Add("ー", "---");

                dicionario.Add("２", "2j");
                #endregion Numeros

                #region A
                dicionario.Add("ア", "A ");
                #endregion A

                #region B
                dicionario.Add("バ", "Ba ");
                dicionario.Add("防", "Boo ");
                #endregion B

                #region D
                dicionario.Add("ダ", "Da ");
                #endregion D

                #region G
                dicionario.Add("具", "Gu ");
                #endregion G

                #region H
                dicionario.Add("菱", "Hishi ");
                dicionario.Add("細", "Hoso ");
                #endregion H

                #region I
                dicionario.Add("要", "Io ");
                #endregion I

                #region J
                dicionario.Add("ジ", "Ji ");
                dicionario.Add("重", "Ju ");
                #endregion J

                #region K
                dicionario.Add("駆", "Kakeru ");
                dicionario.Add("刀", "Katana ");
                dicionario.Add("川", "Kawa ");
                dicionario.Add("飾", "Kazari ");
                dicionario.Add("殊", "Koto ");
                dicionario.Add("ク", "Ku ");
                #endregion K

                #region M
                dicionario.Add("丸", "Maru ");
                dicionario.Add("メ", "Me ");
                #endregion M

                #region N
                dicionario.Add("長", "Naga ");
                dicionario.Add("ノ", "No ");
                #endregion N

                #region O
                dicionario.Add("錘", "Omori ");
                #endregion O

                #region R
                dicionario.Add("リ", "Ri ");
                dicionario.Add("ル", "Ru ");
                #endregion R

                #region S
                dicionario.Add("四", "Shi ");
                dicionario.Add("装", "So ");
                #endregion S

                #region T
                dicionario.Add("武", "Take ");
                dicionario.Add("特", "Toku ");
                dicionario.Add("ト", "To ");
                dicionario.Add("通", "Tsu ");
                dicionario.Add("紡", "Tsumugi ");
                dicionario.Add("常", "Tsune ");
                #endregion T

                #region U
                dicionario.Add("海", "Umi ");
                dicionario.Add("器", "Utsuwa ");
                #endregion U


                foreach (var item in dicionario)
                {
                    if (entrada)
                    {
                        texto = texto.Replace(item.Key, item.Value);
                        continue;
                    }                    

                    texto = texto.Replace(item.Value, item.Key);
                }
            }

            return texto;
        }

        private string VerificarExistencia(ArquivoDTO arquivo)
        {
            if (string.IsNullOrEmpty(arquivo.Caminho))
            {
                throw new RegraDeNegocioException(MensagensSistema.CaminhoInexistente);
            }
            if (string.IsNullOrEmpty(arquivo.NomeArquivo))
            {
                throw new RegraDeNegocioException(MensagensSistema.NomeArquivoInexistente);
            }

            string caminhoArquivo = Path.Combine(arquivo.Caminho, arquivo.NomeArquivo);
            if (!File.Exists(caminhoArquivo))
            {
                throw new RegraDeNegocioException(MensagensSistema.ArquivoNaoEncontrado);
            }

            return caminhoArquivo;
        }
    }
}
