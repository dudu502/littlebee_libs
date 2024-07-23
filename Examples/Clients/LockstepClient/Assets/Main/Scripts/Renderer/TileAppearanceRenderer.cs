using Engine.Client.Ecsr.Components;
using System.Collections;
using System.Collections.Generic;
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
        World.ReadComponent<Appearance, Position, Tile>(EntityId, (appearance, position, tile) =>
        {
            //transform.GetComponent<Renderer>().material.color = new Color(appearance.ShaderR / 255f, appearance.ShaderG / 255f, appearance.ShaderB / 255f);
            transform.position = new Vector3(position.Pos.x.AsFloat(), 0, position.Pos.y.AsFloat());
            Debug.LogWarning("Scale:"+ tile.Size.x+" "+ tile.Size.y);
            transform.localScale = new Vector3(tile.Size.x.AsFloat(), 1, tile.Size.y.AsFloat());
        });
    }
}
