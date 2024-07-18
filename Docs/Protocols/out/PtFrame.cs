//Creation time:2024/7/18 15:33:01
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtFrame
{
    public byte __tag__ { get;private set;}

	public ushort Cmd{ get;private set;}
	public string EntityId{ get;private set;}
	public byte[] ParamContent{ get;private set;}
	   
    public PtFrame SetCmd(ushort value){Cmd=value; __tag__|=1; return this;}
	public PtFrame SetEntityId(string value){EntityId=value; __tag__|=2; return this;}
	public PtFrame SetParamContent(byte[] value){ParamContent=value; __tag__|=4; return this;}
	
    public bool HasCmd(){return (__tag__&1)==1;}
	public bool HasEntityId(){return (__tag__&2)==2;}
	public bool HasParamContent(){return (__tag__&4)==4;}
	
    public static byte[] Write(PtFrame data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasCmd())buffer.WriteUInt16(data.Cmd);
			if(data.HasEntityId())buffer.WriteString(data.EntityId);
			if(data.HasParamContent())buffer.WriteBytes(data.ParamContent);
			
            return buffer.GetRawBytes();
        }
    }

    public static PtFrame Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtFrame data = new PtFrame();
            data.__tag__ = buffer.ReadByte();
			if(data.HasCmd())data.Cmd = buffer.ReadUInt16();
			if(data.HasEntityId())data.EntityId = buffer.ReadString();
			if(data.HasParamContent())data.ParamContent = buffer.ReadBytes();
			
            return data;
        }       
    }
}
}
