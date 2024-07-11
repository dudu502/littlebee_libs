using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Common.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class EcsrComponent : AbstractComponent
{
    public string Name;
    public override AbstractComponent Clone()
    {
        var result = new EcsrComponent();
        result.Name = Name;
        return result;
    }

    public override void CopyFrom(AbstractComponent component)
    {
        Name = ((EcsrComponent)component).Name;
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            Name = buffer.ReadString();
            return this;
        }
    }

    public override byte[] Serialize()
    {
        using(ByteBuffer buffer = new ByteBuffer()) 
        {
            buffer.WriteString(Name);
            return buffer.GetRawBytes();
        }
    }
}
public class TestComponent : AbstractComponent
{
    public int Value;
    public string Desc;
    public override AbstractComponent Clone()
    {
        var result = new TestComponent();
        result.Value = Value;
        result.Desc = Desc;
        return result;
    }

    public override void CopyFrom(AbstractComponent component)
    {
        Value = ((TestComponent)component).Value;
        Desc = ((TestComponent)component).Desc;
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            Value = buffer.ReadInt32();
            Desc = buffer.ReadString();
            return this;
        }
    }

    public override byte[] Serialize()
    {
        using (ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteInt32(Value);
            buffer.WriteString(Desc);
            return buffer.GetRawBytes();
        }
    }
}
public class ComponentPrototype : MonoBehaviour
{
    public string EntityId;
    public LinkedList<AbstractComponent> Components = new LinkedList<AbstractComponent>();
    private Entity _entity;
    void Start()
    {

    }

    

    public byte[] Write()
    {
        _entity = new Entity(Guid.NewGuid());
        foreach(var component in Components)
        {
            _entity.AddComponent(component);
        }
        return Entity.Write(_entity);
    }

    public Entity Read(byte[] bytes)
    {
        Components.Clear();
        _entity = Entity.Read(bytes);
        foreach(var component in _entity.Components.Values)
        {
            Components.AddLast(component);
        }
        EntityId = _entity.Id.ToString();
        return _entity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
