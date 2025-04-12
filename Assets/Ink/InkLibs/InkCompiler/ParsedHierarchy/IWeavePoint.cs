using System.Collections.Generic;
using Ink.InkLibs.InkRuntime;

namespace Ink.InkLibs.InkCompiler.ParsedHierarchy
{
    public interface IWeavePoint
    {
        int indentationDepth { get; }
        Container runtimeContainer { get; }
        List<Object> content { get; }
        string name { get; }
        Identifier identifier { get; }

    }
}

