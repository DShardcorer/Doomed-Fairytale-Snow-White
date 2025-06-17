using UnityEditor;
using UnityEngine;
using System.IO;

namespace Editor
{
    public class LineCounter : EditorWindow
    {
        [MenuItem("Tools/Count Lines")]
        public static void ShowWindow()
        {
            GetWindow<LineCounter>("Line Counter");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Count Lines"))
            {
                CountLines();
            }
        }

        private void CountLines()
        {
            string[] csFiles = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            int totalLines = 0;
            foreach (string file in csFiles)
            {
                totalLines += File.ReadAllLines(file).Length;
            }

            Debug.Log("Total lines of code: " + totalLines);
        }
    }
}