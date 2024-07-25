using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using System;
using UnityEngine;

public class AppearanceRenderer : MonoBehaviour, IEntityRender
{
    public Guid EntityId { get; set; }
    public EntityWorld World { set; get; }
}

