using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using System;
using UnityEngine;

public class PlayerAppearanceRenderer : MonoBehaviour, IEntityRender
{
    public Guid EntityId { get; set; }
    public EntityWorld World { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EntityId != Guid.Empty && World != null)
        {
            World.ReadComponent<Appearance,Position>(EntityId, (appearance,position)=>
            {
                transform.GetComponent<Renderer>().material.color = new Color(appearance.ShaderR/255f,appearance.ShaderG/255f,appearance.ShaderB/255f);
                transform.position = new Vector3(position.Pos.x.AsFloat(),0,position.Pos.y.AsFloat());
            });
        }
    }
}
