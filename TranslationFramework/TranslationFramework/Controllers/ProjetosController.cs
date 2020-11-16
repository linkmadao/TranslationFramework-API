using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;
using TranslationFramework.Servicos;

namespace TranslationFramework.API.Controllers
{
    public class ProjetosController : BaseController
    {
        private readonly ProjetosServico _projetosServico;

        public ProjetosController(ProjetosServico projetosServico)
        {
            _projetosServico = projetosServico;
        }

        /// <summary>
        /// Obtem um projeto a partir do Id
        /// </summary>
        /// <param name="id">Id do projeto</param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/projeto/obter/{id}")]
        public async Task<IActionResult> Obter(Guid id)
        {
            try
            {
                var resultado = await _projetosServico.Obter(id);
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

    }
}
