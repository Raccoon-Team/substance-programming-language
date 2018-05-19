using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdvancedConsoleParameters.Exceptions;

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
                    parameters.Add(new Parameter(args[i]));
                else
                    parameters.Last().Values.Add(args[i]);

            return parameters;
        }

        public static List<Parameter> GetAllAvailableParameters()
        {
            var asm = Assembly.GetEntryAssembly();
            var assemblies = asm.GetReferencedAssemblies().Select(Assembly.Load).Prepend(asm);

            var allAttributes = new List<ParameterAttribute>();
            foreach (var assembly in assemblies)
            foreach (var definedType in assembly.DefinedTypes)
                allAttributes.AddRange(definedType.DeclaredMembers
                    .Select(x => x.GetCustomAttribute<ParameterAttribute>())
                    .Where(x => x != null));

            return allAttributes.Select(x => new Parameter(string.Join("|", x.Keys))
            {
                IsFlag = x.IsFlag,
                PossibleValues = x.PossibleValues.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries),
                Description = x.Description
            }).ToList();
        }

        public static void Initialize(object[] instances, params string[] args)
        {
            var parameters = Parse(args);
            Initialize(parameters, instances);
        }

        public static void Initialize(List<Parameter> parameters, object[] instances)
        {
            var members =
                new List<(MemberInfo member, Parameter parameter, object instance)>(GetAllAttributedMembers(instances, parameters));

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < members.Count; i++)
            {
                var (member, parameter, instance) = members[i];

                var values = parameter.Values;

                switch (member)
                {
                    case FieldInfo fieldInfo:
                        SetValue(fieldInfo, instance, values);
                        break;
                    case MethodInfo methodInfo:
                        var paramsInfo = methodInfo.GetParameters();

                        var methodParams = new object[0];
                        if (paramsInfo.Length > 0)
                            if (paramsInfo[0].ParameterType.IsArray)
                            {
                                methodParams = new object[] {values.ToArray()};
                            }
                            else
                            {
                                if (values.Count != paramsInfo.Length)
                                    throw new MismatchOfArgumentCount(parameter.Key, paramsInfo.Length,
                                        values.Count);
                                methodParams = values
                                    .Select((s, index) => Convert.ChangeType(s, paramsInfo[index].ParameterType))
                                    .ToArray();
                            }

                        methodInfo.Invoke(instance, methodParams);
                        break;
                    case PropertyInfo propertyInfo:
                        SetValue(propertyInfo, instance, values);
                        break;
                }
            }
        }

        private static IEnumerable<(MemberInfo member, Parameter parameter, object instance)> GetAllAttributedMembers(
            IList<object> instances, List<Parameter> parameters)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];
                var members = GetAttributedMembersFromInstance(instance, parameters);

                // ReSharper disable once ForCanBeConvertedToForeach
                for (var j = 0; j < members.Count; j++)
                    yield return members[j];
            }
        }

        private static List<(MemberInfo member, Parameter parameter, object instance)> GetAttributedMembersFromInstance(object instance,
            List<Parameter> parameters)
        {
            var outp = new List<(MemberInfo member, Parameter parameter, object instance)>();

            var type = instance.GetType().GetTypeInfo();
            var pairs = type.DeclaredMembers
                .Select(m => (memberInfo:m, attribute:m.GetCustomAttribute<ParameterAttribute>()))
                .Where(m => m.attribute != null);

            ValidateAttributedMembersTypes(pairs.Select(x => x.memberInfo));

            foreach (var(memberInfo, attribute) in pairs)
            {
                var item = (memberInfo, parameter:parameters.Find(parameter =>
                    attribute.Keys.Contains(parameter.Key)), instance);

                if (item.parameter == null)
                    continue;

                item.parameter.SetUp(attribute);

                outp.Add(item);
            }

            return outp;
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
                        if (parameterInfo != null &&
                            (paramsInfo.Length != 1 || parameterInfo.ParameterType != typeof(string[])))
                            throw new InvalidMethodArgumentTypes(methodInfo);
                        break;
                }
        }

        private static void SetValue(FieldInfo field, object instance, IList<string> value)
        {
            SetValue(instance, field.FieldType, field.SetValue, value);
        }

        private static void SetValue(PropertyInfo property, object instance, IList<string> value)
        {
            SetValue(instance, property.PropertyType, property.SetValue, value);
        }

        private static void SetValue(object instance, Type memberType,
            Action<object, object> setValueMethod, IList<string> values)
        {
            var value = memberType.IsArray ? values.ToArray() : Convert.ChangeType(values[0], memberType);

            setValueMethod(instance, value);
        }
    }
}