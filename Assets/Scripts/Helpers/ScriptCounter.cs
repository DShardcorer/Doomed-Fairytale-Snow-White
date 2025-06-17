using System.IO;
using UnityEngine;

namespace Helpers
{
    public class ScriptCounter : MonoBehaviour
    {
        [ContextMenu("Count .cs Files in Scripts")]
        void CountScriptFiles()
        {
            string scriptsPath = Path.Combine(Application.dataPath, "Scripts");
            string[] files = Directory.GetFiles(scriptsPath, "*.cs", SearchOption.AllDirectories);
            Debug.Log("Number of .cs files in Assets/Scripts: " + files.Length);
        }
    }

}