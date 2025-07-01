using SceneSwitch;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SceneSwitchPortal))]
    public class SceneSwitchPortalEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            SerializedProperty isPortalToOverworld = serializedObject.FindProperty("_isPortalToOverworld");
            
            EditorGUILayout.PropertyField(isPortalToOverworld);
            
            // Only show these fields if NOT a portal to overworld
            if (!isPortalToOverworld.boolValue)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Spawn TO", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_portalToSpawnTo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_sceneToLoad"));
            }
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("THIS PORTAL", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_currentPortal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_playerSpawnOffset"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}