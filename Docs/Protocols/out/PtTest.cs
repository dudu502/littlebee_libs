//Creation time:2024/8/9 17:51:54
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtTest
{
    private byte __tag__;

	public List<int> Elements{ get;private set;}
	   
    public PtTest SetElements(List<int> value){Elements=value; __tag__|=1; return this;}
	
    public bool HasElements(){return (__tag__&1)==1;}
	
    public static byte[] Write(PtTest data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElements())buffer.WriteCollection(data.Elements,element=>buffer.WriteInt32(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtTest Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtTest data = new PtTest();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElements())data.Elements = buffer.ReadCollection(()=>buffer.ReadInt32());
			
            return data;
        }       
    }
}
}
