using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CompilerUtilities.Exceptions;
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
            var index = start;
            var tokensCount = tokens.Count;
            do
            {
                if (tokens[index].TokenType == TokenType.OpenBrace)
                    diff++;
                else if (tokens[index].TokenType == TokenType.CloseBrace)
                    diff--;
                index++;

                if (index == tokensCount)
                    ExceptionManager.ThrowCompiler(ErrorCode.ClosingBraceNotFound, string.Empty, tokens[start].Line);
            } while (diff != 0);

            return index - 1;
        }

        public static Type GetMemberType(object member)
        {
            switch (member)
            {
                case MethodInfo methodInfo:
                    return methodInfo.ReturnType;
                case FieldInfo fieldInfo:
                    return fieldInfo.FieldType;
                case Local local:
                    return local.LocalType;
                case Type type:
                    return type;
                default:
                    var (ltype, _) = ((Type, string)) member;
                    return ltype;
            }
        }

        public static object GetMember(string value, Dictionary<string, Type> definedTypes, LocalLookup locals,
            Type currentType, AssemblyBuilder asmBuilder,
            BindingFlags modifiers = BindingFlags.Public | BindingFlags.NonPublic,
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

            if (locals.Names.Contains(value))
                return locals[value];
            if (definedTypes.ContainsKey(value))
                return definedTypes[value];

            return DynamicMembers.GetInstance().GetMembers(currentType.Name).First(member =>
                member.Name == value &&
                (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Method));
        }

        public static bool CheckUnusedLocal(IList<Token> tokens, int firstInstructionIndex, string localName)
        {
            var methodEndIndex = firstInstructionIndex;
            var diff = 1;
            while (diff != 0)
            {
                if (tokens[methodEndIndex].TokenType == TokenType.Construction && tokens[methodEndIndex].Value != "ret")
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

        public static Type PushToStack(object member, Emit method)
        {
            switch (member)
            {
                case MethodInfo methodInfo:
                    if ((methodInfo.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
                        method.Call(methodInfo);
                    else method.CallVirtual(methodInfo);
                    return methodInfo.ReturnType;
                case FieldInfo fieldInfo:
                    method.LoadField(fieldInfo);
                    return fieldInfo.FieldType;
                case Local local:
                    method.LoadLocal(local);
                    return local.LocalType;
                case Type type:
                    return type;
                default:
                    var (ltype, lvalue) = ((Type, string)) member;
                    var parsed = ltype.GetMethod("Parse", new[] {typeof(string)}).Invoke(null, new[] {lvalue});
                    method.GetType().GetMethod("LoadConstant", new[] {ltype}).Invoke(method, new[] {parsed});
                    //method.LoadConstant(ltype, lvalue);
                    return ltype;
            }
        }

        public static Type PushToStack(string value, Emit method, Dictionary<string, Type> definedTypes,
            Type currentType, AssemblyBuilder asmBuilder)
        {
            var member = GetMember(value, definedTypes, method.Locals, currentType, asmBuilder);

            return PushToStack(member, method);
        }
    }
}