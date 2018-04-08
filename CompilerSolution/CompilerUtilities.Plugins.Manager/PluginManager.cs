using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using CompilerUtilities.Notifications;
using CompilerUtilities.Notifications.Interfaces;
using CompilerUtilities.Notifications.Structs.Enums;
using CompilerUtilities.Plugins.EventArgs;
using CompilerUtilities.Plugins.Stages;

namespace CompilerUtilities.Plugins.Management
{
    public class PluginManager : IPluginManager
    {
        public EventHandler<FileReaderEventArgs> OnFileReadEnd { get; set; }
        public EventHandler<LexerEventArgs> OnTokenized { get; set; }
        public EventHandler<ParserEventArgs> OnParsed { get; set; }
        public EventHandler<GenIntermediateEventArgs> OnIntermediateCodeGenerated { get; set; }
        public EventHandler OnTranslated { get; set; }

        private INotifier _notifier;

        public void Notify(string message)
        {
            _notifier.Notify(NotifyLevel.Info, message);
        }

        [ImportMany(typeof(IPlugin))] private List<IPlugin> _plugins;

        [Import(typeof(IFileReader))] private IFileReader _fileReader;
        [Import(typeof(ILexer))] private ILexer _lexer;
        [Import(typeof(IParser))] private IParser _parser;
        [Import(typeof(IGeneratorIntermediate))] private IGeneratorIntermediate _generatorIntermediate;
        [Import(typeof(ITranslator))] private ITranslator _translator;

        public PluginManager(string componentPath)
        {
            _notifier = new ColoredConsoleNotifier(new FileNotifier("log.txt"));
            ComponentsActivate(componentPath);
        }

        private void ComponentsActivate(string componentPath)
        {
            try
            {
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new DirectoryCatalog(componentPath));

                var compositionContainer = new CompositionContainer(catalog);

                compositionContainer.ComposeParts(this);
            }
            catch (Exception e)
            {
                _notifier.Notify(NotifyLevel.Fatal, "Stage component not found");
                throw;
            }
            

            foreach (var plugin in _plugins.OrderByDescending(plug => plug.Priority))
            {
                plugin.Activate(this);
            }
        }

        public void Run(string inputPath, string outputPath)
        {
            try
            {
                var source = _fileReader.ReadFromFile(inputPath);
                OnFileReadEnd?.Invoke(this, new FileReaderEventArgs(source));
                var tokens = _lexer.Tokenize(source);
                OnTokenized?.Invoke(this, new LexerEventArgs(tokens));
                var tree = _parser.Parse(tokens);
                OnParsed?.Invoke(this, new ParserEventArgs(tree));
                var intermediate = _generatorIntermediate.Generate(tree);
                OnIntermediateCodeGenerated?.Invoke(this, new GenIntermediateEventArgs(intermediate));
                _translator.Translate(intermediate, outputPath);
                OnTranslated?.Invoke(this, new System.EventArgs());
            }
            catch (CompileException e)
            {
                _notifier.Notify(NotifyLevel.Error, e.Message);
            }
        }
    }
}
