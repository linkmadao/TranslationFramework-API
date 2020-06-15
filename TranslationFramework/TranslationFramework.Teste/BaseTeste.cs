using Microsoft.Extensions.Configuration;
using TranslationFramework.Dados;
using TranslationFramework.Dados.Repositorios;

namespace TranslationFramework.Teste
{
    public class BaseTeste
    {
        public readonly AplicacaoContexto contexto;
        public readonly IConfiguration configuration;

        public BaseTeste()
        {
        }

        #region Repositórios
        public ArquivosRepositorio ArquivosRepositorio => 
            new ArquivosRepositorio(contexto);
        #endregion Repositórios
        /*
        #region Servicos
        public ArquivosServico ArquivosServico => 
            new ArquivosServico(ArquivosRepositorio);
        #endregion Servicos

        #region Controllers
        public ArquivosController ArquivoController => 
            new ArquivosController(ArquivosServico);
        #endregion Controllers*/
    }
}
