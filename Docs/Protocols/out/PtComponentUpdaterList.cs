//Creation time:2024/8/9 17:51:54
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
public class PtComponentUpdaterList
{
    private byte __tag__;

	public List<PtComponentUpdater> Elements{ get;private set;}
	   
    public PtComponentUpdaterList SetElements(List<PtComponentUpdater> value){Elements=value; __tag__|=1; return this;}
	
    public bool HasElements(){return (__tag__&1)==1;}
	
    public static byte[] Write(PtComponentUpdaterList data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
			if(data.HasElements())buffer.WriteCollection(data.Elements,element=>PtComponentUpdater.Write(element));
			
            return buffer.GetRawBytes();
        }
    }

    public static PtComponentUpdaterList Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtComponentUpdaterList data = new PtComponentUpdaterList();
            data.__tag__ = buffer.ReadByte();
			if(data.HasElements())data.Elements = buffer.ReadCollection(retbytes=>PtComponentUpdater.Read(retbytes));
			
            return data;
        }       
    }
}
}
