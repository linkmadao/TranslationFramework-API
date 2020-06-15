using System;
using System.Collections;

namespace TranslationFramework.Comum
{
    public static class Funcoes
    {
        public static bool IsEnumerable(this Type type)
        {
            if (type == null || type == typeof(string) || type == typeof(byte[]))
                return false;

            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}
