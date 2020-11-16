using System;
using System.Collections.Generic;

namespace TranslationFramework.Comum
{
    public class ResultadoOperacao
    {
        public Guid Id { get; }
        public bool Sucesso { get; }
        public List<string> Mensagens { get; }

        public ResultadoOperacao(bool success, string message, Guid id) =>
            (Sucesso, Mensagens, Id) = (success, new List<string>{message}, id);
    }
}
