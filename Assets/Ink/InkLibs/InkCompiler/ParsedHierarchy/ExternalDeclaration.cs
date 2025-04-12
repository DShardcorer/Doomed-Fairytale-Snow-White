using System.Collections.Generic;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class ExternalDeclaration : Object, INamedContent
    {
        public string name
        {
            get { return identifier?.name; }
        }
        public Identifier identifier { get; set; }
        public List<string> argumentNames { get; set; }

        public ExternalDeclaration (Identifier identifier, List<string> argumentNames)
        {
            this.identifier = identifier;
            this.argumentNames = argumentNames;
        }

        public override InkRuntime.Object GenerateRuntimeObject ()
        {
            story.AddExternal (this);

            // No runtime code exists for an external, only metadata
            return null;
        }
    }
}

