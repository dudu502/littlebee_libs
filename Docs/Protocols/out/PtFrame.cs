//Creation time:2024/8/9 17:51:54
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtFrame
{
    private byte __tag__;

	public string EntityId{ get;private set;}
	public PtComponentUpdaterList Updaters{ get;private set;}
	public byte[] NewEntitiesRaw{ get;private set;}
	   
    public PtFrame SetEntityId(string value){EntityId=value; __tag__|=1; return this;}
	public PtFrame SetUpdaters(PtComponentUpdaterList value){Updaters=value; __tag__|=2; return this;}
	public PtFrame SetNewEntitiesRaw(byte[] value){NewEntitiesRaw=value; __tag__|=4; return this;}
	
    public bool HasEntityId(){return (__tag__&1)==1;}
	public bool HasUpdaters(){return (__tag__&2)==2;}
	public bool HasNewEntitiesRaw(){return (__tag__&4)==4;}
	
    public static byte[] Write(PtFrame data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasEntityId())buffer.WriteString(data.EntityId);
			if(data.HasUpdaters())buffer.WriteBytes(PtComponentUpdaterList.Write(data.Updaters));
			if(data.HasNewEntitiesRaw())buffer.WriteBytes(data.NewEntitiesRaw);
			
            return buffer.GetRawBytes();
        }
    }

    public static PtFrame Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtFrame data = new PtFrame();
            data.__tag__ = buffer.ReadByte();
			if(data.HasEntityId())data.EntityId = buffer.ReadString();
			if(data.HasUpdaters())data.Updaters = PtComponentUpdaterList.Read(buffer.ReadBytes());
			if(data.HasNewEntitiesRaw())data.NewEntitiesRaw = buffer.ReadBytes();
			
            return data;
        }       
    }
}
}
