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

        public TSVector2 Forward { get; private set; }
        public Position SetForward(TSVector2 forward) { Forward = forward;__tag__ |= 2;return this; }
        public bool HasForwad() => (__tag__ & 2) == 2;
        public Position()
        {

        }
        public override AbstractComponent Clone()
        {
            Position position = new Position();
            position.__tag__ = __tag__;
            position.Pos = Pos;
            position.Forward = Forward;
            return position;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((Position)component).__tag__;
            Pos = ((Position)component).Pos;
            Forward = ((Position)component).Forward;
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
                if (HasForwad())
                {
                    buffer.WriteInt64(Forward.x._serializedValue)
                        .WriteInt64(Forward.y._serializedValue);
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
                if (HasForwad())
                {
                    FP fx, fy;
                    fx._serializedValue = buffer.ReadInt64();
                    fy._serializedValue = buffer.ReadInt64();
                    Forward = new TSVector2(fx, fy);
                }
                return this;
            }
        }
    }
}
