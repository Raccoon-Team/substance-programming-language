using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Sigil;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    internal class InstructionState : MethodChildState
    {
        private readonly MethodBodyState _methodBodyState;

        public InstructionState(Stack<State> stateStack, Dictionary<string, Type> definedTypes,
            AssemblyBuilder asmBuilder, TypeBuilder typeBuilder, Emit method) : base(stateStack, definedTypes,
            asmBuilder, typeBuilder, method)
        {
            var bufferStack = new Stack<State>();
            while (true)
            {
                var state = stateStack.Pop();
                bufferStack.Push(state);
                if (!(state is MethodBodyState m))
                    continue;

                _methodBodyState = m;
                break;
            }

            while (bufferStack.Count != 0)
                stateStack.Push(bufferStack.Pop());
        }

        public override void Execute(IList<Token> tokens, ref int i)
        {
            var currentToken = tokens[i];
            if (currentToken.TokenType == TokenType.Type)
            {
                if (tokens[i + 1].TokenType == TokenType.Identifier)
                {
                    if (!ParserHelper.CheckUnusedLocal(tokens, _methodBodyState.FirstInstructionIndex,
                        tokens[i + 1].Value))
                    {
                        var type = DefinedTypes[currentToken.Value];
                        Method.DeclareLocal(type, tokens[i + 1].Value);
                    }
                    i += 2;
                }
            }
            else if (currentToken.TokenType == TokenType.Operator)
            {
                ProcessOperator(tokens, ref i);
            }
            else if (currentToken.TokenType == TokenType.Identifier)
            {
                if (tokens[i + 1].TokenType == TokenType.OpenBrace)
                {
                    StateStack.Push(new CallMethodState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method));
                    return;
                }
                if (tokens[i + 1].TokenType == TokenType.Operator)
                {
                    i++;
                    return;
                }
                throw new NotImplementedException();
            }
            else if (currentToken.Value == "ret")
            {
                if (tokens[i + 1].TokenType == TokenType.Identifier || tokens[i + 1].TokenType == TokenType.Constant)
                    ParserHelper.PushToStack(tokens[i + 1].Value, Method, DefinedTypes, TypeBuilder);
                Method.Return();
                i++;
            }

            StateStack.Pop();
        }

        private void ProcessOperator(IList<Token> tokens, ref int i)
        {
            if (tokens[i].Value == "=")
            {
                if (!_methodBodyState.UnusedLocals.Contains(tokens[i - 1].Value))
                {
                    //todo узнавать, это присвоение значения или результата вычислений, если последнее, то перенаправлять в следующий if


                    var variable = tokens[i - 1].Value;
                    //todo узнавать, переменная, константа или функция (вроде как сделал)
                    ParserHelper.PushToStack(tokens[i + 1].Value, Method, DefinedTypes, TypeBuilder);
                    //todo узнавать, переменная или поле (вроде как сделал)
                    var member = ParserHelper.GetMember(variable, DefinedTypes, Method.Locals, TypeBuilder);
                    switch (member)
                    {
                        case FieldInfo fieldInfo:
                            Method.StoreField(fieldInfo);
                            break;
                        case Local local:
                            Method.StoreLocal(local);
                            break;
                            default: throw new NotImplementedException("Нельзя присвоить значение константе или методу");
                    }
                }
                i += 2;
            }
            else
            {
                var left = tokens[i - 1].Value;
                var right = tokens[i + 1].Value;

                //todo узнавать, переменная, константа или функция (вроде как сделал)
                ParserHelper.PushToStack(left, Method, DefinedTypes, TypeBuilder);
                ParserHelper.PushToStack(right, Method, DefinedTypes, TypeBuilder);

                //todo добавить больше операций
                switch (tokens[i].Value)
                {
                    case "+":
                        Method.Add();
                        break;
                    case "-":
                        Method.Subtract();
                        break;
                    case "*":
                        Method.Multiply();
                        break;
                    case "/":
                        Method.Divide();
                        break;
                }
                i += 2;
            }
        }
    }
}