using System.Collections.Generic;
using System.Reflection;

namespace IL2MSIL
{
    class Modifiers
    {
        private Dictionary<string, TypeAttributes> typeModifiers;
        private Dictionary<string, FieldAttributes> fieldModifiers;

        public TypeAttributes this[string name] => typeModifiers[name];

        public Modifiers()
        {
            typeModifiers = new Dictionary<string, TypeAttributes>
            {
                ["public"] = TypeAttributes.Public,
                ["abstract"] = TypeAttributes.Abstract,
                ["sealed"] = TypeAttributes.Sealed,
                ["internal"] = TypeAttributes.NotPublic,
            };

            fieldModifiers = new Dictionary<string, FieldAttributes>
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
