using Engine.Client.Ecsr.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableAppearanceRenderer : AppearanceRenderer
{
    // Start is called before the first frame update
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
                transform.position = new Vector3(result.Item2.Pos.x.AsFloat(), 0, result.Item2.Pos.y.AsFloat());
            }
        }
    }
}
