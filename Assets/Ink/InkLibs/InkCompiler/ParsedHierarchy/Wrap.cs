
namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class Wrap<T> : Object where T : InkRuntime.Object
    {
        public Wrap (T objToWrap)
        {
            _objToWrap = objToWrap;
        }

        public override InkRuntime.Object GenerateRuntimeObject ()
        {
            return _objToWrap;
        }

        T _objToWrap;
    }

    // Shorthand for writing Parsed.Wrap<Runtime.Glue> and Parsed.Wrap<Runtime.Tag>
    public class Glue : Wrap<InkRuntime.Glue> {
        public Glue (InkRuntime.Glue glue) : base(glue) {}
    }
    public class LegacyTag : Wrap<InkRuntime.Tag> {
        public LegacyTag (InkRuntime.Tag tag) : base (tag) { }
    }
    
}

