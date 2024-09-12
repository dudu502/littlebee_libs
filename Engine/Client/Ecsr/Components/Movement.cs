using Engine.Common;
using Engine.Common.Protocol;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class Movement : AbstractComponent
    {
        private byte __tag__; 
        public FP Speed { private set; get; }
        public TSVector2 Direction { private set; get; } = TSVector2.zero;
        public Movement()
        {

        }

        public Movement SetSpeed(FP speed) { Speed = speed; __tag__ |= 1; return this; }
        public Movement SetDirection(TSVector2 dir) { Direction = dir; __tag__ |= 2;return this; }

        public bool HasSpeed() => (__tag__ & 1) == 1;
        public bool HasDirection() => (__tag__ & 2) == 2;

        public override AbstractComponent Clone()
        {
            Movement movement = new Movement();
            movement.__tag__ = __tag__;
            movement.Speed = Speed;
            movement.Direction = Direction;
            return movement;

        }
        public override void UpdateParams(byte[] content)
        {
            base.UpdateParams(content);
            Deserialize(content);
            Context.Retrieve(Context.CLIENT).Logger.Warn("Movement UpdateParams "+ this.Direction.ToString());
        }
        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((Movement)component).__tag__;
            Speed = ((Movement)component).Speed;
            Direction = ((Movement)component).Direction;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                FP speed, dx, dy;
                __tag__ = buffer.ReadByte();
                if (HasSpeed())
                {
                    speed._serializedValue = buffer.ReadInt64();
                    Speed = speed;
                }
                if(HasDirection())
                {
                    dx._serializedValue = buffer.ReadInt64();
                    dy._serializedValue = buffer.ReadInt64();
                    Direction = new TSVector2(dx, dy);
                }
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasSpeed()) buffer.WriteInt64(Speed._serializedValue);
                if (HasDirection())
                {
                    buffer.WriteInt64(Direction.x._serializedValue)
                        .WriteInt64(Direction.y._serializedValue);
                }
                return buffer.GetRawBytes();
            }
        }
    }
}
