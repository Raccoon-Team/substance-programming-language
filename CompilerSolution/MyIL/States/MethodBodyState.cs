using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    internal class MethodBodyState : MethodChildState
    {
        private readonly List<(Type type, string name)> _parameters;
        public string[] UnusedLocals;
        public readonly int FirstInstructionIndex;

        public MethodBodyState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, Type typeBuilder, Emit method, List<(Type type, string name)> parameters, int i) : base(stateStack,
                definedTypes, asmBuilder, typeBuilder, method)
        {
            _parameters = parameters;
            parameters.ForEach(p => Method.DeclareLocal(p.type, p.name));
            FirstInstructionIndex = i;
        }

        public override void Execute(IList<Token> tokens, ref int i)
        {
            if (tokens[i].TokenType == TokenType.Construction)
            {
                //parsish
                StateStack.Push(new ConstructionState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method));
            }
            else if (tokens[i].TokenType == TokenType.End)
            {
                i++;
                StateStack.Pop();
                StateStack.Pop();
                Method.CreateMethod();
            }
            else
            {
                StateStack.Push(new InstructionState(StateStack, DefinedTypes, AsmBuilder, TypeBuilder, Method));
            }
        }
    }
}