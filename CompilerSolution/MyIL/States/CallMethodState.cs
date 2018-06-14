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
            Type[] parameterTypes;

            var methodNameToken = tokens[i];
            if (!_hasRecursiveMethodCall)
            {
                tokensCount = tokens.Count;

                var closeIndex = ParserHelper.FindClosingBraceIndex(i + 1, tokens);
                paramsCount = closeIndex - i - 2;

                //callingMethod = 
                //    (MethodInfo) ((MemberInfo[]) ParserHelper.GetMember(tokens[i].Value, DefinedTypes, Method.Locals,
                //        TypeBuilder))[0];
                i++;
                parameterTypes = ParseParameters(tokens, ref i);
            }
            else
            {
                paramsCount -= recursiveState.paramsCount + 4;
                parameterTypes = ParseParameters(tokens, ref i);
            }
            if (parameterTypes is null)
                return;

            callingMethod = AsmBuilder.GetTypes().First(x => x.FullName == TypeBuilder.FullName).GetMethod(methodNameToken.Value, parameterTypes);

            if (callingMethod is null)
                ExceptionManager.ThrowCompiler(ErrorCode.UnexpectedToken, string.Empty, methodNameToken.Line);

            if ((callingMethod.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
                Method.Call(callingMethod);
            else
                Method.CallVirtual(callingMethod, TypeBuilder, parameterTypes);

            StateStack.Pop();
        }

        private Type[] ParseParameters(IList<Token> tokens, ref int i)
        {
            var parameters = new Type[paramsCount];
            var instructionState = new InstructionState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method);

            i++;
            for (var j = 0; j < paramsCount; j++)
            {
                instructionState.Execute(tokens, ref i);
                parameters[j] = ParserHelper.GetMemberType(InstructionState.PrevMember);
            }
            i++;
            return parameters;
        }

        /*private Type[] ParseParameters(IList<Token> tokens, ref int i)
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

                //todo если метод имеет перегрузки, будет BOOOOOOM
                if (member is MemberInfo[] methods)
                    member = (MethodInfo) methods[0];

                if (member is MethodInfo)
                {
                    recursiveState = new CallMethodState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method);
                    StateStack.Push(recursiveState);
                    _hasRecursiveMethodCall = true;
                    return null;
                }

                ParserHelper.PushToStack(member, Method);
            }
            i++;
            return callingMethod.GetParameters().Select(x => x.ParameterType).ToArray();
        }*/
    }
}