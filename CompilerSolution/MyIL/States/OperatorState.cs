using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    internal class OperatorState : MethodChildState
    {
        public OperatorState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, TypeBuilder typeBuilder, Emit method) : base(stateStack, definedTypes, asmBuilder, typeBuilder, method)
        {
        }

        public override void Execute(IList<Token> tokens, ref int i)
        {
            throw new NotImplementedException();
        }
    }
}