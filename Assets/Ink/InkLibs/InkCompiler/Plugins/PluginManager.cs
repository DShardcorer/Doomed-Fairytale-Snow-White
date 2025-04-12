using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Ink.InkLibs.InkCompiler.ParsedHierarchy;
using Path = System.IO.Path;

namespace Ink.InkLibs.InkCompiler.Plugins
{
    public class PluginManager
    {
        public PluginManager (List<string> pluginDirectories)
        {
            _plugins = new List<IPlugin> ();

            foreach (string pluginName in pluginDirectories) 
            {
                foreach (string file in Directory.GetFiles(pluginName, "*.dll"))
                {
                    foreach (Type type in Assembly.LoadFile(Path.GetFullPath(file)).GetExportedTypes())
                    {
                        if (typeof(IPlugin).IsAssignableFrom(type))
                        {
                            _plugins.Add((IPlugin)Activator.CreateInstance(type));
                        }
                    }
                }
            }
        }

		public string PreParse(string storyContent)
		{
			object[] args = new object[] { storyContent };

            foreach (var plugin in _plugins) 
            {
                typeof(IPlugin).InvokeMember("PreParse", BindingFlags.InvokeMethod, null, plugin, args);
            }

			return (string)args[0];
		}

        public Story PostParse(Story parsedStory)
        {
            object[] args = new object[] { parsedStory };

            foreach (var plugin in _plugins) 
            {
                typeof(IPlugin).InvokeMember("PostParse", BindingFlags.InvokeMethod, null, plugin, args);
            }

			return (Story)args[0];
        }

        public InkRuntime.Story PostExport(Story parsedStory, InkRuntime.Story runtimeStory)
        {
            object[] args = new object[] { parsedStory, runtimeStory };

            foreach (var plugin in _plugins) 
            {
                typeof(IPlugin).InvokeMember("PostExport", BindingFlags.InvokeMethod, null, plugin, args);
            }

			return (InkRuntime.Story)args[1];
        }

        List<IPlugin> _plugins;
    }
}

