using System;

namespace CompilerUtilities.PluginImporter
{
    internal class PluginGenericArgs
    {
        public Type TIn, TOut;

        public PluginGenericArgs(object plugin)
        {
            var type = plugin.GetType().FindInterfaces((t, criteria) => true, null)[0];
            var args = type.GenericTypeArguments;
            TIn = args[0];
            TOut = args[1];
        }
    }
}