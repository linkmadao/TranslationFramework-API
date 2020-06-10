using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;

namespace TranslationFramework.Servicos
{
    public class ProcessarExcel
    {
        public static object LerPlanilha()
        {
            string caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "YakuzaKiwami.xlsx");
            VerificarExistencia(caminhoArquivo);

            var byteArray = File.ReadAllBytes(caminhoArquivo);
            Stream stream = new MemoryStream(byteArray);

            var conteudo = new JArray();

            using (var pck = new ExcelPackage(stream))
            {
                var ws = pck.Workbook.Worksheets[0];
               
                int totalColunas = ws.Dimension.End.Column;
                int totalLinhas = ws.Dimension.End.Row;

                for (int row = 2; row <= totalLinhas; row++)
                {
                    var linha = new JObject()
                    {
                        new JProperty("linha", row - 1)
                    };
                    for (int col = 1; col <= totalColunas; col++)
                    {
                        string titulo = "";
                        switch(col)
                        {
                            case 1:
                                titulo = "offset";
                                break;
                            case 2:
                                titulo = "original";
                                break;
                            case 3:
                                titulo = "traducao";
                                break;
                        }

                        linha.Add(new JProperty(titulo, ws.Cells[row, col].Value.ToString()));
                    }

                    conteudo.Add(linha);
                }
            }

            var resultado = new JObject() 
            {
                new JProperty("id", Guid.NewGuid()),
                new JProperty("arquivo", "YakuzaKiwami.xlsx"),
                new JProperty("caminho", Directory.GetCurrentDirectory()),
                new JProperty("conteudo", conteudo)
            };

            return resultado;
        }

        private static void VerificarExistencia(string nomeArquivo)
        {
            if (!File.Exists(nomeArquivo))
            {
                throw new RegraDeNegocioException(MensagensSistema.ArquivoNaoEncontrado);
            }
        }
    }
}
