using Engine.Common.Protocol;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class Wall : AbstractComponent
    {
        public FP Width;
        public FP Height;
        public bool IsRigid;
        public byte Dir;//1:right, 2:left, 4:top, 8:bottom
        public override AbstractComponent Clone()
        {
            Wall rect = new Wall();
            rect.Width = Width;
            rect.Height = Height;
            rect.IsRigid = IsRigid;
            rect.Dir = Dir;
            return rect;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Width = ((Wall)component).Width;
            Height = ((Wall)component).Height;
            IsRigid = ((Wall)component).IsRigid;
            Dir= ((Wall)component).Dir;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes)) 
            {
                FP w,h;
                w._serializedValue = buffer.ReadInt64();
                h._serializedValue = buffer.ReadInt64();
                IsRigid = buffer.ReadBool();
                Dir = buffer.ReadByte();
                Width = w; Height = h;
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteInt64(Width._serializedValue)
                    .WriteInt64(Height._serializedValue)
                    .WriteBool(IsRigid)
                    .WriteByte(Dir)
                    .GetRawBytes();
            }
        }
    }
}
