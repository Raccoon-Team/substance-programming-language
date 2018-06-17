using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    internal abstract class MethodChildState : TypeChildState
    {
        protected Emit Method;

        public MethodChildState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, Type typeBuilder, Emit method) : base(
stateStack, definedTypes, asmBuilder, typeBuilder)
        {
            Method = method;
        }
    }
}