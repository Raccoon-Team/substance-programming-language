using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace IL2MSIL
{
    internal abstract class AssemblyChildState : State
    {
        protected readonly AssemblyBuilder AsmBuilder;

        protected AssemblyChildState(Stack<State> stateStack, Dictionary<string, Type> definedTypes,
            AssemblyBuilder asmBuilder) : base(stateStack, definedTypes)
        {
            AsmBuilder = asmBuilder;
        }
    }
}