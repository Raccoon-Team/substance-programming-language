using System;
using System.Collections.Generic;
using CompilerUtilities.Plugins.Contract;

namespace CompilerUtilities.PluginImporter
{
    internal class PluginGenericArgs : IEquatable<PluginGenericArgs>
    {
        public Type TIn, TOut;

        public PluginGenericArgs(object plugin)
        {
            var args = GetGenericArgs(plugin);
            TIn = args[0];
            TOut = args[1];
        }

        public bool Equals(PluginGenericArgs other)
        {
            return other != null &&
                   EqualityComparer<Type>.Default.Equals(TIn, other.TIn) &&
                   EqualityComparer<Type>.Default.Equals(TOut, other.TOut);
        }

        private static Type[] GetGenericArgs(object plugin)
        {
            var type = plugin.GetType()
                .FindInterfaces((t, criteria) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IStage<,>),
                    null)[0];
            var args = type.GenericTypeArguments;
            return args;
        }

        public static Type[] GetArgs(object extension)
        {
            var args = extension.GetType()
                .FindInterfaces(
                    (type, criteria) => type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IStage<,>) ||
                                        type.GetGenericTypeDefinition() == typeof(IPlugin<>)), null)[0]
                .GenericTypeArguments;

            return args;
        }

        public static Type GetFirstArg(object extension)
        {
            var args = GetGenericArgs(extension);
            return args[0];
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PluginGenericArgs);
        }

        public override int GetHashCode()
        {
            var hashCode = 1613930117;
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(TIn);
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(TOut);
            return hashCode;
        }

        public static bool operator ==(PluginGenericArgs first, PluginGenericArgs second)
        {
            return first?.TIn == second?.TIn && first?.TOut == second?.TOut;
        }

        public static bool operator !=(PluginGenericArgs first, PluginGenericArgs second)
        {
            return !(first == second);
        }
    }
}