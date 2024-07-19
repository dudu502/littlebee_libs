using Engine.Common.Protocol;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class Position : AbstractComponent
    {
        public TSVector2 Pos;
        public Position()
        {

        }
        public override AbstractComponent Clone()
        {
            Position position = new Position();
            position.Pos = Pos;
            return position;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Pos = ((Position)component).Pos;
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteInt64(Pos.x._serializedValue)
                        .WriteInt64(Pos.y._serializedValue)
                        .GetRawBytes();
            }
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                FP x, y;
                x._serializedValue = buffer.ReadInt64();
                y._serializedValue = buffer.ReadInt64();
                Pos = new TSVector2(x, y);
                return this;
            }
        }
    }
}
