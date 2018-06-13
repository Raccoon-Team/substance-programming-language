using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    internal class ConstructionState : MethodChildState
    {
        public override void Execute(IList<Token> tokens, ref int i)
        {
            if (tokens[i].TokenType == TokenType.Identifier)
            {
            }
            StateStack.Push(new ConstructionBodyState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method));
        }

        public ConstructionState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, TypeBuilder typeBuilder, Emit method) : base(stateStack, definedTypes, asmBuilder, typeBuilder, method)
        {
        }
    }
}