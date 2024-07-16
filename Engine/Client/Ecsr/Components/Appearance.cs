using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class Appearance : AbstractComponent
    {
        public const byte StatusDefault = 0;
        public const byte StatusStartLoading = 1;

        public byte Status = StatusDefault;
        public string Resource;
        public override AbstractComponent Clone()
        {
            Appearance appearance = new Appearance();
            appearance.Status = Status;
            appearance.Resource = Resource;
            return appearance;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Status = ((Appearance)component).Status;
            Resource = ((Appearance)component).Resource;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer  = new ByteBuffer(bytes))
            {
                Status = buffer.ReadByte();
                Resource = buffer.ReadString();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(Status);
                buffer.WriteString(Resource);
                return buffer.GetRawBytes();
            }
        }
    }
}
