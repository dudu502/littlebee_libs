//Creation time:2024/8/6 17:35:58
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtI32Points
{
    public byte __tag__ { get;private set;}

	public List<PtI32Point> Elements{ get;private set;}
	   
    public PtI32Points SetElements(List<PtI32Point> value){Elements=value; __tag__|=1; return this;}
	
    public bool HasElements(){return (__tag__&1)==1;}
	
    public static byte[] Write(PtI32Points data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElements())buffer.WriteCollection(data.Elements,element=>PtI32Point.Write(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtI32Points Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtI32Points data = new PtI32Points();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElements())data.Elements = buffer.ReadCollection(retbytes=>PtI32Point.Read(retbytes));
			
            return data;
        }       
    }
}
}
