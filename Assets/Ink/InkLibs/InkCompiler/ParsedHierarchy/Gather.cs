
using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class Gather : Object, IWeavePoint, INamedContent
    {
        public string name
        {
            get { return identifier?.name; }
        }
        public Identifier identifier { get; set; }
        public int indentationDepth { get; protected set; }

        public Container runtimeContainer { get { return (Container) runtimeObject; } }

        public Gather (Identifier identifier, int indentationDepth)
        {
            this.identifier = identifier;
            this.indentationDepth = indentationDepth;
        }

        public override InkRuntime.Object GenerateRuntimeObject ()
        {
            var container = new Container ();
            container.name = name;

            if (this.story.countAllVisits) {
                container.visitsShouldBeCounted = true;
            }

            container.countingAtStartOnly = true;

            // A gather can have null content, e.g. it's just purely a line with "-"
            if (content != null) {
                foreach (var c in content) {
                    container.AddContent (c.runtimeObject);
                }
            }

            return container;
        }

        public override void ResolveReferences (Story context)
        {
            base.ResolveReferences (context);

            if( identifier != null && identifier.name.Length > 0 )
                context.CheckForNamingCollisions (this, identifier, Story.SymbolType.SubFlowAndWeave);
        }
    }
}

