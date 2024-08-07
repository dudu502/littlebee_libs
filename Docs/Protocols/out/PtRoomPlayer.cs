//Creation time:2024/8/6 17:35:58
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtRoomPlayer
{
    public byte __tag__ { get;private set;}

	public uint EntityId{ get;private set;}
	public byte TeamId{ get;private set;}
	public byte Color{ get;private set;}
	public string Name{ get;private set;}
	public string UserId{ get;private set;}
	public byte Status{ get;private set;}
	   
    public PtRoomPlayer SetEntityId(uint value){EntityId=value; __tag__|=1; return this;}
	public PtRoomPlayer SetTeamId(byte value){TeamId=value; __tag__|=2; return this;}
	public PtRoomPlayer SetColor(byte value){Color=value; __tag__|=4; return this;}
	public PtRoomPlayer SetName(string value){Name=value; __tag__|=8; return this;}
	public PtRoomPlayer SetUserId(string value){UserId=value; __tag__|=16; return this;}
	public PtRoomPlayer SetStatus(byte value){Status=value; __tag__|=32; return this;}
	
    public bool HasEntityId(){return (__tag__&1)==1;}
	public bool HasTeamId(){return (__tag__&2)==2;}
	public bool HasColor(){return (__tag__&4)==4;}
	public bool HasName(){return (__tag__&8)==8;}
	public bool HasUserId(){return (__tag__&16)==16;}
	public bool HasStatus(){return (__tag__&32)==32;}
	
    public static byte[] Write(PtRoomPlayer data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasEntityId())buffer.WriteUInt32(data.EntityId);
			if(data.HasTeamId())buffer.WriteByte(data.TeamId);
			if(data.HasColor())buffer.WriteByte(data.Color);
			if(data.HasName())buffer.WriteString(data.Name);
			if(data.HasUserId())buffer.WriteString(data.UserId);
			if(data.HasStatus())buffer.WriteByte(data.Status);
			
            return buffer.GetRawBytes();
        }
    }

    public static PtRoomPlayer Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtRoomPlayer data = new PtRoomPlayer();
            data.__tag__ = buffer.ReadByte();
			if(data.HasEntityId())data.EntityId = buffer.ReadUInt32();
			if(data.HasTeamId())data.TeamId = buffer.ReadByte();
			if(data.HasColor())data.Color = buffer.ReadByte();
			if(data.HasName())data.Name = buffer.ReadString();
			if(data.HasUserId())data.UserId = buffer.ReadString();
			if(data.HasStatus())data.Status = buffer.ReadByte();
			
            return data;
        }       
    }
}
}
