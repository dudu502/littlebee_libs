using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Renders;
using Engine.Client.Protocol.Pt;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;

namespace Engine.Client.Ecsr.Entitas
{
    public struct EntityComponent
    {
        public Guid Id;
        public AbstractComponent Component;
        public EntityComponent(Guid id,AbstractComponent component)
        {
            Id = id;
            Component = component;
        }
    }
    public sealed class EntityWorld : IDisposable
    {
        public sealed class FrameRawData
        {
            public SortedDictionary<Guid, Entity> EntityDict;

            public FrameRawData(SortedDictionary<Guid, Entity> source)
            {
                EntityDict = source;
            }
            public FrameRawData()
            {
                EntityDict = new SortedDictionary<Guid, Entity>();
            }
        }
        EntityInitializer m_EntityInitializer;
        FrameRawData m_CurrentFrameData;
        EntityRenderSpawner m_EntityRenderSpawner;
        public bool IsActive { private set; get; }
        public EntityWorld()
        {
            IsActive = true;
            m_CurrentFrameData = new FrameRawData();
        }
        public void SetEntityInitializer(EntityInitializer init)
        {
            m_EntityInitializer = init;
        }
        public EntityInitializer GetEntityInitializer()
        {
            return m_EntityInitializer;
        }
        public void SetEntityRenderSpawner(EntityRenderSpawner renderSpawner)
        {
            m_EntityRenderSpawner = renderSpawner;
        }
        public EntityRenderSpawner GetRenderSpawner()
        {
            return m_EntityRenderSpawner;
        }
        public int GetEntityCount()
        {
            return m_CurrentFrameData.EntityDict.Count;
        }
        public Entity GetEntity(Guid id)
        {
            if (m_CurrentFrameData.EntityDict.TryGetValue(id, out Entity entity))
                return entity;
            return null;
        }
        public Entity CreateEntity()
        {
            Entity entity = new Entity();
            m_CurrentFrameData.EntityDict[entity.Id] = entity;
            return entity;
        }
        public Entity CreateEntity(Guid entityId)
        {
            Entity entity = new Entity(entityId);
            m_CurrentFrameData.EntityDict[entity.Id] = entity;
            return entity;
        }
        public Entity CreateEntity(Entity entity)
        {
            m_CurrentFrameData.EntityDict[entity.Id] = entity;
            return entity;
        }
        public void CreateEntities(EntityList entityList)
        {
            foreach (Entity entity in entityList.Elements)
                CreateEntity(entity);
        }
        public bool RemoveEntity(Guid entityId)
        {
            return m_CurrentFrameData.EntityDict.Remove(entityId);
        }


        #region Read Component
        public T0 ReadComponent<T0>(Guid entityId)
            where T0 : AbstractComponent
        {
            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                if (entity.Components.TryGetValue(typeof(T0), out AbstractComponent component))
                {
                    return (T0)component;
                }
            }
            return null;
        }
        public (T0,T1) ReadComponent<T0,T1>(Guid entityId) 
            where T0 : AbstractComponent where T1 : AbstractComponent
        {
            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                if (entity.Components.TryGetValue(typeof(T0), out AbstractComponent component0)
                    && entity.Components.TryGetValue(typeof(T1), out AbstractComponent component1))
                {
                    return ((T0)component0,(T1)component1);
                }
            }
            return (null,null);
        }

        public (T0,T1,T2) ReadComponent<T0, T1,T2>(Guid entityId)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent
        {
            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                if (entity.Components.TryGetValue(typeof(T0), out AbstractComponent component0)
                    && entity.Components.TryGetValue(typeof(T1), out AbstractComponent component1)
                    && entity.Components.TryGetValue(typeof(T2), out AbstractComponent component2))
                {
                    return ((T0)component0, (T1)component1, (T2)component2);
                }
            }
            return (null, null, null);
        }

        public (T0,T1,T2,T3) ReadComponent<T0, T1, T2,T3>(Guid entityId)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
        {
            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                if (entity.Components.TryGetValue(typeof(T0), out AbstractComponent component0)
                    && entity.Components.TryGetValue(typeof(T1), out AbstractComponent component1)
                    && entity.Components.TryGetValue(typeof(T2), out AbstractComponent component2)
                    && entity.Components.TryGetValue(typeof(T3), out AbstractComponent component3))
                {
                    return ((T0)component0, (T1)component1, (T2)component2, (T3)component3);
                }
            }
            return (null, null, null, null);
        }

        public (T0, T1, T2, T3, T4) ReadComponent<T0, T1, T2, T3,T4>(Guid entityId, Action<T0, T1, T2, T3,T4> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
            where T4 : AbstractComponent
        {
            if (m_CurrentFrameData!=null&&m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                if (entity.Components.TryGetValue(typeof(T0), out AbstractComponent component0)
                    && entity.Components.TryGetValue(typeof(T1), out AbstractComponent component1)
                    && entity.Components.TryGetValue(typeof(T2), out AbstractComponent component2)
                    && entity.Components.TryGetValue(typeof(T3), out AbstractComponent component3)
                    && entity.Components.TryGetValue(typeof(T4), out AbstractComponent component4))
                {
                    return ((T0)component0, (T1)component1, (T2)component2, (T3)component3, (T4)component4);
                }
            }
            return (null, null, null, null, null);
        }
        public (T0, T1, T2, T3, T4, T5) ReadComponent<T0, T1, T2, T3, T4,T5>(Guid entityId, Action<T0, T1, T2, T3, T4,T5> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
            where T4 : AbstractComponent where T5 : AbstractComponent
        {
            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                if (entity.Components.TryGetValue(typeof(T0), out AbstractComponent component0)
                    && entity.Components.TryGetValue(typeof(T1), out AbstractComponent component1)
                    && entity.Components.TryGetValue(typeof(T2), out AbstractComponent component2)
                    && entity.Components.TryGetValue(typeof(T3), out AbstractComponent component3)
                    && entity.Components.TryGetValue(typeof(T4), out AbstractComponent component4)
                    && entity.Components.TryGetValue(typeof(T5), out AbstractComponent component5))
                {
                    return ((T0)component0, (T1)component1, (T2)component2, (T3)component3, (T4)component4,(T5)component5);
                }
            }
            return (null, null, null, null, null, null);
        }

        public (T0, T1, T2, T3, T4, T5, T6) ReadComponent<T0, T1, T2, T3, T4, T5,T6>(Guid entityId, Action<T0, T1, T2, T3, T4, T5,T6> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
            where T4 : AbstractComponent where T5 : AbstractComponent
            where T6 : AbstractComponent
        {

            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                if (entity.Components.TryGetValue(typeof(T0), out AbstractComponent component0)
                    && entity.Components.TryGetValue(typeof(T1), out AbstractComponent component1)
                    && entity.Components.TryGetValue(typeof(T2), out AbstractComponent component2)
                    && entity.Components.TryGetValue(typeof(T3), out AbstractComponent component3)
                    && entity.Components.TryGetValue(typeof(T4), out AbstractComponent component4)
                    && entity.Components.TryGetValue(typeof(T5), out AbstractComponent component5)
                    && entity.Components.TryGetValue(typeof(T6), out AbstractComponent component6))
                {
                    return ((T0)component0, (T1)component1, (T2)component2, (T3)component3, (T4)component4, (T5)component5,(T6)component6);
                }
            }
            return (null, null, null, null, null, null, null);
        }

        public (T0, T1, T2, T3, T4, T5, T6, T7) ReadComponent<T0, T1, T2, T3, T4, T5, T6,T7>(Guid entityId, Action<T0, T1, T2, T3, T4, T5, T6,T7> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
            where T4 : AbstractComponent where T5 : AbstractComponent
            where T6 : AbstractComponent where T7 : AbstractComponent
        {
            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                if (entity.Components.TryGetValue(typeof(T0), out AbstractComponent component0)
                    && entity.Components.TryGetValue(typeof(T1), out AbstractComponent component1)
                    && entity.Components.TryGetValue(typeof(T2), out AbstractComponent component2)
                    && entity.Components.TryGetValue(typeof(T3), out AbstractComponent component3)
                    && entity.Components.TryGetValue(typeof(T4), out AbstractComponent component4)
                    && entity.Components.TryGetValue(typeof(T5), out AbstractComponent component5)
                    && entity.Components.TryGetValue(typeof(T6), out AbstractComponent component6)
                    && entity.Components.TryGetValue(typeof(T7), out AbstractComponent component7))
                {
                    return ((T0)component0, (T1)component1, (T2)component2, (T3)component3, (T4)component4, (T5)component5, (T6)component6,(T7)component7);
                }
            }
            return (null, null, null, null, null, null, null, null);
        }

        public AbstractComponent[] ReadComponent(Guid entityId, params Type[] componentTypes)
        {
            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict.TryGetValue(entityId, out Entity entity))
            {
                AbstractComponent[] components = new AbstractComponent[componentTypes.Length];
                for (int i = 0; i < componentTypes.Length; ++i)
                {
                    if(entity.Components.TryGetValue(components[i].GetType(), out AbstractComponent component))
                    {
                        components[i] = component;
                    }
                }
                return components;
            }
            return null;
        }
        #endregion

        public void RetrieveComponents<T>(List<EntityComponent> rets) where T : AbstractComponent
        {
            rets.Clear();
            Type componentType = typeof(T);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                foreach (Type comType in entity.Components.Keys)
                {
                    if (componentType == comType)
                        rets.Add(new EntityComponent(entity.Id, entity.Components[comType]));
                }
            }
        }

        #region Foreach
        public void ForEach<T0>(Action<Guid, T0> action)
            where T0 : AbstractComponent
        {
            if (m_CurrentFrameData == null) return;
            Type componentType0 = typeof(T0);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component))
                    action.Invoke(entity.Id, (T0)component);
            }
        }

        public void ForEach<T0, T1>(Action<Guid, T0, T1> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
        {
            if (m_CurrentFrameData == null) return;
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1))
                {
                    action.Invoke(entity.Id, (T0)component0, (T1)component1);
                }
            }
        }

        public void ForEach<T0, T1, T2>(Action<Guid, T0, T1, T2> action)
            where T0 : AbstractComponent where T1 : AbstractComponent 
            where T2 : AbstractComponent
        {
            if (m_CurrentFrameData == null) return;
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            Type componentType2 = typeof(T2);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1)
                    && entity.Components.TryGetValue(componentType2, out AbstractComponent component2))
                {
                    action.Invoke(entity.Id, (T0)component0, (T1)component1, (T2)component2);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3>(Action<Guid, T0, T1, T2, T3> action)
            where T0 : AbstractComponent where T1 : AbstractComponent 
            where T2 : AbstractComponent where T3 : AbstractComponent
        {
            if (m_CurrentFrameData == null) return;
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            Type componentType2 = typeof(T2);
            Type componentType3 = typeof(T3);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1)
                    && entity.Components.TryGetValue(componentType2, out AbstractComponent component2)
                    && entity.Components.TryGetValue(componentType3, out AbstractComponent component3))
                {
                    action.Invoke(entity.Id, (T0)component0, (T1)component1, (T2)component2, (T3)component3);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3, T4>(Action<Guid, T0, T1, T2, T3, T4> action)
            where T0 : AbstractComponent where T1 : AbstractComponent 
            where T2 : AbstractComponent where T3 : AbstractComponent 
            where T4 : AbstractComponent
        {
            if (m_CurrentFrameData == null) return;
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            Type componentType2 = typeof(T2);
            Type componentType3 = typeof(T3);
            Type componentType4 = typeof(T4);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1)
                    && entity.Components.TryGetValue(componentType2, out AbstractComponent component2)
                    && entity.Components.TryGetValue(componentType3, out AbstractComponent component3)
                    && entity.Components.TryGetValue(componentType4, out AbstractComponent component4))
                {
                    action.Invoke(entity.Id, (T0)component0, (T1)component1, (T2)component2, 
                        (T3)component3, (T4)component4);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3, T4,T5>(Action<Guid, T0, T1, T2, T3, T4,T5> action)
            where T0 : AbstractComponent where T1 : AbstractComponent 
            where T2 : AbstractComponent where T3 : AbstractComponent 
            where T4 : AbstractComponent where T5 : AbstractComponent
        {
            if (m_CurrentFrameData == null) return;
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            Type componentType2 = typeof(T2);
            Type componentType3 = typeof(T3);
            Type componentType4 = typeof(T4);
            Type componentType5 = typeof(T5);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1)
                    && entity.Components.TryGetValue(componentType2, out AbstractComponent component2)
                    && entity.Components.TryGetValue(componentType3, out AbstractComponent component3)
                    && entity.Components.TryGetValue(componentType4, out AbstractComponent component4)
                    && entity.Components.TryGetValue(componentType5, out AbstractComponent component5))
                {
                    action.Invoke(entity.Id, (T0)component0, (T1)component1, (T2)component2, 
                        (T3)component3, (T4)component4,(T5)component5);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3, T4, T5,T6>(Action<Guid, T0, T1, T2, T3, T4, T5,T6> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
            where T4 : AbstractComponent where T5 : AbstractComponent
            where T6 : AbstractComponent
        {
            if (m_CurrentFrameData == null) return;
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            Type componentType2 = typeof(T2);
            Type componentType3 = typeof(T3);
            Type componentType4 = typeof(T4);
            Type componentType5 = typeof(T5);
            Type componentType6 = typeof(T6);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1)
                    && entity.Components.TryGetValue(componentType2, out AbstractComponent component2)
                    && entity.Components.TryGetValue(componentType3, out AbstractComponent component3)
                    && entity.Components.TryGetValue(componentType4, out AbstractComponent component4)
                    && entity.Components.TryGetValue(componentType5, out AbstractComponent component5)
                    && entity.Components.TryGetValue(componentType6, out AbstractComponent component6))
                {
                    action.Invoke(entity.Id, (T0)component0, (T1)component1, (T2)component2, 
                        (T3)component3, (T4)component4, (T5)component5,(T6)component6);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3, T4, T5, T6,T7>(Action<Guid, T0, T1, T2, T3, T4, T5, T6,T7> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
            where T4 : AbstractComponent where T5 : AbstractComponent
            where T6 : AbstractComponent where T7 : AbstractComponent
        {
            if (m_CurrentFrameData == null) return;
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            Type componentType2 = typeof(T2);
            Type componentType3 = typeof(T3);
            Type componentType4 = typeof(T4);
            Type componentType5 = typeof(T5);
            Type componentType6 = typeof(T6);
            Type componentType7 = typeof(T7);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1)
                    && entity.Components.TryGetValue(componentType2, out AbstractComponent component2)
                    && entity.Components.TryGetValue(componentType3, out AbstractComponent component3)
                    && entity.Components.TryGetValue(componentType4, out AbstractComponent component4)
                    && entity.Components.TryGetValue(componentType5, out AbstractComponent component5)
                    && entity.Components.TryGetValue(componentType6, out AbstractComponent component6)
                    && entity.Components.TryGetValue(componentType7, out AbstractComponent component7))
                {
                    action.Invoke(entity.Id, (T0)component0, (T1)component1, (T2)component2,
                        (T3)component3, (T4)component4, (T5)component5, (T6)component6,
                        (T7)component7);
                }
            }
        }

        public void ForEach(Action<Guid, AbstractComponent[]> action, params Type[] componentTypes)
        {
            if(m_CurrentFrameData == null) return;
            AbstractComponent[] components = new AbstractComponent[componentTypes.Length];
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                bool allComponentsExist = true;
                for (int i = 0; i < componentTypes.Length; i++)
                {
                    if (entity.Components.TryGetValue(componentTypes[i], out AbstractComponent component))
                    {
                        components[i] = component;
                    }
                    else
                    {
                        allComponentsExist = false;
                        break;
                    }
                }

                if (allComponentsExist)
                {
                    action.Invoke(entity.Id, components);
                }
            }
        }
        #endregion

        public FrameRawData CloneFrameData()
        {
            FrameRawData frameRawData = new FrameRawData();
            foreach(Guid id in m_CurrentFrameData.EntityDict.Keys)
                frameRawData.EntityDict[id] = m_CurrentFrameData.EntityDict[id].Clone();
            return frameRawData;
        }

        public void RestoreWorld(FrameRawData rawData)
        {
            m_CurrentFrameData.EntityDict = rawData.EntityDict;
        }

        public void RestoreFrames(List<PtFrame> frames)
        {
            foreach (PtFrame frame in frames)
            {
                if (frame.HasUpdaters())
                {
                    foreach (PtComponentUpdater updater in frame.Updaters.Elements)
                    {
                        Entity entity = GetEntity(new Guid(frame.EntityId));
                        if (entity != null)
                        {
                            Type type = Type.GetType(updater.ComponentClsName);
                            if (type != null)
                                entity.GetComponent(type)?.UpdateParams(updater.ParamContent);
                        }
                    }
                }
                if (frame.HasNewEntitiesRaw())
                {
                    m_EntityInitializer.CreateEntities(frame.EntityId, frame.NewEntitiesRaw);
                }
            }
        }
        public void RestoreFrames(PtFrames frames)
        {
            RestoreFrames(frames.KeyFrames);
        }
        public void RollBack(FrameRawData rawData,PtFrames frames)
        {
            RestoreWorld(rawData);
            RestoreFrames(frames);
        }

        public void Dispose()
        {
            IsActive = false;
            m_EntityRenderSpawner = null;
            m_CurrentFrameData = null;
            m_EntityInitializer = null;
        }

        public override string ToString()
        {
            int entityCount = 0;
            if (m_CurrentFrameData != null && m_CurrentFrameData.EntityDict != null)
                entityCount = m_CurrentFrameData.EntityDict.Count;
            return $"Entity Count:{entityCount}";
        }
    }
}
