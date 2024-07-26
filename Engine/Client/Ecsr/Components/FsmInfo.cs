using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class FsmInfo : AbstractComponent
    {
        byte __tag__;

        public byte InfoType { get; private set; }
        public FsmInfo SetInfoType(byte type) { InfoType = type; __tag__ |= 1;return this; }
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
                if(HasInfoType())
                    InfoType = buffer.ReadByte();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasInfoType()) buffer.WriteByte(InfoType);

                return buffer.GetRawBytes();
            }
        }
    }
}
