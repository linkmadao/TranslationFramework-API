using System;
using System.Threading.Tasks;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;
using TranslationFramework.Dados.Repositorios;
using TranslationFramework.DTO;

namespace TranslationFramework.Servicos
{
    public class ProjetosServico
    {
        private readonly ProjetosRepositorio _projetosRepositorio;

        public ProjetosServico(ProjetosRepositorio projetosRepositorio)
        {
            _projetosRepositorio = projetosRepositorio;
        }

        public async Task<ProjetoDto> Obter(Guid id)
        {
            var projetoDto = await _projetosRepositorio.Obter(id);

            if (projetoDto == null)
                throw new RegraDeNegocioException(MensagensSistema.ProjetoInexistente);

            return projetoDto;
        }
    }
}
