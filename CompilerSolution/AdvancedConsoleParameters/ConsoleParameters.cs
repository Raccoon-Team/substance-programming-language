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
            var validFieldTypes = new[] {typeof(string[]), typeof(int), typeof(double), typeof(long)};

            foreach (var member in members)
                switch (member)
                {
                    case FieldInfo fieldInfo:
                        var type = fieldInfo.FieldType;
                        if (type.IsArray)
                            if (type.GetElementType() != typeof(string[]))
                                throw new InvalidMemberType(fieldInfo, type);
                        break;
                    case PropertyInfo propertyInfo:
                        type = propertyInfo.PropertyType;
                        if (type.IsArray)
                            if (type.GetElementType() != typeof(string[]))
                                throw new InvalidMemberType(propertyInfo, type);
                        break;
                    case MethodInfo methodInfo:
                        var paramsInfo = methodInfo.GetParameters();
                        ParameterInfo parameterInfo;
                        if ((parameterInfo = paramsInfo.FirstOrDefault(x => x.ParameterType.IsArray)) != null)
                            if (paramsInfo.Length != 1 || parameterInfo.ParameterType != typeof(string[]))
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

                switch (member)
                {
                    case FieldInfo fieldInfo:
                        SetValue(fieldInfo, parameter.Values);
                        break;
                    case MethodInfo methodInfo:
                        var paramsInfo = methodInfo.GetParameters();

                        object[] methodParams;
                        if (paramsInfo[0].ParameterType.IsArray)
                        {
                            methodParams = new[] {parameter.Values.ToArray()};
                        }
                        else
                        {
                            if (parameter.Values.Count != paramsInfo.Length)
                                throw new MismatchOfArgumentCount(parameter.Key, paramsInfo.Length,
                                    parameter.Values.Count);
                            methodParams = parameter.Values
                                .Select((s, index) => Convert.ChangeType(s, paramsInfo[index].ParameterType)).ToArray();
                        }

                        methodInfo.Invoke(methodInfo, methodParams);
                        break;
                    case PropertyInfo propertyInfo:
                        SetValue(propertyInfo, parameter.Values);
                        break;
                }
            }
        }

        private static void SetValue(FieldInfo field, IList<string> value)
        {
            var fieldType = field.FieldType;
            if (fieldType.IsArray)
                field.SetValue(field, value.ToArray());
            else
                field.SetValue(field, Convert.ChangeType(value[0], field.FieldType));
        }

        private static void SetValue(PropertyInfo property, IList<string> value)
        {
            var fieldType = property.PropertyType;
            if (fieldType.IsArray)
                property.SetValue(property, value.ToArray());
            else
                property.SetValue(property, Convert.ChangeType(value[0], fieldType));
        }
    }
}