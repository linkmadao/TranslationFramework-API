using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;
using TranslationFramework.Dados.Repositorios;
using TranslationFramework.Modelos;
using TranslationFramework.Servicos.Autorizacao;

namespace TranslationFramework.API.Controllers
{
    public class UsuariosController : BaseController
    {
        /// <summary>
        /// Obtem um projeto a partir do Id
        /// </summary>
        /// <param name="id">Id do projeto</param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/usuario/login")]
        public async Task<IActionResult> Obter([FromBody] User model)
        {
            try
            {
                // Recupera o usuário
                var user = UserRepositorio.Get(model.Username, model.Password);

                // Verifica se o usuário existe
                if (user == null)
                    throw new RegraDeNegocioException("Usuário ou senha inválidos");

                // Gera o Token
                var token = TokenServico.GerarToken(user);

                // Retorna os dados
                return Ok(token);
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
