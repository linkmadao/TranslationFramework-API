namespace TranslationFramework.Comum.Constantes
{
    /// <summary>
    /// Classe com todas as mensagens de regra de negócio.
    /// </summary>
    public static class MensagensSistema
    {
        #region Mensagens Comuns

        public const string Erro500 =
            "Ops! Ocorreu um erro durante a execução do processo. " +
            "Por gentileza, informe o código a seguir para o suporte: ";

        public const string Erro500RegraNegocio =
            "A execução do processo retornou a seguinte mensagem: ";

        public const string OperacaoOk =
            "Operação Ok.";

        #endregion Mensagens Comuns

        public const string ArquivoInexistente = 
            "O arquivo que deseja obter não existe.";

        public const string ArquivoAtualizadoComSucesso =
            "Arquivo atualizado com sucesso!";

        public const string ArquivosCadastradosComSucesso =
            "O cadastro do(s) arquivo(s) foi executado com sucesso";

        public const string AtualizarArquivoInexistente =
            "O arquivo que deseja atualizar não existe.";

        public const string CaminhoInexistente =
            "Caminho não informado";

        public const string FalhaAoConverterPropriedade =
            "Falha ao converter propriedade {0}. {1}";

        public const string IdDoProjetoNaoInformado =
            "O id do projeto não foi informado";

        public const string NenhumArquivoFoiSelecionado =
            "Nenhum arquivo foi selecionado.";

        public const string ProjetoInexistente =
            "O projeto que deseja obter não existe.";
    }
}
