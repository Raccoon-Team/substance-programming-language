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
            var type = plugin.GetType().FindInterfaces((t, criteria) => t.GetGenericTypeDefinition() == typeof(IStage<,>), null)[0];
            var args = type.GenericTypeArguments;
            TIn = args[0];
            TOut = args[1];
        }

        public static (Type TIn, Type TOut) GetArgs(object extension)
        {
            var type = extension.GetType().FindInterfaces((t, criteria) => t.GetGenericTypeDefinition() == typeof(IStage<,>), null)[0];
            var args = type.GenericTypeArguments;
            return (args[0], args[1]);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PluginGenericArgs);
        }

        public bool Equals(PluginGenericArgs other)
        {
            return other != null &&
                   EqualityComparer<Type>.Default.Equals(TIn, other.TIn) &&
                   EqualityComparer<Type>.Default.Equals(TOut, other.TOut);
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