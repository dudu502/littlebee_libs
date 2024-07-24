using Engine.Client.Ecsr.Components;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;


public class TileAppearanceRenderer : AppearanceRenderer
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var result = World.ReadComponent<Appearance, Engine.Client.Ecsr.Components.Position, Tile>(EntityId);
        if (World.IsActive)
        {
            transform.GetComponent<Renderer>().material.color = new Color(result.Item1.ShaderR / 255f, result.Item1.ShaderG / 255f, result.Item1.ShaderB / 255f);
            transform.position = new Vector3(result.Item2.Pos.x.AsFloat(), 0, result.Item2.Pos.y.AsFloat());

            transform.localScale = new Vector3(result.Item3.Size.x.AsFloat(), 1, result.Item3.Size.y.AsFloat());
        }
    }
}
