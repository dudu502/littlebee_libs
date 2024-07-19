//Creation time:2024/7/19 17:43:52
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Client.Protocol.Pt;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtMap
{
    public byte __tag__ { get;private set;}

	public string Version{ get;private set;}
	public EntityList Entities{ get;private set;}
	   
    public PtMap SetVersion(string value){Version=value; __tag__|=1; return this;}
	public PtMap SetEntities(EntityList value){Entities=value; __tag__|=2; return this;}
	
    public bool HasVersion(){return (__tag__&1)==1;}
	public bool HasEntities(){return (__tag__&2)==2;}
	
    public static byte[] Write(PtMap data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasVersion())buffer.WriteString(data.Version);
			if(data.HasEntities())buffer.WriteBytes(EntityList.Write(data.Entities));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtMap Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtMap data = new PtMap();
            data.__tag__ = buffer.ReadByte();
			if(data.HasVersion())data.Version = buffer.ReadString();
			if(data.HasEntities())data.Entities = EntityList.Read(buffer.ReadBytes());
			
            return data;
        }       
    }
}
}
