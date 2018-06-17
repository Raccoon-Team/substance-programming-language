using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    internal class ConstructionBodyState : MethodChildState
    {
        public override void Execute(IList<Token> tokens, ref int i)
        {
            if (tokens[i].TokenType == TokenType.End)
            {
                StateStack.Pop();
                StateStack.Pop();
            }
            else
            {
                StateStack.Push(new InstructionState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method));
            }
        }

        public ConstructionBodyState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, Type typeBuilder, Emit method) : base(stateStack, definedTypes, asmBuilder, typeBuilder, method)
        {
        }
    }
}