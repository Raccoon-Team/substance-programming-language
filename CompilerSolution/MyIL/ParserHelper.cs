using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Sigil;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    internal static class ParserHelper
    {
        public static bool IsConst(string arg)
        {
            return arg.StartsWith("\"") || double.TryParse(arg, out _);
        }

        /// <summary>
        ///     Находит индекс закрывающей скобки, если таковая не найдена, выдает ошибку
        /// </summary>
        /// <param name="start"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static int FindClosingBraceIndex(int start, IList<Token> tokens)
        {
            var diff = 0;
            var tokensCount = tokens.Count;
            do
            {
                if (tokens[start].TokenType == TokenType.OpenBrace)
                    diff++;
                else if (tokens[start].TokenType == TokenType.CloseBrace)
                    diff--;
                start++;

                if (start == tokensCount)
                    throw new NotImplementedException($"Не найдена закрывающая скобка. Индекс {start}");
            } while (diff != 0);

            return start - 1;
        }

        public static object GetMember(string value, Dictionary<string, Type> definedTypes, LocalLookup locals,
            Type currentType, BindingFlags modifiers = BindingFlags.Public | BindingFlags.NonPublic,
            MemberTypes memberTypes = MemberTypes.Field | MemberTypes.Method)
        {
            if (value.StartsWith("\""))
                return (typeof(string), value.Trim('"'));
            if (int.TryParse(value, out var i))
                return (typeof(int), i.ToString());
            if (float.TryParse(value, out var f))
                return (typeof(float), f.ToString());
            if (double.TryParse(value, out var d))
                return (typeof(double), d.ToString());

            if (value.Contains('.'))
            {
                var parts = value.Split('.');
                var field = currentType.GetField(parts[0]);
                if (field != null)
                    return GetMember(value.Substring(parts[0].Length + 1), definedTypes, locals, field.FieldType,
                        BindingFlags.Public | BindingFlags.Instance, memberTypes);
                if (definedTypes.ContainsKey(value))
                    return GetMember(value.Substring(parts[0].Length + 1), definedTypes, locals, definedTypes[value],
                        BindingFlags.Public | BindingFlags.Static, memberTypes);
            }
            else
            {
                if (locals.Names.Contains(value))
                    return locals[value];
                ((TypeBuilder) currentType).CreateType();
                return currentType.GetMember(value, MemberTypes.Field | MemberTypes.Method,
                    modifiers | BindingFlags.Instance | BindingFlags.Static);
            }
            return null;
        }

        public static bool CheckUnusedLocal(IList<Token> tokens, int firstInstructionIndex, string localName)
        {
            var methodEndIndex = firstInstructionIndex;
            var diff = 1;
            while (diff != 0)
            {
                if (tokens[methodEndIndex].TokenType == TokenType.Construction)
                    diff++;
                else if (tokens[methodEndIndex].TokenType == TokenType.End)
                    diff--;
                methodEndIndex++;
            }

            var count = 0;

            for (var i = firstInstructionIndex; i < methodEndIndex; i++)
            {
                if (tokens[i].Value == localName && (i + 1 == methodEndIndex || tokens[i + 1].Value != "="))
                    count++;

                if (count > 1)
                    return true;
            }

            return false;
        }

        public static void PushToStack(string value, Emit method, Dictionary<string, Type> definedTypes,
            Type currentType)
        {
            var member = GetMember(value, definedTypes, method.Locals, currentType);

            switch (member)
            {
                case MethodInfo methodInfo:
                    if ((methodInfo.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
                        method.Call(methodInfo);
                    else method.CallVirtual(methodInfo);
                    break;
                case FieldInfo fieldInfo:
                    method.LoadField(fieldInfo);
                    break;
                case Local local:
                    method.LoadLocal(local);
                    break;
                default:
                    var (ltype, lvalue) = ((Type, string)) member;
                    method.LoadConstant(ltype, lvalue);
                    break;
            }
        }
    }
}