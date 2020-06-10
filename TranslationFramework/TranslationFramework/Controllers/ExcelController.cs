using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;
using TranslationFramework.Servicos;

namespace TranslationFramework.API.Controllers
{
    [ApiController]
    public class ExcelController : Controller
    {
        [HttpPost]
        [Route("v1/excel/lerPlanilha")]
        public object LerPlanilha()
        {
            try
            {
                return ProcessarExcel.LerPlanilha();
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