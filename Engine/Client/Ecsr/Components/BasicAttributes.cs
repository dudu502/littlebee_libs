using Engine.Common.Protocol;
using System;

namespace Engine.Client.Ecsr.Components
{
    public class BasicAttributes : AbstractComponent
    {
        public uint Hp;
        public uint Mp;
 
        public BasicAttributes()
        {

        }
        public override AbstractComponent Clone()
        {
            BasicAttributes basicAttributes = new BasicAttributes();
            basicAttributes.Hp = Hp;
            basicAttributes.Mp = Mp;
            return basicAttributes;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Mp = ((BasicAttributes)component).Hp;
            Hp = ((BasicAttributes)component).Mp;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                Hp = buffer.ReadUInt32();
                Mp = buffer.ReadUInt32();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteUInt32(Hp)
                    .WriteUInt32(Mp)
                    .GetRawBytes();
            }
        }
    }
}
