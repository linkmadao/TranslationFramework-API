using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;
using TranslationFramework.DTO;
using TranslationFramework.Servicos;

namespace TranslationFramework.API.Controllers
{
    [ApiController]
    public class ArquivosController : Controller
    {
        private readonly ArquivosServico _arquivosServico;

        public ArquivosController(ArquivosServico arquivosServico)
        {
            _arquivosServico = arquivosServico;
        }

        [HttpGet]
        [Route("v1/excel/obterArquivo")]
        public async Task<IActionResult> LerPlanilha(Guid id)
        {
            try
            {
                var resultado = await _arquivosServico.Obter(id);
                return Ok(resultado);
            }
            catch (RegraDeNegocioException e)
            {
                var guid = Guid.NewGuid();
                return BadRequest(new ResultadoOperacao(false, MensagensSistema.Erro500RegraNegocio + e.Message, guid));
            }
            catch (Exception)
            {
                var guid = Guid.NewGuid();
                return BadRequest(new ResultadoOperacao(false, MensagensSistema.Erro500 + guid.ToString(), guid));
            }
        }

        [HttpPost]
        [Route("v1/excel/gravarPlanilha")]
        public async Task<IActionResult> CadastrarArquivo([FromBody] ArquivoDTO arquivo)
        {
            try
            {
                await _arquivosServico.GravarPlanilha(arquivo);
                return Ok(MensagensSistema.OperacaoOk);
            }
            catch (RegraDeNegocioException e)
            {
                var guid = Guid.NewGuid();
                return BadRequest(new ResultadoOperacao(false, MensagensSistema.Erro500RegraNegocio + e.Message, guid));
            }
            catch (Exception)
            {
                var guid = Guid.NewGuid();
                return BadRequest(new ResultadoOperacao(false, MensagensSistema.Erro500 + guid.ToString(), guid));
            }
        }
    }
}