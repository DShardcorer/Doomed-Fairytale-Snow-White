using Ink.InkLibs.InkCompiler.ParsedHierarchy;

namespace Ink.InkLibs.InkCompiler.InkParser
{
    public partial class InkParser
    {
        protected AuthorWarning AuthorWarning()
        {
            Whitespace ();

            var identifier = Parse (IdentifierWithMetadata);
            if (identifier == null || identifier.name != "TODO")
                return null;

            Whitespace ();

            ParseString (":");

            Whitespace ();

            var message = ParseUntilCharactersFromString ("\n\r");

            return new AuthorWarning (message);
        }

    }
}

