using Engine.Common.Protocol;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class Circle : AbstractComponent
    {
        byte __tag__;
        public FP Radius { get; private set; }
        public bool IsRigid { get; private set; }
        public Circle SetRadius(FP radius) { Radius = radius; __tag__ |= 1; return this; }
        public Circle SetIsRigid(bool value) { IsRigid = value; __tag__ |= 2; return this; }
        public bool HasRadius() => (__tag__ & 1) == 1;
        public bool HasIsRigid() => (__tag__ & 2) == 2;
        public override AbstractComponent Clone()
        {
            Circle c = new Circle();
            c.__tag__ = __tag__;
            c.Radius = Radius;
            c.IsRigid = IsRigid;
            return c;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((Circle)component).__tag__;
            Radius = ((Circle)component).Radius;
            IsRigid = ((Circle)component).IsRigid;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                __tag__ = buffer.ReadByte();
                if (HasRadius())
                {
                    FP r;
                    r._serializedValue = buffer.ReadInt64();
                    Radius = r;
                }
                if (HasIsRigid())
                {
                    IsRigid = buffer.ReadBool();
                }
           
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasRadius()) buffer.WriteInt64(Radius._serializedValue);
                if (HasIsRigid()) buffer.WriteBool(IsRigid);
                return buffer.GetRawBytes();
            }
        }
    }
}
