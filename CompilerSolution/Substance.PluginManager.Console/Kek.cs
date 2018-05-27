using System;
using CompilerUtilities.Plugins.Contract;

namespace Substance.PluginManager.Console
{
    [RequiredCompilerVersion("12.5a")]
    internal class Kek : IPlugin<int>, IPlugin<bool>
    {
        private uint _priority;

        uint IPlugin<bool>.Priority
        {
            get { return _priority; }
        }

        bool IPlugin<bool>.Activate(bool input)
        {
            throw new NotImplementedException();
        }

        uint IPlugin<int>.Priority
        {
            get { return _priority; }
        }

        public int Activate(int input)
        {
            return 0;
        }

        public void Initialize(IFileBuffer fileBuffer)
        {
            throw new NotImplementedException();
        }
    }
}