//Creation time:2024/8/9 17:51:54
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtLaunchData
{
    private byte __tag__;

	public string RoomServerAddr{ get;private set;}
	public ushort RoomServerPort{ get;private set;}
	public uint MapId{ get;private set;}
	public string ConnectionKey{ get;private set;}
	public bool IsStandaloneMode{ get;private set;}
	public byte PlayerNumber{ get;private set;}
	   
    public PtLaunchData SetRoomServerAddr(string value){RoomServerAddr=value; __tag__|=1; return this;}
	public PtLaunchData SetRoomServerPort(ushort value){RoomServerPort=value; __tag__|=2; return this;}
	public PtLaunchData SetMapId(uint value){MapId=value; __tag__|=4; return this;}
	public PtLaunchData SetConnectionKey(string value){ConnectionKey=value; __tag__|=8; return this;}
	public PtLaunchData SetIsStandaloneMode(bool value){IsStandaloneMode=value; __tag__|=16; return this;}
	public PtLaunchData SetPlayerNumber(byte value){PlayerNumber=value; __tag__|=32; return this;}
	
    public bool HasRoomServerAddr(){return (__tag__&1)==1;}
	public bool HasRoomServerPort(){return (__tag__&2)==2;}
	public bool HasMapId(){return (__tag__&4)==4;}
	public bool HasConnectionKey(){return (__tag__&8)==8;}
	public bool HasIsStandaloneMode(){return (__tag__&16)==16;}
	public bool HasPlayerNumber(){return (__tag__&32)==32;}
	
    public static byte[] Write(PtLaunchData data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasRoomServerAddr())buffer.WriteString(data.RoomServerAddr);
			if(data.HasRoomServerPort())buffer.WriteUInt16(data.RoomServerPort);
			if(data.HasMapId())buffer.WriteUInt32(data.MapId);
			if(data.HasConnectionKey())buffer.WriteString(data.ConnectionKey);
			if(data.HasIsStandaloneMode())buffer.WriteBool(data.IsStandaloneMode);
			if(data.HasPlayerNumber())buffer.WriteByte(data.PlayerNumber);
			
            return buffer.GetRawBytes();
        }
    }

    public static PtLaunchData Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtLaunchData data = new PtLaunchData();
            data.__tag__ = buffer.ReadByte();
			if(data.HasRoomServerAddr())data.RoomServerAddr = buffer.ReadString();
			if(data.HasRoomServerPort())data.RoomServerPort = buffer.ReadUInt16();
			if(data.HasMapId())data.MapId = buffer.ReadUInt32();
			if(data.HasConnectionKey())data.ConnectionKey = buffer.ReadString();
			if(data.HasIsStandaloneMode())data.IsStandaloneMode = buffer.ReadBool();
			if(data.HasPlayerNumber())data.PlayerNumber = buffer.ReadByte();
			
            return data;
        }       
    }
}
}
