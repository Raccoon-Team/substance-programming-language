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
    internal class InstructionState : MethodChildState
    {
        private readonly MethodBodyState _methodBodyState;

        private bool IsInner;
        private string literal;

        public static object PrevMember;

        public InstructionState(Stack<State> stateStack, Dictionary<string, Type> definedTypes,
            AssemblyBuilder asmBuilder, Type typeBuilder, Emit method) : base(stateStack, definedTypes,
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

            if (literal != null)
            {
                //ParserHelper.PushToStack(PrevMember, Method);
                //var member = ParserHelper.GetMember(literal, DefinedTypes, Method.Locals, TypeBuilder);

                if (PrevMember is MemberInfo[] members)
                    PrevMember = members[0];

                switch (PrevMember)
                {
                    case FieldInfo field:
                        Method.StoreField(field);
                        break;
                    case Local local:
                        Method.StoreLocal(local);
                        break;
                    default:
                        ExceptionManager.ThrowCompiler(ErrorCode.NotPossibleToSetValue, "", currentToken.Line);
                        break;
                }
            }

            if (IsInner)
            {
                if (tokens[i + 1].TokenType != TokenType.Operator)
                {
                    ParserHelper.PushToStack(tokens[i].Value, Method, DefinedTypes, TypeBuilder, AsmBuilder);
                    i++;
                }
                else
                {
                    var left = tokens[i].Value;
                    var right = tokens[i + 2].Value;

                    //todo узнавать, переменная, константа или функция (вроде как сделал)
                    ParserHelper.PushToStack(left, Method, DefinedTypes, TypeBuilder, AsmBuilder);
                    ParserHelper.PushToStack(right, Method, DefinedTypes, TypeBuilder, AsmBuilder);

                    //todo добавить больше операций
                    switch (tokens[i + 1].Value)
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
                    i += 3;
                }
            }
            else
            {
                if (currentToken.TokenType == TokenType.Type)
                {
                    if (tokens[i + 1].TokenType == TokenType.Identifier)
                        if (!ParserHelper.CheckUnusedLocal(tokens, _methodBodyState.FirstInstructionIndex,
                            tokens[i + 1].Value))
                        {
                            var type = DefinedTypes[currentToken.Value];
                            Method.DeclareLocal(type, tokens[i + 1].Value);
                            i++;
                        }
                    i++;
                    PrevMember = DefinedTypes[currentToken.Value];
                }
                else if (currentToken.Value == "=") //(currentToken.TokenType == TokenType.Operator)
                {
                    StateStack.Push(new InstructionState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method)
                    {
                        IsInner = true
                    });
                    literal = tokens[i - 1].Value;
                    i++;
                    return;
                    //ProcessOperator(tokens, ref i);
                }
                else if (currentToken.TokenType == TokenType.Identifier)
                {
                    if (tokens[i + 1].TokenType == TokenType.OpenBrace)
                    {
                        StateStack.Push(new CallMethodState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method));
                        return;
                    }
                    PrevMember = ParserHelper.GetMember(currentToken.Value, DefinedTypes, Method.Locals, TypeBuilder, AsmBuilder);
                    i++;
                }
                else if (currentToken.Value == "ret")
                {
                    var nextToken = tokens[i + 1];
                    if (nextToken.TokenType == TokenType.Identifier ||
                        nextToken.TokenType == TokenType.Constant)
                    {
                        ParserHelper.PushToStack(nextToken.Value, Method, DefinedTypes, TypeBuilder, AsmBuilder);
                        i++;
                    }
                    Method.Return();
                    i++;
                }
                else if (currentToken.Value == ".")
                {
                    MemberInfo[] members;
                    if (PrevMember is Type type)
                    {
                        type = DefinedTypes[type.FullName];
                        members = type.GetMember(tokens[i + 1].Value);
                    }
                    else
                        members = PrevMember.GetType().GetMember(tokens[i + 1].Value);
                    if (members.Length == 0)
                        ExceptionManager.ThrowCompiler(ErrorCode.UnexpectedToken, string.Empty, tokens[i + 1].Line);

                    var currMember = members[0];

                    if (currMember is MethodInfo)
                    {
                        i++;
                        ParserHelper.PushToStack(PrevMember, Method);
                        Method.Box(ParserHelper.GetMemberType(PrevMember));

                        var callMethodState = new CallMethodState(StateStack, DefinedTypes, AsmBuilder,
                            ((MethodInfo) currMember).DeclaringType, Method);
                        StateStack.Push(callMethodState);
                        callMethodState.Execute(tokens, ref i);
                        return;
                    }
                    else
                    {
                        i += 2;
                    }
                    PrevMember = currMember;
                }
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
                    ParserHelper.PushToStack(tokens[i + 1].Value, Method, DefinedTypes, TypeBuilder, AsmBuilder);
                    //todo узнавать, переменная или поле (вроде как сделал)
                    var member = ParserHelper.GetMember(variable, DefinedTypes, Method.Locals, TypeBuilder, AsmBuilder);
                    switch (member)
                    {
                        case FieldInfo fieldInfo:
                            Method.StoreField(fieldInfo);
                            break;
                        case Local local:
                            Method.StoreLocal(local);
                            break;
                        default:
                            //todo NotPossibleToSetValue
                            ExceptionManager.ThrowCompiler(ErrorCode.NotPossibleToSetValue, "", tokens[i].Line);
                            break;
                    }
                }
                i += 2;
            }
            else
            {
                var left = tokens[i - 1].Value;
                var right = tokens[i + 1].Value;

                //todo узнавать, переменная, константа или функция (вроде как сделал)
                ParserHelper.PushToStack(left, Method, DefinedTypes, TypeBuilder, AsmBuilder);
                ParserHelper.PushToStack(right, Method, DefinedTypes, TypeBuilder, AsmBuilder);

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