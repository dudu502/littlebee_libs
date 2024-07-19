using Engine.Common.Protocol;

namespace Engine.Client.Ecsr.Components
{
    public class Appearance : AbstractComponent
    {
        public const byte StatusDefault = 0;
        public const byte StatusStartLoading = 1;

        public byte Status = StatusDefault;
        public string Resource;
        public byte ShaderR = 255;
        public byte ShaderG = 255;
        public byte ShaderB = 255;
        /// <summary>
        /// Selected by EntityId
        /// If Empty means unselected
        /// </summary>
        public string SelectedByEntityId = string.Empty;
        public override AbstractComponent Clone()
        {
            Appearance appearance = new Appearance();
            appearance.Status = Status;
            appearance.Resource = Resource;
            appearance.ShaderR = ShaderR;
            appearance.ShaderG = ShaderG;
            appearance.ShaderB = ShaderB;
            appearance.SelectedByEntityId = SelectedByEntityId;
            return appearance;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            Status = ((Appearance)component).Status;
            Resource = ((Appearance)component).Resource;
            ShaderR = ((Appearance)component).ShaderR;
            ShaderG = ((Appearance)component).ShaderG;
            ShaderB = ((Appearance)component).ShaderB;
            SelectedByEntityId = ((Appearance)component).SelectedByEntityId;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer  = new ByteBuffer(bytes))
            {
                Status = buffer.ReadByte();
                Resource = buffer.ReadString();
                ShaderR = buffer.ReadByte();
                ShaderG = buffer.ReadByte();
                ShaderB = buffer.ReadByte();
                SelectedByEntityId = buffer.ReadString();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                return buffer.WriteByte(Status)
                    .WriteString(Resource)
                    .WriteByte(ShaderR)
                    .WriteByte(ShaderG)
                    .WriteByte(ShaderB)
                    .WriteString(SelectedByEntityId)
                    .GetRawBytes();
            }
        }
    }
}
