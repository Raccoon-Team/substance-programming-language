using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using CompilerUtilities.Exceptions;

namespace IL2MSIL
{
    internal class FieldState : TypeChildState
    {
        private readonly IList<string> _modifs;
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
                var fieldAttributes = ModifierCollection.GetFieldAttributes(_modifs);
                var field = ((TypeBuilder)TypeBuilder).DefineField(Name, DefinedTypes[Type], fieldAttributes);

                DynamicMembers.GetInstance().AddMember(TypeBuilder.Name, field);
                StateStack.Pop();
            }
            else
            {
                //todo UnexpectedToken
                ExceptionManager.ThrowCompiler(ErrorCode.UnexpectedToken, "", tokens[i].Line);
            }
        }

        public FieldState(Stack<State> stateStack, Dictionary<string, Type> definedTypes, AssemblyBuilder asmBuilder, Type typeBuilder, IList<string> modifs) : base(stateStack, definedTypes, asmBuilder, typeBuilder)
        {
            _modifs = modifs;
        }
    }
}