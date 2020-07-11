using TranslationFramework.DTO;
using TranslationFramework.Modelos;

namespace TranslationFramework.Dados.Handlers
{
    public static partial class Conversoes
    {
        public static LinhaArquivo ConverterParaModel(this LinhaArquivoDTO dto)
        {
            return ConverterPara(dto, new LinhaArquivo());
        }

        public static LinhaArquivoDTO ConverterParaDTO(this LinhaArquivo model)
        {
            return ConverterPara(model, new LinhaArquivoDTO());
        }
    }
}
