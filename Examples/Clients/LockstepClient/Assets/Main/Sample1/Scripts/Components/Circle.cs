using Engine.Common.Protocol;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class Circle : AbstractComponent
    {
        public FP Radius;
        public bool IsRigid;
        public override AbstractComponent Clone()
        {
            Circle c = new Circle();
            c.Radius = Radius;
            c.IsRigid = IsRigid;
            return c;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Radius = ((Circle)component).Radius;
            IsRigid = ((Circle)component).IsRigid;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                FP r;
                r._serializedValue = buffer.ReadInt64();
                Radius = r;
                IsRigid = buffer.ReadBool();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteInt64(Radius._serializedValue)
                    .WriteBool(IsRigid)
                    .GetRawBytes();
            }
        }
    }
}
