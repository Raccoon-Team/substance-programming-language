using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using CompilerUtilities.Exceptions;
using Sigil.NonGeneric;

namespace IL2MSIL
{
    // ReSharper disable once InconsistentNaming
    public class ILTranslator
    {
        private readonly Dictionary<string, Type> _definedTypes;
        private readonly Dictionary<string, MethodInfo> _standartMethods;
        private AssemblyBuilder _asmBuilder;
        private TypeBuilder _currentTypeBuilder;
        private Emit _method;
        private ModuleBuilder _moduleBuilder;
        private IList<Token> _tokens;

        private ILTranslator()
        {
            _definedTypes = new Dictionary<string, Type>
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
            _standartMethods = new Dictionary<string, MethodInfo>
            {
                ["print"] = console.GetMethod("Write", new[] {typeof(string)}),
                ["println"] = console.GetMethod("WriteLine", new[] {typeof(string)}),
                ["read"] = console.GetMethod("Read", new Type[0]),
                ["readln"] = console.GetMethod("ReadLine", new Type[0]),
                ["pause"] = console.GetMethod("ReadKey", new Type[0]),
                ["start"] = typeof(Process).GetMethod("Start", new[] {typeof(string), typeof(string)})
            };
        }

        public static void Compile(string assemblyName, bool isExecutable, IList<string> lines)
        {
            new ILTranslator().CompileToFile(assemblyName, isExecutable, lines);
        }

        public void CompileToFile(string assemblyName, bool isExecutable, IList<string> lines)
        {
            _tokens = ILTokenizer.Tokenize(lines, _definedTypes.Select(x => x.Key).ToList(), out var customTypes);

            Initialize(assemblyName, isExecutable, out var fileName);
            //DefineTypes(customTypes);
            
            var stateMachine = new StateMachine(_definedTypes);
            var asm = stateMachine.GetGeneratedAssembly(_tokens, assemblyName, _asmBuilder, _moduleBuilder);
            
            if (isExecutable)
            {
                SetEntryPoint();
            }
            asm.Save(fileName);
        }

        private void SetEntryPoint()
        {
            var entryExists = false;
            foreach (var type in _definedTypes.Values)
            {
                var main = type.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                if (main != null)
                {
                    if (entryExists)
                        ExceptionManager.ThrowCompiler(ErrorCode.EntryPointAlreadyExists, String.Empty, -1);
                    _asmBuilder.SetEntryPoint(main);
                    entryExists = true;
                }
            }

            if (!entryExists)
                ExceptionManager.ThrowCompiler(ErrorCode.EntryPointNotExists, String.Empty, -1);
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
            _asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _asmBuilder.DefineDynamicModule(name, fileName);
        }

        private void DefineTypes(IList<(string typeName, TypeAttributes attributes)> customTypes)
        {
            var customTypesCount = customTypes.Count;
            for (var i = 0; i < customTypesCount; i++)
            {
                var (typeName, attributes) = customTypes[i];

                var typeBuilder = _moduleBuilder.DefineType(typeName, attributes);
                _definedTypes[typeName] = typeBuilder.AsType();
            }
        }
    }
}