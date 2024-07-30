using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class FsmInfo : AbstractComponent
    {
        byte __tag__;

        public uint InfoType { get; private set; }
        public FsmInfo SetInfoType(uint type) { InfoType = type; __tag__ |= 1; return this; }
        public bool HasInfoType() => (__tag__ & 1) == 1;
        public FsmInfo()
        {

        }
        public override AbstractComponent Clone()
        {
            FsmInfo info = new FsmInfo();
            info.__tag__ = __tag__;
            info.InfoType = InfoType;
            return info;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((FsmInfo)component).__tag__;
            InfoType = ((FsmInfo)component).InfoType;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                __tag__ = buffer.ReadByte();
                if (HasInfoType())
                    InfoType = buffer.ReadUInt32();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasInfoType()) buffer.WriteUInt32(InfoType);

                return buffer.GetRawBytes();
            }
        }
    }
}
