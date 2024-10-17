using System;
using System.Linq;
using MVVM;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeReference))]
public class TypeReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeNameProp = property.FindPropertyRelative("_typeName");

        // Display the script field
        MonoScript script = EditorGUI.ObjectField(position, label, GetScript(typeNameProp.stringValue), typeof(MonoScript), false) as MonoScript;

        if (script != null)
        {
            Type scriptType = script.GetClass();
            if (scriptType != null)
            {
                typeNameProp.stringValue = scriptType.AssemblyQualifiedName;
            }
        }
        else
        {
            typeNameProp.stringValue = string.Empty;
        }

        EditorGUI.EndProperty();
    }

    private MonoScript GetScript(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            return null;

        Type type = Type.GetType(typeName);
        if (type == null)
            return null;

        return AssetDatabase.FindAssets($"t:script {type.Name}")
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<MonoScript>(path))
            .FirstOrDefault(script => script.GetClass() == type);
    }
}