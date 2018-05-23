using System;
using CompilerUtilities.Plugins.Contract;

namespace Substance.PluginManager.Console
{
    [RequiredCompilerVersion("12.5a")]
    internal class Kek : IPlugin<int>, IPlugin<bool>
    {
        bool IPlugin<bool>.Activate(bool input)
        {
            throw new NotImplementedException();
        }

        public int Activate(int input)
        {
            return 0;
        }
    }
}