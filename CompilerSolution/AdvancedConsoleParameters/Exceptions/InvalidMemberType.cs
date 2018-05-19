using System;
using System.Reflection;

namespace AdvancedConsoleParameters.Exceptions
{
    [Serializable]
    public class InvalidMemberType : Exception
    {
        public InvalidMemberType(MemberInfo memberInfo, Type invalidType) : base(
            $"Атрибут ParameterAttribute невозможно применить члену {memberInfo}, так как он имеет недопустимый тип {invalidType}. Доступные типы членов: string[], string, все стандартные числовые типы.")
        {
        }
    }
}