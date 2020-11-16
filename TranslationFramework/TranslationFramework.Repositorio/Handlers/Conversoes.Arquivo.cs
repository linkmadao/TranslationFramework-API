using TranslationFramework.DTO;
using TranslationFramework.Modelos;

namespace TranslationFramework.Dados.Handlers
{
    public static partial class Conversoes
    {
        public static Arquivo ConverterParaModel(this ArquivoDto dto)
        {
            return ConverterPara(dto, new Arquivo());
        }

        public static ArquivoDto ConverterParaDTO(this Arquivo model)
        {
            return ConverterPara(model, new ArquivoDto());
        }
    }
}
