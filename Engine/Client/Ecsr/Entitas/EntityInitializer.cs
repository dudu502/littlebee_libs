using Engine.Client.Protocol.Pt;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine.Client.Ecsr.Entitas
{
    public class EntityInitializer
    {
        public EntityWorld World { get; private set; }
        public List<EntityList> InitEntities;
        public EntityInitializer(EntityWorld world)
        {
            World = world;
        }

        public virtual Task CreateEntities(uint mapId)
        {
            // need override
            return Task.CompletedTask;
        }

        public void CreateEntities(List<EntityList> entityLists)
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
        public virtual EntityList OnCreateSelfEntityComponents(Guid entityId)
        {
            return null;
        }
        public void CreateEntities(string entityId, byte[] raw)
        {
            EntityList entityList = EntityList.Read(raw);
            if(entityList != null)
            {
                World.CreateEntities(entityList);
            }
        }   
    }
}
