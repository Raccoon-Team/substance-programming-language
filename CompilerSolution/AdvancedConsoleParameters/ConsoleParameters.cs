using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdvancedConsoleParameters
{
    public static class ConsoleParameters
    {
        public static List<Parameter> Parse(string[] args)
        {
            var parameters = new List<Parameter>();
            var length = args.Length;

            for (var i = 0; i < length; i++)
            {
                if (args[i][0] == '-' && !char.IsDigit(args[i][1]))
                    parameters.Add(new Parameter(args[i].Substring(1)));
                else
                    parameters.Last().Values.Add(args[i]);
            }

            return parameters;
        }

        public static void Initialize(params string[] args)
        {
            var parameters = Parse(args);
            Initialize(parameters, args);
        }

        public static void Initialize(List<Parameter> parameters, params string[] args)
        {
            var asm = Assembly.GetCallingAssembly();
            foreach (var type in asm.DefinedTypes)
            {
                SetValues(type.DeclaredFields, parameters);
                SetValues(type.DeclaredProperties, parameters);
                SetValues(type.DeclaredMethods, parameters);
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
            property.SetValue(property, Convert.ChangeType(value[0], property.PropertyType));
        }

        private static void SetValues<T>(IEnumerable<T> declaredMembers, List<Parameter> parameters)
            where T : MemberInfo
        {
            foreach (var member in declaredMembers)
            {
                var atr = member.GetCustomAttribute<ParameterAttribute>();
                if (atr == null)
                    continue;

                var parameter = parameters.Find(x => atr.Key.Split('|').Contains(x.Name));
                if (parameter == null)
                    continue;

                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        SetValue(member as FieldInfo, parameter.Values);
                        break;
                    case MemberTypes.Method:
                        var method = member as MethodInfo;
                        var parametersInfo = method.GetParameters();
                        var methodParams = parameter.Values
                            .Select((s, i) => Convert.ChangeType(s, parametersInfo[i].ParameterType)).ToArray();
                        method.Invoke(method, methodParams);
                        break;
                    case MemberTypes.Property:
                        SetValue(member as PropertyInfo, parameter.Values);
                        break;
                }
            }
        }
    }
}