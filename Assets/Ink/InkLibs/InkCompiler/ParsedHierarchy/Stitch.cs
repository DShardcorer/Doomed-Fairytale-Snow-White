using System.Collections.Generic;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
	public class Stitch : FlowBase
	{
        public override FlowLevel flowLevel { get { return FlowLevel.Stitch; } }

        public Stitch (Identifier name, List<Object> topLevelObjects, List<Argument> arguments, bool isFunction) : base(name, topLevelObjects, arguments, isFunction)
		{
		}
	}
}

