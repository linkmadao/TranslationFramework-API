using System;
using System.Collections.Generic;

namespace TranslationFramework.Comum
{
    public class ResultadoOperacao
    {
        public Guid Id { get; set; }
        public bool Sucesso { get; set; }
        public List<string> Mensagens { get; set; }

        public ResultadoOperacao(bool success)
        {
            Sucesso = success;
            Mensagens = new List<string>();
        }

        public ResultadoOperacao(bool success, string message)
        {
            Sucesso = success;
            Mensagens = new List<string>
            {
                message
            };
        }

        public ResultadoOperacao(bool success, List<string> messages)
        {
            Sucesso = success;
            Mensagens = messages;
        }

        public ResultadoOperacao(bool success, string message, Guid id)
        {
            Id = id;
            Sucesso = success;
            Mensagens = new List<string>
            {
                message
            };
        }
    }
}
