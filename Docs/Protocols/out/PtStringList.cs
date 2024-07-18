//Creation time:2024/7/18 15:33:01
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtStringList
{
    public byte __tag__ { get;private set;}

	public List<string> Element{ get;private set;}
	   
    public PtStringList SetElement(List<string> value){Element=value; __tag__|=1; return this;}
	
    public bool HasElement(){return (__tag__&1)==1;}
	
    public static byte[] Write(PtStringList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElement())buffer.WriteCollection(data.Element,element=>buffer.WriteString(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtStringList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtStringList data = new PtStringList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElement())data.Element = buffer.ReadCollection(()=>buffer.ReadString());
			
            return data;
        }       
    }
}
}
