using Engine.Client.Ecsr.Components;
using Engine.Client.Protocol.Pt;
using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Engine.Client.Ecsr.Entitas
{
    public class EntityInitializer
    {
        public EntityWorld World { get; private set; }
        public EntityInitializer(EntityWorld world)
        {
            World = world;
        }

        public virtual void OnCreateEntities(uint mapId)
        {
    
        }

        public void OnCreateEntities(List<EntityList> entityLists)
        {
            foreach (EntityList entityList in entityLists)
            {
                foreach(Entity entity in entityList.Elements) 
                    World.CreateEntity(entity);
            }
        }
        /// <summary>
        /// Create self Entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual List<Entity> OnCreateSelfEntityComponents(Guid entityId)
        {
            return null;
        }
        public void OnCreateEntities(PtFrame frame)
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
