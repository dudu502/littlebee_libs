using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class FsmInfo : AbstractComponent
    {
        public byte InfoType;
        public FsmInfo() 
        {

        }
        public override AbstractComponent Clone()
        {
            FsmInfo info = new FsmInfo();
            info.InfoType = InfoType;
            return info;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            InfoType = ((FsmInfo)component).InfoType;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                InfoType = buffer.ReadByte();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteByte(InfoType)
                    .GetRawBytes();
            }
        }
    }
}
