using Engine.Client.Ecsr.Components;
using Engine.Common.Protocol;

public class FSMInfo : AbstractComponent
{
    byte __tag__;
    public byte InfoType { get; private set; }
    public FSMInfo SetInfoType(byte type) { InfoType = type; __tag__ |= 1; return this; }
    public bool HasInfoType() => (__tag__ & 1) == 1;

    public int CurrentId { get; private set; }
    public FSMInfo SetCurrentId(int currentId) { CurrentId = currentId; __tag__ |= 2; return this; }
    public bool HasCurrentId() => (__tag__ & 2) == 2;


    public override AbstractComponent Clone()
    {
        FSMInfo info = new FSMInfo();
        info.__tag__ = __tag__;
        info.InfoType = InfoType;
        info.CurrentId = CurrentId;
        return info;
    }

    public override void CopyFrom(AbstractComponent component)
    {
        __tag__ = ((FSMInfo)component).__tag__;
        InfoType = ((FSMInfo)component).InfoType;
        CurrentId = ((FSMInfo)component).CurrentId;
    }

    public override AbstractComponent Deserialize(byte[] bytes)
    {
        using (ByteBuffer buffer = new ByteBuffer(bytes))
        {
            __tag__ = buffer.ReadByte();
            if (HasInfoType())
                InfoType = buffer.ReadByte();
            if (HasCurrentId())
                CurrentId = buffer.ReadInt32();
            return this;
        }
    }

    public override byte[] Serialize()
    {
        using (ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(__tag__);
            if (HasInfoType()) buffer.WriteByte(InfoType);
            if (HasCurrentId()) buffer.WriteInt32(CurrentId);
            return buffer.GetRawBytes();
        }
    }
}
