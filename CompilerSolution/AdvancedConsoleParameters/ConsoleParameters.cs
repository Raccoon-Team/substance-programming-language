using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdvancedConsoleParameters.Exceptions;
using CompilerUtilities.Exceptions;

namespace AdvancedConsoleParameters
{
    public static class ConsoleParameters
    {
        public static List<Parameter> Parse(string[] args)
        {
            var parameters = new List<Parameter>();
            var length = args.Length;

            for (var i = 0; i < length; i++)
                if (args[i][0] == '-' && !char.IsDigit(args[i][1]))
                    parameters.Add(new Parameter(args[i].Substring(1)));
                else
                    parameters.Last().Values.Add(args[i]);

            return parameters;
        }

        private static void ValidateAttributedMembersTypes(IEnumerable<MemberInfo> members)
        {
            void CheckArray(Type type, MemberInfo memberInfo)
            {
                if (type.IsArray && type.GetElementType() != typeof(string[]))
                    throw new InvalidMemberType(memberInfo, type);
            }

            foreach (var member in members)
                switch (member)
                {
                    case FieldInfo fieldInfo:
                        CheckArray(fieldInfo.FieldType, fieldInfo);
                        break;
                    case PropertyInfo propertyInfo:
                        CheckArray(propertyInfo.PropertyType, propertyInfo);
                        break;
                    case MethodInfo methodInfo:
                        var paramsInfo = methodInfo.GetParameters();
                        var parameterInfo = paramsInfo.FirstOrDefault(x => x.ParameterType.IsArray);
                        if (parameterInfo != null && (paramsInfo.Length != 1 || parameterInfo.ParameterType != typeof(string[])))
                            throw new InvalidMethodArgumentTypes(methodInfo);
                        break;
                }
        }

        private static List<(MemberInfo member, Parameter parameter)> GetAttributedMembers(Assembly asm,
            List<Parameter> parameters)
        {
            var outp = new List<(MemberInfo member, Parameter parameter)>();

            foreach (var type in asm.DefinedTypes)
            {
                var pairs = type.DeclaredMembers.Select(m => (m, m.GetCustomAttribute<ParameterAttribute>()))
                    .Where(m => m.Item2 != null);
                //outp.AddRange(pairs.Select(p => (p.Item1, parameters.Find(parameter => p.Item2.Keys.Contains(parameter.Key)))).Where(x => x.Item2 != null));

                ValidateAttributedMembersTypes(pairs.Select(x => x.Item1));

                foreach (var pair in pairs)
                {
                    var item = (pair.Item1, parameters.Find(parameter => pair.Item2.Keys.Contains(parameter.Key)));

                    if (item.Item2 == null)
                        continue;

                    item.Item2.SetUp(pair.Item2);

                    outp.Add(item);
                }
            }

            return outp;
        }

        public static void Initialize(params string[] args)
        {
            var parameters = Parse(args);
            Initialize(parameters);
        }

        public static void Initialize(List<Parameter> parameters)
        {
            var asm = Assembly.GetEntryAssembly();

            var members = GetAttributedMembers(asm, parameters);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < members.Count; i++)
            {
                var (member, parameter) = members[i];

                var values = parameter.Values;

                switch (member)
                {
                    case FieldInfo fieldInfo:
                        SetValue(fieldInfo, values);
                        break;
                    case MethodInfo methodInfo:
                        var paramsInfo = methodInfo.GetParameters();

                        object[] methodParams;
                        if (paramsInfo[0].ParameterType.IsArray)
                        {
                            methodParams = new[] {values.ToArray()};
                        }
                        else
                        {
                            if (values.Count != paramsInfo.Length)
                                throw new MismatchOfArgumentCount(parameter.Key, paramsInfo.Length,
                                    values.Count);
                            methodParams = values
                                .Select((s, index) => Convert.ChangeType(s, paramsInfo[index].ParameterType)).ToArray();
                        }

                        methodInfo.Invoke(methodInfo, methodParams);
                        break;
                    case PropertyInfo propertyInfo:
                        SetValue(propertyInfo, values);
                        break;
                }
            }
        }

        private static void SetValue(FieldInfo field, IList<string> value)
        {
            SetValue(field, field.FieldType, field.SetValue, value);
        }

        private static void SetValue(PropertyInfo property, IList<string> value)
        {
            SetValue(property, property.PropertyType, property.SetValue, value);
        }

        private static void SetValue(MemberInfo memberInfo, Type memberType,
            Action<object, object> setValueMethod, IList<string> values)
        {
            var value = memberType.IsArray ? values.ToArray() : Convert.ChangeType(values[0], memberType);

            setValueMethod(memberInfo, value);
        }
    }
}