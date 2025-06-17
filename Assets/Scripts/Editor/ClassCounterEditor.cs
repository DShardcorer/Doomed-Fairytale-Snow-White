using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class ClassCounterEditor
{
    [MenuItem("Tools/Count Classes")]
    public static void CountClasses()
    {
        string[] scriptPaths = AssetDatabase.FindAssets("t:Script");
        int classCount = 0;

        foreach (string scriptPathGUID in scriptPaths)
        {
            string scriptPath = AssetDatabase.GUIDToAssetPath(scriptPathGUID);
            string scriptContent = File.ReadAllText(scriptPath);
            classCount += Regex.Matches(scriptContent, @"\bclass\s+\w+").Count;
        }

        Debug.Log("Total classes in project: " + classCount);
    }
}