using Engine.Common.Protocol;
using TrueSync;

namespace Engine.Client.Ecsr.Components
{
    public class Wall : AbstractComponent
    {
        byte __tag__;
        public FP Width { private set; get; }
        public FP Height { private set; get; }
        public bool IsRigid { private set; get; }
        public byte Dir { private set; get; }//1:right, 2:left, 4:top, 8:bottom

        public Wall SetWidth(FP width) { Width = width; __tag__ |= 1; return this; }
        public Wall SetHeight(FP height) { Height = height; __tag__ |= 2; return this; }
        public Wall SetIsRigid(bool value) { IsRigid = value; __tag__ |= 4; return this; }
        public Wall SetDir(byte value) { Dir = value; __tag__ |= 8; return this; }
        public bool HasWidth() => (__tag__ & 1) == 1;
        public bool HasHeight() => (__tag__ & 2) == 2;
        public bool HasIsRigid() => (__tag__ & 4) == 4;
        public bool HasDir() => (__tag__ & 8) == 8;
        public override AbstractComponent Clone()
        {
            Wall rect = new Wall();
            rect.__tag__ = __tag__;
            rect.Width = Width;
            rect.Height = Height;
            rect.IsRigid = IsRigid;
            rect.Dir = Dir;
            return rect;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((Wall)component).__tag__;
            Width = ((Wall)component).Width;
            Height = ((Wall)component).Height;
            IsRigid = ((Wall)component).IsRigid;
            Dir = ((Wall)component).Dir;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                __tag__ = buffer.ReadByte(); 
                FP w, h;
                if (HasWidth())
                {
                    w._serializedValue = buffer.ReadInt64();
                    Width = w;
                }
                if (HasHeight())
                {
                    h._serializedValue = buffer.ReadInt64();
                    Height = h;
                }
                if(HasIsRigid())
                    IsRigid = buffer.ReadBool();
                if(HasDir())
                    Dir = buffer.ReadByte();
              
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasWidth()) buffer.WriteInt64(Width._serializedValue);
                if (HasHeight()) buffer.WriteInt64(Height._serializedValue);
                if (HasIsRigid()) buffer.WriteBool(IsRigid);
                if (HasDir()) buffer.WriteByte(Dir);
                return buffer.GetRawBytes();
            }
        }
    }
}
