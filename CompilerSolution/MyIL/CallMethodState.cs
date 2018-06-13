using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Sigil;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    internal class CallMethodState : MethodChildState
    {
        private bool _hasRecursiveMethodCall;
        private MethodInfo callingMethod;
        private int paramsCount, tokensCount;
        private CallMethodState recursiveState;

        public CallMethodState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, TypeBuilder typeBuilder, Emit method) : base(stateStack, definedTypes, asmBuilder, typeBuilder, method)
        {
        }


        public override void Execute(IList<Token> tokens, ref int i)
        {
            if (!_hasRecursiveMethodCall)
            {
                tokensCount = tokens.Count;

                var closeIndex = ParserHelper.FindClosingBraceIndex(i + 1, tokens);
                paramsCount = closeIndex - i - 2;

                callingMethod =
                    (MethodInfo) ((MemberInfo[]) ParserHelper.GetMember(tokens[i].Value, DefinedTypes, Method.Locals,
                        TypeBuilder))[0];
                i++;
                ParseParameters(tokens, ref i);
            }
            else
            {
                paramsCount -= recursiveState.paramsCount + 4;
                ParseParameters(tokens, ref i);
                i++;
            }
        }

        private void ParseParameters(IList<Token> tokens, ref int i)
        {
            var localParamsCount = paramsCount;

            while (localParamsCount-- > 0)
            {
                i++;
                MemberTypes memberType;
                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (i + 1 < tokensCount && tokens[i + 1].TokenType == TokenType.OpenBrace)
                    memberType = MemberTypes.Method;
                else
                    memberType = MemberTypes.Field;

                var member = ParserHelper.GetMember(tokens[i].Value, DefinedTypes, Method.Locals, TypeBuilder,
                    BindingFlags.Public | BindingFlags.NonPublic, memberType);

                if (member is MemberInfo[])
                    member = (MethodInfo) ((MemberInfo[]) member)[0];

                switch (member)
                {
                    case FieldInfo fieldInfo:
                        Method.LoadField(fieldInfo);
                        break;
                    case MethodInfo methodInfo:
                        recursiveState = new CallMethodState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method);
                        StateStack.Push(recursiveState);
                        _hasRecursiveMethodCall = true;
                        return;
                    case Local local:
                        Method.LoadLocal(local);
                        break;
                    default:
                        var (retType, value) = ((Type, object)) member;
                        Method.LoadConstant(retType, value.ToString());
                        break;
                }
            }
            i++;
            var parameterTypes = callingMethod.GetParameters().Select(x => x.ParameterType).ToArray();

            if ((callingMethod.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
                Method.Call(callingMethod);
            //Method.Call(callingMethod, parameterTypes);
            else
                Method.CallVirtual(callingMethod, TypeBuilder, parameterTypes);

            StateStack.Pop();
        }
    }
}