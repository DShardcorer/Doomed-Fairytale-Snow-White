
namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public class AuthorWarning : Object
    {
        public string warningMessage;

        public AuthorWarning(string message)
        {
            warningMessage = message;
        }

        public override InkRuntime.Object GenerateRuntimeObject ()
        {
            Warning (warningMessage);
            return null;
        }
    }
}

