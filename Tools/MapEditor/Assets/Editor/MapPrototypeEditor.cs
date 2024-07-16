using Engine.Client.Ecsr.Systems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapPrototype))]
public class MapPrototypeEditor : Editor
{
    int selectedOptionIndex = 0;
    bool showComponentDetails = false;
    List<Type> SystemTypes = new List<Type>();
    private void OnEnable()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                bool isEntitySystem = false;
                var interfaces = type.GetInterfaces();
                foreach (var i in interfaces)
                {
                    if (i == typeof(IEntitySystem))
                    {
                        isEntitySystem = true;
                        break;
                    }
                }
                if (isEntitySystem)
                {
                    SystemTypes.Add(type);
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapPrototype mapPrototype = (MapPrototype)target;
        var componentProtos = GameObject.FindObjectsOfType<ComponentPrototype>();
        GUILayout.Label("ComponentPrototype Count:" + componentProtos.Length);
        showComponentDetails = EditorGUILayout.Foldout(showComponentDetails, "Show Details");
        if (showComponentDetails)
        {
            foreach (var proto in componentProtos)
            {
                EditorGUILayout.ObjectField(proto.name, proto.gameObject, typeof(ComponentPrototype), true);
            }
        }
        GUILayout.Space(10);
        GUILayout.BeginVertical();
        var current = mapPrototype.Systems.First;
        while (current != null)
        {
            var next = current.Next;
            GUILayout.BeginHorizontal();
            GUILayout.Label("ECS-System:" + current.Value.ToString());
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
                mapPrototype.Systems.Remove(current);
            GUILayout.EndHorizontal();
            current = next;
            GUILayout.Space(10);
        }
        GUILayout.EndVertical();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        selectedOptionIndex = EditorGUILayout.Popup("System Type", selectedOptionIndex, SystemTypes.ConvertAll<string>(e => e.ToString()).ToArray());
        if (GUILayout.Button("Add Component"))
        {
            Type systemType = SystemTypes[selectedOptionIndex];
            mapPrototype.Systems.AddLast(systemType);
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Save"))
        {

        }
    }
}
