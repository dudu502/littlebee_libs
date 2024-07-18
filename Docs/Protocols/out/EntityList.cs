//Creation time:2024/7/18 15:33:01
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class EntityList
{
    public byte __tag__ { get;private set;}

	public List<Entity> Elements{ get;private set;}
	   
    public EntityList SetElements(List<Entity> value){Elements=value; __tag__|=1; return this;}
	
    public bool HasElements(){return (__tag__&1)==1;}
	
    public static byte[] Write(EntityList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElements())buffer.WriteCollection(data.Elements,element=>Entity.Write(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static EntityList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            EntityList data = new EntityList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElements())data.Elements = buffer.ReadCollection(retbytes=>Entity.Read(retbytes));
			
            return data;
        }       
    }
}
}
