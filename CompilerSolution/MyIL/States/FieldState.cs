using System;
using System.Collections.Generic;
using System.Reflection.Emit;

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
                throw new NotImplementedException();
            }
        }

        public FieldState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, TypeBuilder typeBuilder) : base(stateStack, definedTypes, asmBuilder, typeBuilder)
        {
        }
    }
}