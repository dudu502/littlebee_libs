//Creation time:2024/7/3 11:43:27
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtFrames
{
    public byte __tag__ { get;private set;}

	public int FrameIdx{ get;private set;}
	public List<PtFrame> KeyFrames{ get;private set;}
	   
    public PtFrames SetFrameIdx(int value){FrameIdx=value; __tag__|=1; return this;}
	public PtFrames SetKeyFrames(List<PtFrame> value){KeyFrames=value; __tag__|=2; return this;}
	
    public bool HasFrameIdx(){return (__tag__&1)==1;}
	public bool HasKeyFrames(){return (__tag__&2)==2;}
	
    public static byte[] Write(PtFrames data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasFrameIdx())buffer.WriteInt32(data.FrameIdx);
			if(data.HasKeyFrames())buffer.WriteCollection(data.KeyFrames,element=>PtFrame.Write(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtFrames Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtFrames data = new PtFrames();
            data.__tag__ = buffer.ReadByte();
			if(data.HasFrameIdx())data.FrameIdx = buffer.ReadInt32();
			if(data.HasKeyFrames())data.KeyFrames = buffer.ReadCollection(retbytes=>PtFrame.Read(retbytes));
			
            return data;
        }       
    }
}
}
