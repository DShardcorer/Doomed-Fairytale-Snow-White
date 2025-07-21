#if UNITY_EDITOR
using FlagSystem.FlagSystem;
using UnityEditor;
using UnityEngine;

namespace FlagSystem.Editor
{
    [CustomEditor(typeof(FlagReference))]
    public class FlagReferenceEditor : UnityEditor.Editor
    {
        SerializedProperty categoryProp;
        SerializedProperty flagProp;
        SerializedProperty descriptionProp;
        FlagType currentFlagType = FlagType.Boolean;

        private void OnEnable()
        {
            categoryProp = serializedObject.FindProperty("category");
            flagProp = serializedObject.FindProperty("flag");
            descriptionProp = serializedObject.FindProperty("description");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(categoryProp);
            
            FlagReference flagRef = target as FlagReference;
            GameFlag flag = flagRef.flag;
            
            // Flag Type selector
            EditorGUI.BeginChangeCheck();
            currentFlagType = (FlagType)EditorGUILayout.EnumPopup("Flag Type", 
                flag != null ? flag.GetFlagType() : currentFlagType);
                
            if (EditorGUI.EndChangeCheck() || flag == null)
            {
                // Create new flag of selected type
                GameFlag newFlag = null;
                string flagId = flag?.id ?? "";
                
                switch (currentFlagType)
                {
                    case FlagType.Boolean:
                        newFlag = new BoolGameFlag { id = flagId };
                        break;
                    case FlagType.Integer:
                        newFlag = new IntGameFlag { id = flagId };
                        break;
                    case FlagType.Float:
                        newFlag = new FloatGameFlag { id = flagId };
                        break;
                    case FlagType.String:
                        newFlag = new StringGameFlag { id = flagId };
                        break;
                }
                
                flagRef.flag = newFlag;
                EditorUtility.SetDirty(flagRef);
            }
            
            // Flag ID field
            EditorGUI.BeginChangeCheck();
            string newId = EditorGUILayout.TextField("Flag ID", flag?.id ?? "");
            if (EditorGUI.EndChangeCheck() && flag != null)
            {
                flag.id = newId;
                EditorUtility.SetDirty(flagRef);
            }
            
            // Value field based on type
            if (flag != null)
            {
                EditorGUI.BeginChangeCheck();
                
                if (flag is BoolGameFlag boolFlag)
                {
                    boolFlag.value = EditorGUILayout.Toggle("Default Value", boolFlag.value);
                }
                else if (flag is IntGameFlag intFlag)
                {
                    intFlag.value = EditorGUILayout.IntField("Default Value", intFlag.value);
                }
                else if (flag is FloatGameFlag floatFlag)
                {
                    floatFlag.value = EditorGUILayout.FloatField("Default Value", floatFlag.value);
                }
                else if (flag is StringGameFlag stringFlag)
                {
                    stringFlag.value = EditorGUILayout.TextField("Default Value", stringFlag.value);
                }
                
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(flagRef);
                }
            }
            
            EditorGUILayout.PropertyField(descriptionProp);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif