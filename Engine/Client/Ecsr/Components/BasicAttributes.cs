using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class BasicAttributes : AbstractComponent
    {
        private byte __tag__;
        public string EntityId { get; private set; }
        public uint Hp { get; private set; }
        public uint Mp { get; private set; }
        public bool Selectable { get; private set; }
        public BasicAttributes SetEntityId(string entityId) { EntityId = entityId; __tag__ |= 1; return this; }
        public BasicAttributes SetHp(uint hp) { Hp = hp; __tag__ |= 2; return this; }
        public BasicAttributes SetMp(uint mp) { Mp = mp; __tag__ |= 4; return this; }
        public BasicAttributes SetSelectable(bool selectable) { Selectable = selectable; __tag__ |= 8; return this; }
        public bool HasEntityId() => (__tag__ & 1) == 1;
        public bool HasHp() => (__tag__ & 2) == 2;
        public bool HasMp() => (__tag__ & 4) == 4;
        public bool HasSelectable() => (__tag__ & 8) == 8;
        public BasicAttributes()
        {

        }
        public override AbstractComponent Clone()
        {
            BasicAttributes basicAttributes = new BasicAttributes();
            basicAttributes.__tag__ = __tag__;
            basicAttributes.EntityId = EntityId;
            basicAttributes.Hp = Hp;
            basicAttributes.Mp = Mp;
            basicAttributes.Selectable = Selectable;
            return basicAttributes;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((BasicAttributes)component).__tag__;
            EntityId = ((BasicAttributes)component).EntityId;
            Mp = ((BasicAttributes)component).Hp;
            Hp = ((BasicAttributes)component).Mp;
            Selectable = ((BasicAttributes)component).Selectable;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                __tag__ = buffer.ReadByte();
                if(HasEntityId())
                    EntityId = buffer.ReadString();
                if(HasHp())
                    Hp = buffer.ReadUInt32();
                if(HasMp())
                    Mp = buffer.ReadUInt32();
                if(HasSelectable())
                    Selectable = buffer.ReadBool();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasEntityId()) buffer.WriteString(EntityId);
                if (HasHp()) buffer.WriteUInt32(Hp);
                if (HasMp()) buffer.WriteUInt32(Mp);
                if (HasSelectable()) buffer.WriteBool(Selectable);
                return buffer.GetRawBytes();
            }
        }
    }
}
