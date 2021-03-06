﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;
using TranslationFramework.DTO;
using TranslationFramework.Servicos;

namespace TranslationFramework.API.Controllers
{
    public class ArquivosController : BaseController
    {
        private readonly ArquivosServico _arquivosServico;

        public ArquivosController(ArquivosServico arquivosServico)
        {
            _arquivosServico = arquivosServico;
        }

        /// <summary>
        /// Obtem um arquivo a partir do Id
        /// </summary>
        /// <param name="id">Id do arquivo</param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/arquivo/obter/{id}")]
        public async Task<IActionResult> Obter(Guid id)
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

        /// <summary>
        /// Atualiza um arquivo já cadastrado no sistema
        /// </summary>
        /// <param name="arquivoDto"></param>
        /// <returns>200(OK) ou 304(Sem Alteração)</returns>
        [HttpPut]
        [Route("v1/arquivo/atualizar")]
        public async Task<IActionResult> Atualizar([FromBody] ArquivoDto arquivoDto)
        {
            try
            {
                var resultado = await _arquivosServico.Atualizar(arquivoDto);
                if (resultado)
                {
                    return Ok(MensagensSistema.ArquivoAtualizadoComSucesso);
                }

                // Não há alterações no arquivo
                return StatusCode(304);

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

        /// <summary>
        /// Cadastra arquivos em excel no sistema
        /// </summary>
        /// <param name="arquivos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/arquivo/gravarPlanilhas")]
        public async Task<IActionResult> CadastrarArquivos([FromForm] CarregaArquivosDto arquivos)
        {
            try
            {
                await _arquivosServico.CarregaArquivos(arquivos);
                return StatusCode(201, MensagensSistema.ArquivosCadastradosComSucesso);
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