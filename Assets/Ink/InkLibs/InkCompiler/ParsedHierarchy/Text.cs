
using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
	public class Text : Object
	{
		public string text { get; set; }

		public Text (string str)
		{
			text = str;
		}

		public override InkRuntime.Object GenerateRuntimeObject ()
		{
			return new StringValue(this.text);
		}

        public override string ToString ()
        {
            return this.text;
        }
	}
}

