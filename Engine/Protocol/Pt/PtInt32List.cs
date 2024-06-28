//Creation time:2024/6/26 14:07:17
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol.Pt
{
    public class PtInt32List
    {
        public byte __tag__ { get; private set; }

        public int Value { get; private set; }

        public PtInt32List SetValue(int value) { Value = value; __tag__ |= 1; return this; }

        public bool HasValue() { return (__tag__ & 1) == 1; }

        public static byte[] Write(PtInt32List data)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(data.__tag__);
                if (data.HasValue()) buffer.WriteInt32(data.Value);

                return buffer.GetRawBytes();
            }
        }

        public static PtInt32List Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                PtInt32List data = new PtInt32List();
                data.__tag__ = buffer.ReadByte();
                if (data.HasValue()) data.Value = buffer.ReadInt32();

                return data;
            }
        }
    }
}
