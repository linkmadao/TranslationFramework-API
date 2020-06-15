namespace TranslationFramework.Dados.Repositorios
{
    public class BaseRepositorio
    {
        public AplicacaoContexto _contexto { get; private set; }

        public BaseRepositorio(AplicacaoContexto contexto)
        {
            _contexto = contexto;
        }
    }
}
