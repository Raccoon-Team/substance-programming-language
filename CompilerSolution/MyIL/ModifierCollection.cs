using System;
using System.Collections.Generic;
using System.Reflection;
using CompilerUtilities.Exceptions;

namespace IL2MSIL
{
    internal class ModifierCollection
    {
        public static Dictionary<string, TypeAttributes> TypeAttributeses;
        public static Dictionary<string, TypeAttributes> TypeModifiers;

        public static Dictionary<string, MethodAttributes> MethodAttributeses;
        public static Dictionary<string, MethodAttributes> MethodModifiers;

        private static readonly Dictionary<string, FieldAttributes> FieldAttributeses;
        private static readonly Dictionary<string, FieldAttributes> FieldModifiers;

        public static FieldAttributes GetFieldAttributes(IList<string> modifiers)
        {
            var accessExists = false;

            FieldAttributes attributes = 0;
            
            for (var i = 0; i < modifiers.Count; i++)
            {
                if (FieldModifiers.ContainsKey(modifiers[i]))
                {
                    if (accessExists)
                        ExceptionManager.ThrowCompiler(ErrorCode.AccessModifierAlreadySet, String.Empty, -1);

                    attributes |= FieldModifiers[modifiers[i]];
                    accessExists = true;
                }
                else
                    attributes |= FieldAttributeses[modifiers[i]];
            }

            if (!accessExists)
                attributes |= FieldAttributes.Private;
            //    ExceptionManager.ThrowCompiler(ErrorCode.ModifierExpected, String.Empty);

            return attributes;
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