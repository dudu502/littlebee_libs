using Engine.Common.Protocol;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class Position : AbstractComponent
    {
        byte __tag__;
        public TSVector2 Pos { get; private set; } = new TSVector2();
        public Position SetPos(TSVector2 pos) { Pos = pos; __tag__ |= 1; return this; }
        public bool HasPos() => (__tag__ & 1) == 1;
        public Position()
        {

        }
        public override AbstractComponent Clone()
        {
            Position position = new Position();
            position.__tag__ = __tag__;
            position.Pos = Pos;
            return position;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((Position)component).__tag__;
            Pos = ((Position)component).Pos;
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasPos())
                {
                    buffer.WriteInt64(Pos.x._serializedValue)
                        .WriteInt64(Pos.y._serializedValue);
                }
                return buffer.GetRawBytes();
            }
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                __tag__ = buffer.ReadByte();
                if (HasPos())
                {
                    FP x, y;
                    x._serializedValue = buffer.ReadInt64();
                    y._serializedValue = buffer.ReadInt64();
                    Pos = new TSVector2(x, y);
                }
                return this;
            }
        }
    }
}
