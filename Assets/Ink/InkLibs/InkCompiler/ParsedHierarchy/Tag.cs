
using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class Tag : Object
    {

        public bool isStart;
        public bool inChoice;
        
        public override InkRuntime.Object GenerateRuntimeObject ()
        {
            if( isStart )
                return ControlCommand.BeginTag();
            else
                return ControlCommand.EndTag();
        }

        public override string ToString ()
        {
            if( isStart )
                return "#StartTag";
            else
                return "#EndTag";
        }
    }
}

