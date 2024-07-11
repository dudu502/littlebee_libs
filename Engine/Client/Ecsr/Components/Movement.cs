using Engine.Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class Movement : AbstractComponent
    {
        public FP Speed;
        public TSVector2 Direction = TSVector2.zero;
        public Movement()
        {

        }
        public override AbstractComponent Clone()
        {
            Movement movement = new Movement();
            movement.Speed = Speed;
            movement.Direction = Direction;
            return movement;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Speed = ((Movement)component).Speed;
            Direction = ((Movement)component).Direction;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                FP speed, dx, dy;
                speed._serializedValue = buffer.ReadInt64();
                dx._serializedValue = buffer.ReadInt64();
                dy._serializedValue = buffer.ReadInt64();
                Speed = speed;
                Direction = new TSVector2(dx, dy);
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteInt64(Speed._serializedValue);
                buffer.WriteInt64(Direction.x._serializedValue);
                buffer.WriteInt64(Direction.y._serializedValue);
                return buffer.GetRawBytes();
            }
        }
    }
}
