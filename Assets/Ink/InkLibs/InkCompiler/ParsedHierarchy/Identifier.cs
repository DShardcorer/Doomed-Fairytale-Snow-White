using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy {
    public class Identifier {
        public string name;
        public DebugMetadata debugMetadata;

        public override string ToString()
        {
            return name;
        }

        public static Identifier Done = new Identifier { name = "DONE", debugMetadata = null };
    }
}
