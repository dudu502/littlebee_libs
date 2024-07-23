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
    public TSVector2 Size;
    public override AbstractComponent Clone()
    {
        Tile tile = new Tile();
        tile.Size = Size;
        return tile;
    }

    public override void CopyFrom(AbstractComponent component)
    {
        Size = ((Tile)component).Size;
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            FP x, y;
            x._serializedValue = buffer.ReadInt64();
            y._serializedValue = buffer.ReadInt64();
            Size = new TSVector2(x,y);
            return this;
        }
    }

    public override byte[] Serialize()
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            return buffer.WriteInt64(Size.x._serializedValue)
                .WriteInt64(Size.y._serializedValue)
                .GetRawBytes();
        }
    }
}

