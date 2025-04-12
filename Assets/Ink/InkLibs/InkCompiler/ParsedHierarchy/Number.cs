
using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
	public class Number : Expression
	{
		public object value;
		
		public Number(object value)
		{
            if (value is int || value is float || value is bool) {
                this.value = value;
            } else {
                throw new System.Exception ("Unexpected object type in Number");
            }
		}

        public override void GenerateIntoContainer (Container container)
		{
            if (value is int) {
                container.AddContent (new IntValue ((int)value));
            } else if (value is float) {
                container.AddContent (new FloatValue ((float)value));
            } else if(value is bool) {
                container.AddContent (new BoolValue ((bool)value));
            }
		}

        public override string ToString ()
        {
            if (value is float) {
                return ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            } else {
                return value.ToString();
            }
        }

        // Equals override necessary in order to check for CONST multiple definition equality
        public override bool Equals (object obj)
        {
            var otherNum = obj as Number;
            if (otherNum == null) return false;

            return this.value.Equals (otherNum.value);
        }

        public override int GetHashCode ()
        {
            return this.value.GetHashCode ();
        }
         
	}
}

