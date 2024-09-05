using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAppearanceRenderer : AppearanceRenderer
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EntityId != Guid.Empty && World != null)
        {
            var result = World.ReadComponent<Appearance, Engine.Client.Ecsr.Components.Position>(EntityId);
            if (World.IsActive)
            {
                transform.GetComponent<Renderer>().material.color = new Color(result.Item1.ShaderR / 255f, result.Item1.ShaderG / 255f, result.Item1.ShaderB / 255f);
                transform.position = Vector3.Lerp(transform.position, new Vector3(result.Item2.Pos.x.AsFloat(), 0, result.Item2.Pos.y.AsFloat()),0.25f);
            }
        }
    }
}
