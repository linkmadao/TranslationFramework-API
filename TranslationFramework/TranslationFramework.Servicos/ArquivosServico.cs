using OfficeOpenXml;
using System;
using System.IO;
using System.Threading.Tasks;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;
using TranslationFramework.Dados.Repositorios;
using TranslationFramework.DTO;

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

            if(arquivo != null)
            {
                arquivo.PorcentagemTraduzida = CalcularPorcentagemTraducao(arquivo);
            }

            return arquivo;
        }

        /// <summary>
        /// Lê o arquivo excel informado e grava no banco de dados os dados desse arquivo
        /// </summary>
        /// <param name="arquivo">Informações do arquivo excel</param>
        /// <returns></returns>
        public async Task GravarPlanilha(ArquivoDTO arquivo)
        {
            var byteArray = File.ReadAllBytes(VerificarExistencia(arquivo));
            Stream stream = new MemoryStream(byteArray);

            arquivo.Caminho = arquivo.Caminho.Substring(58);
            if (string.IsNullOrEmpty(arquivo.Caminho))
            {
                arquivo.Caminho = @"\";
            }

            using (var pck = new ExcelPackage(stream))
            {
                var ws = pck.Workbook.Worksheets[0];

                int totalColunas = ws.Dimension.End.Column;
                int totalLinhas = ws.Dimension.End.Row;

                for (int row = 2; row <= totalLinhas; row++)
                {
                    var linha = new LinhaArquivoDTO()
                    {
                        Id = Guid.NewGuid(),
                        Linha = row - 1,
                    };

                    for (int col = 1; col <= totalColunas; col++)
                    {
                        switch (col)
                        {
                            case 1:
                                linha.Offset = ws.Cells[row, col].Value.ToString();
                                break;
                            case 2:
                                linha.Original = ws.Cells[row, col].Value.ToString();
                                break;
                            case 3:
                                linha.Traducao = ws.Cells[row, col].Value.ToString();
                                break;
                        }
                    }

                    arquivo.Linhas.Add(linha);
                }
            }

            await _arquivosRepositorio.CadastrarEditar(arquivo);
        }

        private static decimal CalcularPorcentagemTraducao(ArquivoDTO arquivo)
        {
            decimal totalLinhas = arquivo.Linhas.Count;
            decimal linhasModificadas = 0;

            foreach (var linha in arquivo.Linhas)
            {
                if (!linha.Original.Equals(linha.Traducao))
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

        private static string VerificarExistencia(ArquivoDTO arquivo)
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
