using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TranslationFramework.Comum;
using TranslationFramework.Comum.Constantes;

namespace TranslationFramework.Dados.Handlers
{
    public static partial class Conversoes
    {
        private const bool somenteExclusaoLogica = true;

        public static void BindEdicao<F, T>(F from, T to)
        {
            ConverterPara(from, to);
        }

        private static T ConverterPara<F, T>(F from, T to)
        {
            if (from == null)
            {
                return default;
            }

            foreach (var propertyBase in from.GetType().GetProperties()
                .Where(f => !f.CustomAttributes.Any(g => g.AttributeType == typeof(IgnorarBindAttribute))))
            {
                try
                {
                    var propertyTo = to.GetType().GetProperty(propertyBase.Name);

                    if (propertyBase.PropertyType.IsEnumerable())
                    {
                        ConversaoTipoColecao(propertyBase, propertyTo, from, to);
                    }
                    else if (propertyBase.PropertyType.IsClass && 
                        propertyBase.PropertyType != typeof(string) &&
                        propertyBase.PropertyType != typeof(byte[]))
                    {
                        ConversaoTipoEntity(propertyBase, propertyTo, from, to);
                    }
                    else
                    {
                        ConversaoTipoSimples(propertyBase, from, to);
                    }
                }
                catch (Exception e)
                {
                    throw new RegraDeNegocioException(string.Format(MensagensSistema.FalhaConverterPropriedade, propertyBase.Name, e.Message));
                }
            }

            return to;
        }

        private static void ConversaoTipoColecao<F, T>(PropertyInfo propertyBase, PropertyInfo propertyTo, F from, T to)
        {
            var listaFrom = (IList)propertyBase.GetValue(from);
            var listaTo = (IList)propertyTo.GetValue(to);

            if (listaFrom.Count == 0)
            {
                listaTo.Clear();
            }

            var itensType = listaTo.GetType().GetGenericArguments().Single();

            if (itensType.IsClass &&
                itensType != typeof(string))
            {
                foreach (var itemFrom in listaFrom)
                {
                    var itemTo = Convert.ChangeType(Activator.CreateInstance(itensType), itensType);
                    var itemExistente = false;

                    if (listaTo.Count > 0)
                    {
                        for (int i = 0; i < listaTo.Count; i++)
                        {
                            var keyTo = listaTo[i].GetType().GetProperties()
                                .FirstOrDefault(f => f.CustomAttributes.Any(g => g.AttributeType == typeof(KeyAttribute)));

                            if (keyTo == null ||
                                ((keyTo.PropertyType == typeof(int) && (int)keyTo.GetValue(listaTo[i]) == 0) ||
                                (keyTo.PropertyType == typeof(Guid) && (Guid)keyTo.GetValue(listaTo[i]) == Guid.Empty)))
                            {
                                continue;
                            }

                            var deleteRegister = true;

                            foreach (var item in listaFrom)
                            {
                                var keyFrom = item.GetType().GetProperty(keyTo.Name);

                                if (keyFrom == null ||
                                    (keyFrom.PropertyType == typeof(int) && (int)keyFrom.GetValue(item) == 0) ||
                                    (keyFrom.PropertyType == typeof(Guid) && (Guid)keyFrom.GetValue(item) == Guid.Empty))
                                {
                                    continue;
                                }

                                if (keyFrom.GetValue(item).Equals(keyTo.GetValue(listaTo[i])))
                                {
                                    deleteRegister = false;
                                }

                            }

                            if (deleteRegister)
                            {
                                if (somenteExclusaoLogica)
                                {
                                    var item = listaTo[i];
                                    var ativo = item.GetType().GetProperty("Ativo");

                                    if (ativo == null)
                                    {
                                        listaTo.RemoveAt(i);
                                        i -= 1;
                                    }
                                    else
                                    {
                                        ativo.SetValue(item, false);
                                    }
                                }
                                else
                                {
                                    #pragma warning disable CS0162 
                                    listaTo.RemoveAt(i);
                                    i -= 1;
                                    #pragma warning restore CS0162
                                }
                            }
                        }

                        foreach (var itemToRef in listaTo)
                        {
                            var keyTo = itemToRef.GetType().GetProperties()
                                .FirstOrDefault(f => f.CustomAttributes.Any(g => g.AttributeType == typeof(KeyAttribute)));

                            if (keyTo == null ||
                                (keyTo.PropertyType == typeof(int) && (int)keyTo.GetValue(itemToRef) == 0) ||
                                (keyTo.PropertyType == typeof(Guid) && (Guid)keyTo.GetValue(itemToRef) == Guid.Empty))
                            {
                                continue;
                            }

                            var keyFrom = itemFrom.GetType().GetProperty(keyTo.Name).GetValue(itemFrom);

                            if (keyTo.GetValue(itemToRef).Equals(keyFrom))
                            {
                                itemTo = itemToRef;
                                itemExistente = true;
                                break;
                            }
                        }
                    }

                    var itemLista = ConverterPara(itemFrom, itemTo);

                    if (!itemExistente)
                    {
                        listaTo.Add(itemLista);
                    }
                }
            }
            else
            {
                foreach (var item in listaFrom)
                {
                    listaTo.Add(item);
                }
            }
        }

        private static void ConversaoTipoEntity<F, T>(PropertyInfo propertyBase, PropertyInfo propertyTo, F from, T to)
        {
            var classFrom = propertyBase.GetValue(from);
            if (classFrom != null)
            {
                var classTo = propertyBase.GetValue(to) ?? Convert.ChangeType(Activator.CreateInstance(propertyTo.PropertyType), propertyTo.PropertyType);

                var returnClass = ConverterPara(classFrom, classTo);

                to.GetType().GetProperty(propertyBase.Name)?.SetValue(to, returnClass);
            }
        }

        private static void ConversaoTipoSimples<F, T>(PropertyInfo propertyBase, F from, T to)
        {
            to.GetType().GetProperty(propertyBase.Name)?.SetValue(to, propertyBase.GetValue(from));
        }
    }
}