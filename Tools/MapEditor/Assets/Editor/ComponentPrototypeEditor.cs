using Engine.Client.Ecsr.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(ComponentPrototype))]
public class ComponentPrototypeEditor : Editor
{
    List<Type> ComponentTypes = new List<Type>();
    int selectedOptionIndex = 0;
    private void OnEnable()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if(type.BaseType == typeof(AbstractComponent))
                {
                    ComponentTypes.Add(type);
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 
        GUILayout.Space(10);
        ComponentPrototype componentPrototype = (ComponentPrototype)target;
        GUILayout.BeginVertical();

        var current = componentPrototype.Components.First;
        while (current != null)
        {
            var next = current.Next;
            GUILayout.BeginHorizontal();
            GUILayout.Label("ECS-Component: " + current.Value.GetType().ToString());
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                componentPrototype.Components.Remove(current);
            }

            GUILayout.EndHorizontal();

            var fields = current.Value.GetType().GetFields();
            foreach (var field in fields)
            {
                GUILayout.BeginHorizontal();
                if (field.FieldType == typeof(int))
                {
                    field.SetValue(current.Value, EditorGUILayout.IntField(field.Name, (int)field.GetValue(current.Value)));
                }
                else if (field.FieldType == typeof(float))
                {
                    field.SetValue(current.Value, EditorGUILayout.FloatField(field.Name, (float)field.GetValue(current.Value)));
                }
                else if (field.FieldType == typeof(uint))
                {
                    field.SetValue(current.Value, (uint)EditorGUILayout.IntField(field.Name, Convert.ToInt32(field.GetValue(current.Value))));
                }
                else if (field.FieldType == typeof(string))
                {
                    field.SetValue(current.Value, EditorGUILayout.TextField(field.Name, (string)field.GetValue(current.Value)));
                }
                GUILayout.EndHorizontal();
            }
            current = next;
            GUILayout.Space(10);
        }
        GUILayout.EndVertical();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();    
        selectedOptionIndex = EditorGUILayout.Popup("Component Type", selectedOptionIndex, ComponentTypes.ConvertAll<string>(e=>e.ToString()).ToArray());
        if (GUILayout.Button("Add Component"))
        {
            Type componentType = ComponentTypes[selectedOptionIndex];
            componentPrototype.Components.AddLast( Activator.CreateInstance(componentType) as AbstractComponent);
        }
        GUILayout.EndHorizontal();
    }
}
