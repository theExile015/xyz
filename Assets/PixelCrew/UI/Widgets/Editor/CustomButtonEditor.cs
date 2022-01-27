using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomButton), true)]
    public class CustomButtonEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"));
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}