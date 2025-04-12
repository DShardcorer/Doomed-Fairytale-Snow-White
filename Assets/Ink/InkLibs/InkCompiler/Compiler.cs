using System.Collections.Generic;
using Ink.InkLibs.InkCompiler.ParsedHierarchy;
using Ink.InkLibs.InkCompiler.Plugins;
using Ink.InkLibs.InkRuntime;
using Divert = Ink.InkLibs.InkCompiler.ParsedHierarchy.Divert;
using Object = Ink.InkLibs.InkCompiler.ParsedHierarchy.Object;
using Path = Ink.InkLibs.InkRuntime.Path;
using Story = Ink.InkLibs.InkCompiler.ParsedHierarchy.Story;
using VariableAssignment = Ink.InkLibs.InkCompiler.ParsedHierarchy.VariableAssignment;

namespace Ink.InkLibs.InkCompiler
{
    public class Compiler
    {
        public class Options
        {
            public string sourceFilename;
            public List<string> pluginDirectories;
            public bool countAllVisits;
            public ErrorHandler errorHandler;
            public IFileHandler fileHandler;
        }

        public Story parsedStory {
            get {
                return _parsedStory;
            }
        }

        public Compiler (string inkSource, Options options = null)
        {
            _inputString = inkSource;
            _options = options ?? new Options();
            if( _options.pluginDirectories != null )
                _pluginManager = new PluginManager (_options.pluginDirectories);
        }

        public Story Parse()
        {
            _parser = new InkParser.InkParser(_inputString, _options.sourceFilename, OnParseError, _options.fileHandler);
            _parsedStory = _parser.Parse();
            return _parsedStory;
        }

        public InkRuntime.Story Compile ()
        {
            if( _pluginManager != null )
                _inputString = _pluginManager.PreParse(_inputString);

            Parse();

            if( _pluginManager != null )
                _parsedStory = _pluginManager.PostParse(_parsedStory);

            if (_parsedStory != null && !_hadParseError) {

                _parsedStory.countAllVisits = _options.countAllVisits;

                _runtimeStory = _parsedStory.ExportRuntime (_options.errorHandler);

                if( _pluginManager != null )
                    _runtimeStory = _pluginManager.PostExport (_parsedStory, _runtimeStory);
            } else {
                _runtimeStory = null;
            }

            return _runtimeStory;
        }

        public class CommandLineInputResult {
            public bool requestsExit;
            public int choiceIdx = -1;
            public string divertedPath;
            public string output;
        }
        public CommandLineInputResult HandleInput (CommandLineInput inputResult)
        {
            var result = new CommandLineInputResult ();

            // Request for debug source line number
            if (inputResult.debugSource != null) {
                var offset = (int)inputResult.debugSource;
                var dm = DebugMetadataForContentAtOffset (offset);
                if (dm != null)
                    result.output = "DebugSource: " + dm.ToString ();
                else
                    result.output = "DebugSource: Unknown source";
            }

            // Request for runtime path lookup (to line number)
            else if (inputResult.debugPathLookup != null) {
                var pathStr = inputResult.debugPathLookup;
                var contentResult = _runtimeStory.ContentAtPath (new Path (pathStr));
                var dm = contentResult.obj.debugMetadata;
                if( dm != null )
                    result.output = "DebugSource: " + dm.ToString ();
                else
                    result.output = "DebugSource: Unknown source";
            }

            // User entered some ink
            else if (inputResult.userImmediateModeStatement != null) {
                var parsedObj = inputResult.userImmediateModeStatement as Object;
                return ExecuteImmediateStatement(parsedObj);

            } else {
              return null;
            }

            return result;
        }

        CommandLineInputResult ExecuteImmediateStatement(Object parsedObj) {
            var result = new CommandLineInputResult ();

           // Variable assignment: create in Parsed.Story as well as the Runtime.Story
           // so that we don't get an error message during reference resolution
           if (parsedObj is VariableAssignment) {
               var varAssign = (VariableAssignment)parsedObj;
               if (varAssign.isNewTemporaryDeclaration) {
                   _parsedStory.TryAddNewVariableDeclaration (varAssign);
               }
           }

           parsedObj.parent = _parsedStory;
           var runtimeObj = parsedObj.runtimeObject;

           parsedObj.ResolveReferences (_parsedStory);

           if (!_parsedStory.hadError) {

               // Divert
               if (parsedObj is Divert) {
                   var parsedDivert = parsedObj as Divert;
                   result.divertedPath = parsedDivert.runtimeDivert.targetPath.ToString();
               }

               // Expression or variable assignment
               else if (parsedObj is Expression || parsedObj is VariableAssignment) {
                   var evalResult = _runtimeStory.EvaluateExpression ((Container)runtimeObj);
                   if (evalResult != null) {
                       result.output = evalResult.ToString ();
                   }
               }
           } else {
               _parsedStory.ResetError ();
           }

          return result;
        }

        public void RetrieveDebugSourceForLatestContent ()
        {
            foreach (var outputObj in _runtimeStory.state.outputStream) {
                var textContent = outputObj as StringValue;
                if (textContent != null) {
                    var range = new DebugSourceRange ();
                    range.length = textContent.value.Length;
                    range.debugMetadata = textContent.debugMetadata;
                    range.text = textContent.value;
                    _debugSourceRanges.Add (range);
                }
            }
        }

        DebugMetadata DebugMetadataForContentAtOffset (int offset)
        {
            int currOffset = 0;

            DebugMetadata lastValidMetadata = null;
            foreach (var range in _debugSourceRanges) {
                if (range.debugMetadata != null)
                    lastValidMetadata = range.debugMetadata;

                if (offset >= currOffset && offset < currOffset + range.length)
                    return lastValidMetadata;

                currOffset += range.length;
            }

            return null;
        }

        public struct DebugSourceRange
        {
            public int length;
            public DebugMetadata debugMetadata;
            public string text;
        }

        // Need to wrap the error handler so that we know
        // when there was a critical error between parse and codegen stages
        void OnParseError (string message, ErrorType errorType)
        {
            if( errorType == ErrorType.Error )
                _hadParseError = true;
            
            if (_options.errorHandler != null)
                _options.errorHandler (message, errorType);
            else
                throw new System.Exception(message);
        }

        string _inputString;
        Options _options;


        InkParser.InkParser _parser;
        Story _parsedStory;
        InkRuntime.Story _runtimeStory;

        PluginManager _pluginManager;

        bool _hadParseError;

        List<DebugSourceRange> _debugSourceRanges = new List<DebugSourceRange> ();
    }
}
