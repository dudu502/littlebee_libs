using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class Appearance : AbstractComponent
    {
        public string Resource;
        public override AbstractComponent Clone()
        {
            Appearance appearance = new Appearance();
            appearance.Resource = Resource;
            return appearance;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Resource = ((Appearance)component).Resource;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer  = new ByteBuffer(bytes))
            {
                Resource = buffer.ReadString();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteString(Resource);
                return buffer.GetRawBytes();
            }
        }
    }
}
