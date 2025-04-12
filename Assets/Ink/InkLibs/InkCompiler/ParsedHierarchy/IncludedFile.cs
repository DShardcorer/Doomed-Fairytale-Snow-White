
namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class IncludedFile : Object
    {
        public Story includedStory { get; private set; }

        public IncludedFile (Story includedStory)
        {
            this.includedStory = includedStory;
        }

        public override InkRuntime.Object GenerateRuntimeObject ()
        {
            // Left to the main story to process
            return null;
        }
    }
}

