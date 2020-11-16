namespace TranslationFramework.Dados.Repositorios
{
    public class BaseRepositorio
    {
        public AplicacaoContexto Contexto { get; private set; }

        public BaseRepositorio(AplicacaoContexto contexto)
        {
            Contexto = contexto;
        }
    }
}
