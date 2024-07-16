using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Systems;
using Engine.Common.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;
public class FightSystem : IEntitySystem
{
    public EntityWorld World { get; set; }

    public void Execute()
    {

    }
}
public class MoveSystem : IEntitySystem
{
    public EntityWorld World { get; set; }

    public void Execute()
    {
        
    }
}
public class MapPrototype : MonoBehaviour
{
    public LinkedList<Type> Systems = new LinkedList<Type>(); 
    private void Start()
    {
        
    }

    public byte[] Write()
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteInt32(Systems.Count);
            foreach(Type type in Systems)
            {
                buffer.WriteString(type.AssemblyQualifiedName);
            }
            //write all componentPrototype
            var componentProtos = GameObject.FindObjectsOfType<ComponentPrototype>();
            buffer.WriteInt32(componentProtos.Length);
            foreach(ComponentPrototype componentProto in componentProtos)
            {
                buffer.WriteBytes(componentProto.Write());
            }
            return buffer.GetRawBytes();
        }
    }
    public void Read(byte[] bytes)
    {
        Systems.Clear();
        using (ByteBuffer buffer = new ByteBuffer(bytes))
        {
            var sysCount = buffer.ReadInt32();
            for (int i = 0; i < sysCount; ++i)
            {
                Systems.AddLast(Type.GetType(buffer.ReadString()));
            }
            int componentProtoCount = buffer.ReadInt32();
            for (int i = 0; i < componentProtoCount; ++i)
            {
                //buffer.ReadBytes()

            }
            //read all componentPrototye

        }
    }
}

