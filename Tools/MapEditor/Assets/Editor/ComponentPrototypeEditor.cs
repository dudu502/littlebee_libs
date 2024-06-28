using Engine.Client.Ecsr.Components;
using System;
using System.Collections;
using System.Collections.Generic;
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
        ComponentPrototype componentPrototype = (ComponentPrototype)target;
        GUILayout.BeginVertical();
        foreach(var component in componentPrototype.Components)
        {
            GUILayout.Box("ECS-Component: "+component.GetType().ToString());
            var fields = component.GetType().GetFields();
            foreach (var field in fields)
            {
                GUILayout.BeginHorizontal();
                if (field.FieldType == typeof(int))
                {
                    field.SetValue(component, EditorGUILayout.IntField(field.Name, (int)field.GetValue(component)));
                }
                else if (field.FieldType == typeof(float))
                {
                    field.SetValue(component, EditorGUILayout.FloatField(field.Name, (float)field.GetValue(component)));
                }
                else if (field.FieldType == typeof(uint))
                {
                    field.SetValue(component, (uint)EditorGUILayout.IntField(field.Name, Convert.ToInt32(field.GetValue(component))));
                }
                else if (field.FieldType == typeof(string))
                {
                    field.SetValue(component, EditorGUILayout.TextField(field.Name, (string)field.GetValue(component)));
                }
                GUILayout.EndHorizontal();
                // Add more else if clauses for other types you may have in AbstractComponent

                // Add more GUI controls for other field types if needed
            }
            
            GUILayout.Space(10);
        }

        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
      
        selectedOptionIndex = EditorGUILayout.Popup("Component Type", selectedOptionIndex, ComponentTypes.ConvertAll<string>(e=>e.ToString()).ToArray());

        if (GUILayout.Button("Add Component"))
        {
            Type componentType = ComponentTypes[selectedOptionIndex];
            componentPrototype.Components.Add( Activator.CreateInstance(componentType) as AbstractComponent);
        }
        GUILayout.EndHorizontal();
    }
}
