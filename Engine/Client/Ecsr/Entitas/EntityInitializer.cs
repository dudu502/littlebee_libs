using Engine.Client.Ecsr.Components;
using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using System;

namespace Engine.Client.Ecsr.Entitas
{
    public class EntityInitializer
    {
        public EntityWorld World { get; private set; }
        public EntityInitializer(EntityWorld world)
        {
            World = world;
        }

        /// <summary>
        /// Load component byte array
        /// </summary>
        /// <param name="mapBindaryRaw"></param>
        public void CreateEntities(byte[] mapBindaryRaw)
        {

        }

        public void CreateEntities(PtFrame frame)
        {
            using (ByteBuffer bytebuffer = new ByteBuffer(frame.ParamContent))
            {
                int count = bytebuffer.ReadInt32();
                for (int i = 0; i < count; ++i)
                {
                    Type componentType = Type.GetType(bytebuffer.ReadString());
                    AbstractComponent component = Activator.CreateInstance(componentType) as AbstractComponent;
                    component.Deserialize(bytebuffer.ReadBytes());
                    Entity entity = World.CreateEntity(new Guid(frame.EntityId));
                    entity.AddComponent(component);
                }
            }
        }
    }
}
