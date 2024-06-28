using Engine.Client.Ecsr.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EcsrComponent : AbstractComponent
{
    public string f;
    public override AbstractComponent Clone()
    {
        throw new NotImplementedException();
    }

    public override void CopyFrom(AbstractComponent component)
    {
        throw new NotImplementedException();
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        throw new NotImplementedException();
    }

    public override byte[] Serialize()
    {
        throw new NotImplementedException();
    }
}
public class TestComponent : AbstractComponent
{
    public int value;
    public string desc;
    public override AbstractComponent Clone()
    {
        return this;
    }

    public override void CopyFrom(AbstractComponent component)
    {
       
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        return this;
    }

    public override byte[] Serialize()
    {
        return null;
    }
}
public class ComponentPrototype : MonoBehaviour
{
    public List<AbstractComponent> Components = new List<AbstractComponent>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
