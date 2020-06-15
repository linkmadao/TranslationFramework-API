namespace TranslationFramework.Comum.Constantes
{
    /// <summary>
    /// Classe com todas as mensagens de regra de negócio.
    /// </summary>
    public static class MensagensSistema
    {
        public const string OperacaoOk = "Operação Ok.";

        public const string Erro500 = "Ops! Ocorreu um erro durante a execução do processo. " +
            "Por gentileza, informe o código a seguir para o suporte: ";

        public const string Erro500RegraNegocio = "A execução do processo retornou a seguinte mensagem: ";

        public const string ArquivoNaoEncontrado = 
            "Não foi possível obter o arquivo desejado.";

        public const string CaminhoInexistente =
            "Caminho não informado";

        public const string FalhaConverterPropriedade =
            "Falha ao converter propriedade {0}. {1}";

        public const string NomeArquivoInexistente =
            "Nome do Arquivo não informado";
    }
}
