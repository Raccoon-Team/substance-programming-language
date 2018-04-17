using System;
using System.Reflection;

namespace AdvancedConsoleParameters.Exceptions
{
    public class InvalidMethodArgumentTypes : Exception
    {
        public InvalidMethodArgumentTypes(MethodInfo methodInfo) : base(
            $"Атрибут ParameterAttribute невозможно применить методу {methodInfo}, так как он имеет недопустимые параметры. Допустимы только стандартные числовые параметры, string. Если в параметрах присутствует массив, то других параметров быть не должно. Так же массив может быть только типа string[]")
        {
        }
    }
}