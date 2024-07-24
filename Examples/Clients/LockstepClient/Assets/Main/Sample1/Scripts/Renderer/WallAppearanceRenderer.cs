using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WallAppearanceRenderer : AppearanceRenderer
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var result = World.ReadComponent<Appearance, Engine.Client.Ecsr.Components.Position, Wall>(EntityId);
        transform.position = new Vector3(result.Item2.Pos.x.AsFloat(),0,result.Item2.Pos.y.AsFloat());
        transform.localScale = new Vector3(result.Item3.Width.AsFloat(),1,result.Item3.Height.AsFloat());
    }
}
