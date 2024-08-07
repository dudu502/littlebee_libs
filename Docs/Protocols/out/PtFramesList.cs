//Creation time:2024/8/6 17:35:58
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtFramesList
{
    public byte __tag__ { get;private set;}

	public int FrameIdx{ get;private set;}
	public List<PtFrames> Elements{ get;private set;}
	   
    public PtFramesList SetFrameIdx(int value){FrameIdx=value; __tag__|=1; return this;}
	public PtFramesList SetElements(List<PtFrames> value){Elements=value; __tag__|=2; return this;}
	
    public bool HasFrameIdx(){return (__tag__&1)==1;}
	public bool HasElements(){return (__tag__&2)==2;}
	
    public static byte[] Write(PtFramesList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasFrameIdx())buffer.WriteInt32(data.FrameIdx);
			if(data.HasElements())buffer.WriteCollection(data.Elements,element=>PtFrames.Write(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtFramesList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtFramesList data = new PtFramesList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasFrameIdx())data.FrameIdx = buffer.ReadInt32();
			if(data.HasElements())data.Elements = buffer.ReadCollection(retbytes=>PtFrames.Read(retbytes));
			
            return data;
        }       
    }
}
}
