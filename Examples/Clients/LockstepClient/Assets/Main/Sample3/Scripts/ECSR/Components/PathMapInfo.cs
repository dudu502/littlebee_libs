using Engine.Client.Ecsr.Components;
using Engine.Common.Protocol;

public class PathMapInfo : AbstractComponent
{
    byte __tag__;
    public uint MapId { get; private set; }
    public PathMapInfo SetMapId(uint mapId) { MapId = mapId;__tag__ |= 1;return this; }
    public bool HasMapId() => (__tag__ & 1) == 1;
    public PtPathMapNode[,] MapNodes { get; private set; }
    public PathMapInfo SetMapNodes(PtPathMapNode[,] mapNodes)
    {
        MapNodes = mapNodes;__tag__ |= 2; return this;
    }
    public bool HasMapNodes() => (__tag__ & 2) == 2;
    public override AbstractComponent Clone()
    {
        PathMapInfo info = new PathMapInfo();
        info.__tag__ = __tag__;
        info.MapId = MapId;
        info.MapNodes = MapNodes;
        return info;
    }

    public override void CopyFrom(AbstractComponent component)
    {
        __tag__ = ((PathMapInfo)component).__tag__;
        MapId = ((PathMapInfo)component).MapId;
        MapNodes = ((PathMapInfo)component).MapNodes;
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        using (ByteBuffer buffer = new ByteBuffer(bytes))
        {
            __tag__ = buffer.ReadByte();
            if (HasMapId()) MapId = buffer.ReadUInt32();
            if (HasMapNodes())
            {
                int iCount = buffer.ReadInt32();
                int jCount = buffer.ReadInt32();
                MapNodes = new PtPathMapNode[iCount, jCount];
                for (int i = 0; i < iCount; ++i)
                {
                    for (int j = 0; j < jCount; ++j)
                    {
                        MapNodes[i, j] = PtPathMapNode.Read(buffer.ReadBytes());
                    }
                }
            }
            return this;
        }
    }

    public override byte[] Serialize()
    {
        using (ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(__tag__);
            if (HasMapId()) buffer.WriteUInt32(MapId);
            if (HasMapNodes())
            {
                int iCount = MapNodes.GetLength(0);
                int jCount = MapNodes.GetLength(1);
                buffer.WriteInt32(iCount);
                buffer.WriteInt32(jCount);
                for (int i = 0; i < iCount; ++i)
                {
                    for (int j = 0; j < jCount; ++j)
                    {
                        buffer.WriteBytes(PtPathMapNode.Write(MapNodes[i, j]));
                    }
                }
            }
            return buffer.GetRawBytes();
        }
    }
}

