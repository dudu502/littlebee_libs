using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class Appearance : AbstractComponent
    {
        public const byte StatusDefault = 0;
        public const byte StatusStartLoading = 1;
        private byte __tag__;

        public byte Status { get; private set; } = StatusDefault;
        public string Resource { get; private set; }
        public byte ShaderR { get; private set; } = 255;
        public byte ShaderG { get; private set; } = 255;
        public byte ShaderB { get; private set; } = 255;

        public Appearance SetStatus(byte status) { Status = status; __tag__ |= 1; return this; }
        public Appearance SetResource(string resource) { Resource = resource; __tag__ |= 2; return this; }
        public Appearance SetShaderR(byte r) { ShaderR = r; __tag__ |= 4; return this; }
        public Appearance SetShaderG(byte g) { ShaderG = g; __tag__ |= 8; return this; }
        public Appearance SetShaderB(byte b) { ShaderB = b; __tag__ |= 16; return this; }

        public bool HasStatus() => (__tag__ & 1) == 1;
        public bool HasResource() => (__tag__ & 2) == 2;
        public bool HasShaderR() => (__tag__ & 4) == 4;
        public bool HasShaderG() => (__tag__ & 8) == 8;
        public bool HasShaderB() => (__tag__ & 16) == 16;
        public override AbstractComponent Clone()
        {
            Appearance appearance = new Appearance();
            appearance.__tag__ = __tag__;
            appearance.Status = Status;
            appearance.Resource = Resource;
            appearance.ShaderR = ShaderR;
            appearance.ShaderG = ShaderG;
            appearance.ShaderB = ShaderB;
            return appearance;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            __tag__ = ((Appearance)component).__tag__;
            Status = ((Appearance)component).Status;
            Resource = ((Appearance)component).Resource;
            ShaderR = ((Appearance)component).ShaderR;
            ShaderG = ((Appearance)component).ShaderG;
            ShaderB = ((Appearance)component).ShaderB;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                __tag__ = buffer.ReadByte();
                if (HasStatus())
                    Status = buffer.ReadByte();
                if (HasResource())
                    Resource = buffer.ReadString();
                if (HasShaderR())
                    ShaderR = buffer.ReadByte();
                if (HasShaderG())
                    ShaderG = buffer.ReadByte();
                if (HasShaderB())
                    ShaderB = buffer.ReadByte();

                return this;
            }
        }

        public override byte[] Serialize()
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteByte(__tag__);
                if (HasStatus()) buffer.WriteByte(Status);
                if (HasResource()) buffer.WriteString(Resource);
                if (HasShaderR()) buffer.WriteByte(ShaderR);
                if (HasShaderG()) buffer.WriteByte(ShaderG);
                if (HasShaderB()) buffer.WriteByte(ShaderB);
                return buffer.GetRawBytes();
            }
        }
    }
}
