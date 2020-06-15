using TranslationFramework.DTO;
using TranslationFramework.Modelos;

namespace TranslationFramework.Dados.Handlers
{
    public static partial class Conversoes
    {
        public static Arquivo ConverterParaModel(this ArquivoDTO dto)
        {
            return ConverterPara(dto, new Arquivo());
        }

        public static ArquivoDTO ConverterParaDTO(this Arquivo model)
        {
            return ConverterPara(model, new ArquivoDTO());
        }
    }
}
