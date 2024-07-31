using Engine.Common.Protocol;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class MoveToPosition : AbstractComponent
    {
        byte __tag__;

        public FP Speed;
        public TSVector2 ToPosition { private set; get; }
        public MoveToPosition SetSpeed(FP speed) { Speed = speed;__tag__ |= 1;return this; }
        public bool HasSpeed() => (__tag__ & 1) == 1;

        public MoveToPosition SetToPosition(TSVector2 toPos) { ToPosition = toPos;__tag__ |= 2;return this; }
        public bool HasToPosition() => (__tag__ & 2) == 2;
        
        public override AbstractComponent Clone()
        {
            MoveToPosition position = new MoveToPosition();
            position.__tag__ = __tag__;
            position.Speed = Speed;
            position.ToPosition = ToPosition;
            return position;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((MoveToPosition)component).__tag__;
            Speed = ((MoveToPosition)component).Speed;
            ToPosition = ((MoveToPosition)component).ToPosition;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                FP speed, tx, ty;
                __tag__ = buffer.ReadByte();
                if(HasSpeed())
                {
                    speed._serializedValue = buffer.ReadInt64();
                    Speed = speed;
                }
                if (HasToPosition())
                {
                    tx._serializedValue = buffer.ReadInt64();
                    ty._serializedValue = buffer.ReadInt64();
                    ToPosition = new TSVector2(tx,ty);
                }
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasSpeed()) buffer.WriteInt64(Speed._serializedValue);
                if (HasToPosition()) buffer.WriteInt64(ToPosition.x._serializedValue).WriteInt64(ToPosition.y._serializedValue);

                return buffer.GetRawBytes();
            }
        }
    }
}
