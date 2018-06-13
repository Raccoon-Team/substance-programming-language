using System.Collections.Generic;
using System.Reflection;

namespace IL2MSIL
{
    internal class Modifiers
    {
        private readonly Dictionary<string, TypeAttributes> _typeModifiers;
        private Dictionary<string, FieldAttributes> _fieldModifiers;

        public TypeAttributes this[string name] => _typeModifiers[name];

        public Modifiers()
        {
            _typeModifiers = new Dictionary<string, TypeAttributes>
            {
                ["public"] = TypeAttributes.Public,
                ["abstract"] = TypeAttributes.Abstract,
                ["sealed"] = TypeAttributes.Sealed,
                ["internal"] = TypeAttributes.NotPublic,
            };

            _fieldModifiers = new Dictionary<string, FieldAttributes>
            {
                ["public"] = FieldAttributes.Public,
                ["private"] = FieldAttributes.Private,
                ["internal"] = FieldAttributes.Assembly,
                ["static"] = FieldAttributes.Static,
                ["const"] = FieldAttributes.Literal,
            };
        }
    }
}
