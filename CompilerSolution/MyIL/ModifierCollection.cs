using System.Collections.Generic;
using System.Reflection;

namespace IL2MSIL
{
    internal class ModifierCollection
    {
        public static Dictionary<string, TypeAttributes> TypeAttributeses;
        public static Dictionary<string, TypeAttributes> TypeModifiers;

        public static Dictionary<string, MethodAttributes> MethodAttributeses;
        public static Dictionary<string, MethodAttributes> MethodModifiers;

        public static Dictionary<string, FieldAttributes> FieldAttributeses;
        public static Dictionary<string, FieldAttributes> FieldModifiers;

        public static void kek()
        {
            
        }

        static ModifierCollection()
        {
            TypeAttributeses = new Dictionary<string, TypeAttributes>
            {
                ["abstract"] = TypeAttributes.Abstract,
                ["sealed"] = TypeAttributes.Sealed,
                ["static"] = TypeAttributes.Abstract | TypeAttributes.Sealed,

                ["class"] = TypeAttributes.Class,
                ["interface"] = TypeAttributes.Interface
            };

            TypeModifiers = new Dictionary<string, TypeAttributes>
            {
                ["public"] = TypeAttributes.Public,
                ["internal"] = TypeAttributes.NotPublic,
            };

            MethodAttributeses = new Dictionary<string, MethodAttributes>
            {
                ["static"] = MethodAttributes.Static,
                ["sealed"] = MethodAttributes.Final,
                ["abstract"] = MethodAttributes.Abstract,
                ["virtual"] = MethodAttributes.Virtual,
            };

            MethodModifiers = new Dictionary<string, MethodAttributes>
            {
                ["public"] = MethodAttributes.Public,
                ["internal"] = MethodAttributes.Assembly,
                ["private"] = MethodAttributes.Private,
                ["protected"] = MethodAttributes.Family,
            };

            FieldAttributeses = new Dictionary<string, FieldAttributes>
            {
                ["static"] = FieldAttributes.Static,
                ["readonly"] = FieldAttributes.InitOnly,
            };

            FieldModifiers = new Dictionary<string, FieldAttributes>
            {
                ["public"] = FieldAttributes.Public,
                ["internal"] = FieldAttributes.Assembly,
                ["private"] = FieldAttributes.Private,
                ["protected"] = FieldAttributes.Family,
            };
        }
    }
}