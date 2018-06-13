using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace IL2MSIL
{
    internal class InitializeState : AssemblyChildState
    {
        private readonly ModuleBuilder _moduleBuilder;

        public InitializeState(Stack<State> stateStack, Dictionary<string, Type> definedTypes,
            AssemblyBuilder asmBuilder, ModuleBuilder moduleBuilder) : base(stateStack, definedTypes, asmBuilder)
        {
            _moduleBuilder = moduleBuilder;
        }

        public override void Execute(IList<Token> tokens, ref int i)
        {
            if (tokens[i].TokenType == TokenType.TypeDef || tokens[i].TokenType == TokenType.Modifier)
                StateStack.Push(new TypeState(StateStack, DefinedTypes, AsmBuilder, _moduleBuilder));

            else if (tokens[i].TokenType == TokenType.Namespace)
                StateStack.Push(new NamespaceState(StateStack, DefinedTypes, AsmBuilder));

            else if (tokens[i].TokenType == TokenType.Using)
                StateStack.Push(new UsingState(StateStack, DefinedTypes, AsmBuilder));
        }
    }
}