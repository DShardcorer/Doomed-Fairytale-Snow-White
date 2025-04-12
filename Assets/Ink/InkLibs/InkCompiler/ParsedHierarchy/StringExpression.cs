using System.Collections.Generic;
using System.Text;
using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class StringExpression : Expression
    {
        public bool isSingleString {
            get {
                if (content.Count != 1)
                    return false;

                var c = content [0];
                if (!(c is Text))
                    return false;

                return true;
            }
        }

        public StringExpression (List<Object> content)
        {
            AddContent (content);
        }

        public override void GenerateIntoContainer (Container container)
        {
            container.AddContent (ControlCommand.BeginString());

            foreach (var c in content) {
                container.AddContent (c.runtimeObject);
            }
                
            container.AddContent (ControlCommand.EndString());
        }

        public override string ToString ()
        {
            var sb = new StringBuilder ();
            foreach (var c in content) {
                sb.Append (c.ToString ());
            }
            return sb.ToString ();
        }

        // Equals override necessary in order to check for CONST multiple definition equality
        public override bool Equals (object obj)
        {
            var otherStr = obj as StringExpression;
            if (otherStr == null) return false;

            // Can only compare direct equality on single strings rather than
            // complex string expressions that contain dynamic logic
            if (!this.isSingleString || !otherStr.isSingleString) {
                return false;
            }

            var thisTxt = this.ToString ();
            var otherTxt = otherStr.ToString ();
            return thisTxt.Equals (otherTxt);
        }

        public override int GetHashCode ()
        {
            return this.ToString ().GetHashCode ();
        }
    }
}

