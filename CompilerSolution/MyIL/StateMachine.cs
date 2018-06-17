using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace IL2MSIL
{
    internal class StateMachine
    {
        private readonly Stack<State> _state;
        private readonly Dictionary<string, Type> _definedTypes;

        public StateMachine(Dictionary<string, Type> definedTypes)
        {
            _definedTypes = definedTypes;
            _state = new Stack<State>();
        }

        public AssemblyBuilder GetGeneratedAssembly(IList<Token> tokens, string assemblyName, AssemblyBuilder asmBuilder, ModuleBuilder module)
        {
            //var asmBuilder = CreateAssemblyBuilder(assemblyName);
            //var module = asmBuilder.DefineDynamicModule(assemblyName, assemblyName + ".exe");

            _state.Push(new InitializeState(_state, _definedTypes, asmBuilder, module));

            var i = 0;
            var tokensCount = tokens.Count;
            while (i < tokensCount)
                _state.Peek().Execute(tokens, ref i);

            return asmBuilder;
        }

        //private static AssemblyBuilder CreateAssemblyBuilder(string assemblyName)
        //{
        //    var domain = AppDomain.CurrentDomain;
        //    var asmName = new AssemblyName(assemblyName);
        //    return domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
        //}
    }
}