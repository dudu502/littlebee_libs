//Creation time:2024/8/9 17:51:54
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtI32Point
{
    private byte __tag__;

	public int X{ get;private set;}
	public int Y{ get;private set;}
	   
    public PtI32Point SetX(int value){X=value; __tag__|=1; return this;}
	public PtI32Point SetY(int value){Y=value; __tag__|=2; return this;}
	
    public bool HasX(){return (__tag__&1)==1;}
	public bool HasY(){return (__tag__&2)==2;}
	
    public static byte[] Write(PtI32Point data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasX())buffer.WriteInt32(data.X);
			if(data.HasY())buffer.WriteInt32(data.Y);
			
            return buffer.GetRawBytes();
        }
    }

    public static PtI32Point Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtI32Point data = new PtI32Point();
            data.__tag__ = buffer.ReadByte();
			if(data.HasX())data.X = buffer.ReadInt32();
			if(data.HasY())data.Y = buffer.ReadInt32();
			
            return data;
        }       
    }
}
}
