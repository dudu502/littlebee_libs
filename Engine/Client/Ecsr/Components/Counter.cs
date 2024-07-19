using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class Counter : AbstractComponent
    {
        public int Count;
        public Counter()
        {

        }
        public override AbstractComponent Clone()
        {
            Counter counter = new Counter();
            counter.Count = Count;
            return counter;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Count = ((Counter)component).Count;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes)) 
            {
                Count = buffer.ReadInt32();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteInt32(Count)
                    .GetRawBytes();
            }
        }
    }
}
