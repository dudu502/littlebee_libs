using Engine.Client.Ecsr.Components;
using Engine.Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Ecsr.Components
{
    public class CComponent : AbstractComponent
    {
        public int DS;
        public string EF;
        public CComponent()
        {

        }

        public override AbstractComponent Clone()
        {
            CComponent cComponent = new CComponent();
            cComponent.DS = DS;
            cComponent.EF = EF;
            return cComponent;
        }

        public override void CopyFrom(AbstractComponent component)
        {
            DS = ((CComponent)component).DS;
            EF = ((CComponent)component).EF;
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            using(ByteBuffer buffer = new ByteBuffer(bytes))
            {
                DS = buffer.ReadInt32();
                EF = buffer.ReadString();
                return this;
            }
        }

        public override byte[] Serialize()
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteInt32(DS); 
                buffer.WriteString(EF);
                return buffer.GetRawBytes();
            }
        }
    }
}
