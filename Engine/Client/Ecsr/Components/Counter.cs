using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class Counter : AbstractComponent
    {
        private byte __tag__;
        public int Count { get; private set; }
        public Counter SetCount(int count) { Count = count; __tag__ |= 1; return this; }
        public bool HasCount() => (__tag__ & 1) == 1;
        public Counter()
        {

        }
        public override AbstractComponent Clone()
        {
            Counter counter = new Counter();
            counter.__tag__ = __tag__;
            counter.Count = Count;
            return counter;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((Counter)component).__tag__;
            Count = ((Counter)component).Count;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes)) 
            {
                __tag__ = buffer.ReadByte();
                if(HasCount())
                    Count = buffer.ReadInt32();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasCount()) buffer.WriteInt32(Count);
                return buffer.GetRawBytes();
            }
        }
    }
}
