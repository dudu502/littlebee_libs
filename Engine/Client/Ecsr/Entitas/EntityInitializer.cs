using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Renders;
using Engine.Client.Protocol.Pt;
using Engine.Common;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        /// <summary>
        /// Load map config
        /// Load Render...
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public async Task CreateEntities(uint mapId)
        {
            await Task.Run(async () =>
            {
                string path = Path.Combine(Context.Retrieve(Context.CLIENT).GetMeta(ContextMetaId.PERSISTENT_DATA_PATH), "map", mapId + ".map");
                byte[] bytes = File.ReadAllBytes(path);
                PtMap map = PtMap.Read(bytes);
                if (map.HasEntities())
                {
                    List<CreateEntityRendererRequest> requests = new List<CreateEntityRendererRequest>();
                    foreach (Entity entity in map.Entities.Elements)
                    {
                        World.CreateEntity(entity);

                        Appearance appearance = entity.GetComponent(typeof(Appearance)) as Appearance;
                        if (appearance != null && appearance.Status == Appearance.StatusDefault)
                        {
                            appearance.SetStatus(Appearance.StatusStartLoading);
                            CreateEntityRendererRequest request = new CreateEntityRendererRequest(entity.Id, appearance.Status, appearance.Resource);
                            requests.Add(request);
                            World.GetRenderSpawner().CreateEntityRenderer(request);
                        }
                    }
                    bool loadedAll = false;
                    while (!loadedAll)
                    {
                        await Task.Delay(10);
                        loadedAll = true;
                        foreach (CreateEntityRendererRequest request in requests)
                        {
                            if (request.Status != Appearance.StatusLoaded)
                            {
                                loadedAll = false;
                                continue;
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Load map config
        /// Load Render...
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        //public virtual Task CreateEntities(uint mapId)
        //{
        //    return Task.Run(() =>
        //    {
        //        string path = Path.Combine(Context.Retrieve(Context.CLIENT).GetMeta(ContextMetaId.PERSISTENT_DATA_PATH), "map", mapId + ".map");
        //        byte[] bytes = File.ReadAllBytes(path);
        //        PtMap map = PtMap.Read(bytes);
        //        if (map.HasEntities())
        //        {
        //            List<CreateEntityRendererRequest> requests = new List<CreateEntityRendererRequest>();
        //            foreach(Entity entity in map.Entities.Elements)
        //            {
        //                World.CreateEntity(entity);

        //                Appearance appearance = entity.GetComponent(typeof(Appearance)) as Appearance;
        //                if(appearance != null && appearance.Status == Appearance.StatusDefault) 
        //                {
        //                    appearance.SetStatus(Appearance.StatusStartLoading);
        //                    CreateEntityRendererRequest request = new CreateEntityRendererRequest(entity.Id, appearance.Status, appearance.Resource);
        //                    requests.Add(request);
        //                    World.GetRenderSpawner().CreateEntityRenderer(request);
        //                }
        //            }

        //            bool loadedAll = false;
        //            while (!loadedAll)
        //            {
        //                Thread.Sleep(20);
        //                loadedAll = true;
        //                foreach (CreateEntityRendererRequest request in requests)
        //                {
        //                    if (request.Status != Appearance.StatusLoaded)
        //                    {
        //                        loadedAll = false;
        //                        continue;
        //                    }
        //                }
        //            }
        //        }
        //    });
        //}

        public void CreateEntities(List<EntityList> entityLists)
        {
            foreach (EntityList entityList in entityLists)
            {
                foreach (Entity entity in entityList.Elements)
                {
                    World.CreateEntity(entity);
                    Context.Retrieve(Context.CLIENT).Logger.Warn("CreateEntities EntityId:"+entity.Id.ToString());
                }
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
