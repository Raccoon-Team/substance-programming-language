using System;
using System.Collections.Generic;

namespace IL2MSIL
{
    internal abstract class State
    {
        protected readonly Stack<State> StateStack;
        protected readonly Dictionary<string, Type> DefinedTypes;

        public State(Stack<State> stateStack, Dictionary<string, Type> definedTypes)
        {
            StateStack = stateStack;
            DefinedTypes = definedTypes;
        }

        public abstract void Execute(IList<Token> tokens, ref int i);
    }
}