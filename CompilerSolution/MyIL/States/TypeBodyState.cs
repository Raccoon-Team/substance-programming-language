using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using CompilerUtilities.Exceptions;

namespace IL2MSIL
{
    internal class TypeBodyState : TypeChildState
    {
        private readonly List<string> modifs = new List<string>();

        public override void Execute(IList<Token> tokens, ref int i)
        {
            if (tokens[i].TokenType == TokenType.Modifier)
            {
                modifs.Add(tokens[i].Value);
                i++;
            }
            else if (tokens[i].Value == "func")
            {
                i++;
                StateStack.Push(new MethodState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, modifs.ToArray()));
                modifs.Clear();
            }
            else if (tokens[i].TokenType == TokenType.Type)
            {
                StateStack.Push(new FieldState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, modifs.ToArray()));
                modifs.Clear();
            }
            else if (tokens[i].TokenType == TokenType.End)
            {
                i++;
                StateStack.Pop();
                StateStack.Pop();
                TypeBuilder.CreateType();
            }
            else
            {
                //todo UnexpectedToken
                ExceptionManager.ThrowCompiler(ErrorCode.UnexpectedToken, "", tokens[i].Line);
            }
        }

        public TypeBodyState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, TypeBuilder typeBuilder) : base(stateStack, definedTypes, asmBuilder, typeBuilder)
        {
        }
    }
}