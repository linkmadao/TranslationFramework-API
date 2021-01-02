using TranslationFramework.DTO;
using TranslationFramework.Modelos;

namespace TranslationFramework.Dados.Handlers
{
    public static partial class Conversoes
    {
        public static Projeto ConverterParaModel(this ProjetoDto dto)
        {
            return ConverterPara(dto, new Projeto());
        }

        public static ProjetoDto ConverterParaDto(this Projeto model)
        {
            return ConverterPara(model, new ProjetoDto());
        }
    }
}
