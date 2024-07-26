using Engine.Client.Ecsr.Components;
using Engine.Common.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;

public class Tile : AbstractComponent
{
    byte __tag__;
    public TSVector2 Size { get; private set; }
    public Tile SetSize(TSVector2 size) { Size = size; __tag__|=1; return this; }
    public bool HasSize()=>(__tag__&1)==1;
    public override AbstractComponent Clone()
    {
        Tile tile = new Tile();
        tile.__tag__ = __tag__;
        tile.Size = Size;
        return tile;
    }

    public override void CopyFrom(AbstractComponent component)
    {
        __tag__ = ((Tile)component).__tag__;
        Size = ((Tile)component).Size;
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            __tag__ = buffer.ReadByte();
            if (HasSize())
            {
                FP x, y;
                x._serializedValue = buffer.ReadInt64();
                y._serializedValue = buffer.ReadInt64();
                Size = new TSVector2(x, y);
            }
            return this;
        }
    }

    public override byte[] Serialize()
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(__tag__);
            if (HasSize())
            {
                buffer.WriteInt64(Size.x._serializedValue)
                        .WriteInt64(Size.y._serializedValue);
            }

            return buffer.GetRawBytes();
        }
    }
}

