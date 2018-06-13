using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using CompilerUtilities.Exceptions;

namespace IL2MSIL
{
    internal class FieldState : TypeChildState
    {
        public string Name;
        public string Type;

        public override void Execute(IList<Token> tokens, ref int i)
        {
            if (tokens[i].TokenType == TokenType.Type)
            {
                Type = tokens[i++].Value;
            }
            else if (tokens[i].TokenType == TokenType.Identifier)
            {
                Name = tokens[i++].Value;
                StateStack.Pop();
            }
            else
            {
                //todo UnexpectedToken
                ExceptionManager.ThrowCompiler(ErrorCode.UnexpectedToken, "", tokens[i].Line);
            }
        }

        public FieldState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, TypeBuilder typeBuilder) : base(stateStack, definedTypes, asmBuilder, typeBuilder)
        {
        }
    }
}