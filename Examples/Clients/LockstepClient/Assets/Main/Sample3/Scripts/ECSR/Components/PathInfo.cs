using Engine.Client.Ecsr.Components;
using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using TrueSync;

public class PathInfo : AbstractComponent
{
    byte __tag__;
    public byte DoFindPath { private set; get; }
    public PtI32Points PathPoints { private set; get; }
    public ushort Step { private set; get; }
    public TSVector2 DestinationPosition { private set; get; }
    public bool HasDoFindPath() => (__tag__ & 1) == 1;
    public bool HasPathPoints() => (__tag__ & 2) == 2;
    public bool HasStep() => (__tag__ & 4) == 4;
    public bool HasDestinationPosition() => (__tag__ & 8) == 8;
    public PathInfo SetDoFindPath(byte doFindPath) { DoFindPath = doFindPath; __tag__ |= 1; return this; }
    public PathInfo SetPathPoints(PtI32Points points) { PathPoints = points; __tag__ |= 2; return this; }
    public PathInfo SetStep(ushort step) { Step = step; __tag__ |= 4; return this; }
    public PathInfo SetDestinationPosition(TSVector2 tSVector) { DestinationPosition = tSVector; __tag__ |= 8; return this; }

    public override AbstractComponent Clone()
    {
        PathInfo pathInfo = new PathInfo();
        pathInfo.__tag__ = __tag__;
        pathInfo.DoFindPath = DoFindPath;
        pathInfo.PathPoints = PathPoints;
        pathInfo.Step = Step;
        pathInfo.DestinationPosition = DestinationPosition;
        return pathInfo;
    }

    public override void CopyFrom(AbstractComponent component)
    {
        __tag__ = ((PathInfo)component).__tag__;
        DoFindPath = ((PathInfo)component).DoFindPath;
        PathPoints = ((PathInfo)component).PathPoints;
        Step = ((PathInfo)component).Step;
        DestinationPosition = ((PathInfo)component).DestinationPosition;
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        using (ByteBuffer buffer = new ByteBuffer(bytes))
        {
            __tag__ = buffer.ReadByte();
            if (HasDoFindPath())
                DoFindPath = buffer.ReadByte();
            if (HasPathPoints())
                PathPoints = PtI32Points.Read(buffer.ReadBytes());
            if (HasStep())
                Step = buffer.ReadUInt16();
            if (HasDestinationPosition())
            {
                FP x, y;
                x._serializedValue = buffer.ReadInt64();
                y._serializedValue = buffer.ReadInt64();
                DestinationPosition = new TSVector2(x, y);
            }
            return this;
        }
    }

    public override byte[] Serialize()
    {
        using (ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(__tag__);
            if (HasDoFindPath()) buffer.WriteByte(DoFindPath);
            if (HasPathPoints()) buffer.WriteBytes(PtI32Points.Write(PathPoints));
            if (HasStep()) buffer.WriteUInt16(Step);
            if (HasDestinationPosition())
            {
                buffer.WriteInt64(DestinationPosition.x._serializedValue)
                      .WriteInt64(DestinationPosition.y._serializedValue);
            }
            return buffer.GetRawBytes();
        }
    }
}