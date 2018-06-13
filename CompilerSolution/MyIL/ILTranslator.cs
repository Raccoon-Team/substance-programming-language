using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    // ReSharper disable once InconsistentNaming
    public class ILTranslator
    {
        private readonly Dictionary<string, Type> definedTypes;
        private readonly Dictionary<string, MethodInfo> standartMethods;
        private AssemblyBuilder asmBuilder;
        private TypeBuilder currentTypeBuilder;
        private Emit method;
        private ModuleBuilder moduleBuilder;
        private IList<Token> tokens;

        public ILTranslator()
        {
            definedTypes = new Dictionary<string, Type>
            {
                ["void"] = typeof(void),
                ["int"] = typeof(int),
                ["string"] = typeof(string),
                ["double"] = typeof(double),
                ["byte"] = typeof(byte),
                ["char"] = typeof(char),
                ["short"] = typeof(short),
                ["long"] = typeof(long),
                ["ushort"] = typeof(ushort),
                ["ulong"] = typeof(ulong),
                ["uint"] = typeof(uint),
                ["decimal"] = typeof(decimal),
                ["bool"] = typeof(bool),
                ["big_int"] = typeof(BigInteger)
            };

            var console = typeof(Console);
            standartMethods = new Dictionary<string, MethodInfo>
            {
                ["print"] = console.GetMethod("Write", new[] {typeof(string)}),
                ["println"] = console.GetMethod("WriteLine", new[] {typeof(string)}),
                ["read"] = console.GetMethod("Read", new Type[0]),
                ["readln"] = console.GetMethod("ReadLine", new Type[0]),
                ["pause"] = console.GetMethod("ReadKey", new Type[0]),
                ["start"] = typeof(Process).GetMethod("Start", new[] {typeof(string), typeof(string)})
            };
        }

        public void CompileToFile(string assemblyName, bool isExecutable, IList<string> lines)
        {
            tokens = new ILTokenizer().Tokenize(lines, definedTypes.Select(x => x.Key).ToList(), out var customTypes);

            Initialize(assemblyName, isExecutable, out var fileName);
            DefineTypes(customTypes);
            
            var stateMachine = new StateMachine(definedTypes);
            var asm = stateMachine.GetGeneratedAssembly(tokens, assemblyName);
            asm.Save(fileName);
        }

        private void Initialize(string name, bool isExecutable, out string fileName)
        {
            fileName = name;
            if (isExecutable)
                fileName += ".exe";
            else
                fileName += ".dll";

            var domain = Thread.GetDomain();

            var asmName = new AssemblyName(name);
            asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            moduleBuilder = asmBuilder.DefineDynamicModule(name, fileName);
        }

        private void DefineTypes(IList<(string typeName, TypeAttributes attributes)> customTypes)
        {
            var customTypesCount = customTypes.Count;
            for (var i = 0; i < customTypesCount; i++)
            {
                var (typeName, attributes) = customTypes[i];

                var typeBuilder = moduleBuilder.DefineType(typeName, attributes);
                definedTypes[typeName] = typeBuilder.AsType();
            }
        }
    }
}