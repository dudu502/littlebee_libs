//Creation time:2024/6/26 13:40:02
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Common.Protocol;

namespace Engine.Common.Protocol
{
    public partial class PtMessagePackage
    {
        public int ExtraPeerId;
        public object ExtraData;
    }
    public partial class PtMessagePackage
    {
        public byte __tag__ { get; private set; }

        public ushort MessageId { get; private set; }
        public byte[] Content { get; private set; }

        public PtMessagePackage SetMessageId(ushort value) { MessageId = value; __tag__ |= 1; return this; }
        public PtMessagePackage SetContent(byte[] value) { Content = value; __tag__ |= 2; return this; }

        public bool HasMessageId() { return (__tag__ & 1) == 1; }
        public bool HasContent() { return (__tag__ & 2) == 2; }

        public static byte[] Write(PtMessagePackage data)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(data.__tag__);
                if (data.HasMessageId()) buffer.WriteUInt16(data.MessageId);
                if (data.HasContent()) buffer.WriteBytes(data.Content);

                return buffer.GetRawBytes();
            }
        }

        public static PtMessagePackage Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                PtMessagePackage data = new PtMessagePackage();
                data.__tag__ = buffer.ReadByte();
                if (data.HasMessageId()) data.MessageId = buffer.ReadUInt16();
                if (data.HasContent()) data.Content = buffer.ReadBytes();

                return data;
            }
        }

        public static PtMessagePackage Build(ushort messageId)
        {
            return new PtMessagePackage().SetMessageId(messageId);
        }
        public static PtMessagePackage Build(ushort messageId, byte[] bytes)
        {
            PtMessagePackage message = Build(messageId);
            if (bytes != null)
                message.SetContent(bytes);
            return message;
        }
    }
}
