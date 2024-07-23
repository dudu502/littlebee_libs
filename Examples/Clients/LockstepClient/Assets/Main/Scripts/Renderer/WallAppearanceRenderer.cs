using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAppearanceRenderer : AppearanceRenderer
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        World.ReadComponent<Appearance, Position, Engine.Client.Ecsr.Components.Wall>(EntityId, (appearance, position,rect) =>
        {
            //transform.GetComponent<Renderer>().material.color = new Color(appearance.ShaderR / 255f, appearance.ShaderG / 255f, appearance.ShaderB / 255f);
            transform.position = new Vector3(position.Pos.x.AsFloat(), 0, position.Pos.y.AsFloat());
            transform.localScale = new Vector3(rect.Width.AsFloat(),1,rect.Height.AsFloat());
        });
    }
}
