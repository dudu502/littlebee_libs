//Creation time:2024/8/9 17:51:54
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtComponentUpdater
{
    private byte __tag__;

	public string ComponentClsName{ get;private set;}
	public byte[] ParamContent{ get;private set;}
	   
    public PtComponentUpdater SetComponentClsName(string value){ComponentClsName=value; __tag__|=1; return this;}
	public PtComponentUpdater SetParamContent(byte[] value){ParamContent=value; __tag__|=2; return this;}
	
    public bool HasComponentClsName(){return (__tag__&1)==1;}
	public bool HasParamContent(){return (__tag__&2)==2;}
	
    public static byte[] Write(PtComponentUpdater data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasComponentClsName())buffer.WriteString(data.ComponentClsName);
			if(data.HasParamContent())buffer.WriteBytes(data.ParamContent);
			
            return buffer.GetRawBytes();
        }
    }

    public static PtComponentUpdater Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtComponentUpdater data = new PtComponentUpdater();
            data.__tag__ = buffer.ReadByte();
			if(data.HasComponentClsName())data.ComponentClsName = buffer.ReadString();
			if(data.HasParamContent())data.ParamContent = buffer.ReadBytes();
			
            return data;
        }       
    }
}
}
