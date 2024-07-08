using Engine.Client.Ecsr.Components;
using Engine.Common.Event;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;

namespace Engine.Client.Ecsr.Entitas
{
    public class EntityWorld : IDisposable
    {
        public class FrameRawData
        {
            public static readonly List<Type> ComponentTypes = new List<Type>();
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
        public EntityWorld(params Type[] comps)
        {
            FrameRawData.ComponentTypes.AddRange(comps);
            m_CurrentFrameData = new FrameRawData();
            m_EntityInitializer = new EntityInitializer(this);
        }
        public int GetEntityCount()
        {
            return m_CurrentFrameData.EntityDict.Count;
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


        #region Foreach
        public void ForEach<T0>(Action<Entity, T0> action)
            where T0 : AbstractComponent
        {
            Type componentType0 = typeof(T0);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component))
                    action.Invoke(entity, (T0)component);
            }
        }

        public void ForEach<T0, T1>(Action<Entity, T0, T1> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
        {
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1))
                {
                    action.Invoke(entity, (T0)component0, (T1)component1);
                }
            }
        }

        public void ForEach<T0, T1, T2>(Action<Entity, T0, T1, T2> action)
            where T0 : AbstractComponent where T1 : AbstractComponent 
            where T2 : AbstractComponent
        {
            Type componentType0 = typeof(T0);
            Type componentType1 = typeof(T1);
            Type componentType2 = typeof(T2);
            foreach (Entity entity in m_CurrentFrameData.EntityDict.Values)
            {
                if (entity.Components.TryGetValue(componentType0, out AbstractComponent component0)
                    && entity.Components.TryGetValue(componentType1, out AbstractComponent component1)
                    && entity.Components.TryGetValue(componentType2, out AbstractComponent component2))
                {
                    action.Invoke(entity, (T0)component0, (T1)component1, (T2)component2);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3>(Action<Entity, T0, T1, T2, T3> action)
            where T0 : AbstractComponent where T1 : AbstractComponent 
            where T2 : AbstractComponent where T3 : AbstractComponent
        {
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
                    action.Invoke(entity, (T0)component0, (T1)component1, (T2)component2, (T3)component3);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3, T4>(Action<Entity, T0, T1, T2, T3, T4> action)
            where T0 : AbstractComponent where T1 : AbstractComponent 
            where T2 : AbstractComponent where T3 : AbstractComponent 
            where T4 : AbstractComponent
        {
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
                    action.Invoke(entity, (T0)component0, (T1)component1, (T2)component2, 
                        (T3)component3, (T4)component4);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3, T4,T5>(Action<Entity, T0, T1, T2, T3, T4,T5> action)
            where T0 : AbstractComponent where T1 : AbstractComponent 
            where T2 : AbstractComponent where T3 : AbstractComponent 
            where T4 : AbstractComponent where T5 : AbstractComponent
        {
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
                    action.Invoke(entity, (T0)component0, (T1)component1, (T2)component2, 
                        (T3)component3, (T4)component4,(T5)component5);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3, T4, T5,T6>(Action<Entity, T0, T1, T2, T3, T4, T5,T6> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
            where T4 : AbstractComponent where T5 : AbstractComponent
            where T6 : AbstractComponent
        {
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
                    action.Invoke(entity, (T0)component0, (T1)component1, (T2)component2, 
                        (T3)component3, (T4)component4, (T5)component5,(T6)component6);
                }
            }
        }

        public void ForEach<T0, T1, T2, T3, T4, T5, T6,T7>(Action<Entity, T0, T1, T2, T3, T4, T5, T6,T7> action)
            where T0 : AbstractComponent where T1 : AbstractComponent
            where T2 : AbstractComponent where T3 : AbstractComponent
            where T4 : AbstractComponent where T5 : AbstractComponent
            where T6 : AbstractComponent where T7 : AbstractComponent
        {
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
                    action.Invoke(entity, (T0)component0, (T1)component1, (T2)component2,
                        (T3)component3, (T4)component4, (T5)component5, (T6)component6,
                        (T7)component7);
                }
            }
        }

        public void ForEach(Action<Entity, AbstractComponent[]> action, params Type[] componentTypes)
        {
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
                    action.Invoke(entity, components);
                }
            }
        }
        #endregion






        //public List<AbstractComponent> GetAllCloneComponents()
        //{
        //    List<AbstractComponent> components = new List<AbstractComponent>();
        //    int size = m_FrameData.Components.Count;
        //    for (int i = 0; i < size; ++i)
        //        components.Add(m_FrameData.Components[i].Clone());
        //    return components;
        //}

        //public void RestoreWorld(FrameRawData rawData)
        //{
        //    m_FrameData.Components = rawData.Components;
        //    m_FrameData.EntityComponents = rawData.EntityComponents;
        //    m_FrameData.TypeComponents = rawData.TypeComponents;
        //}
        /// <summary>
        /// 获取关键帧数据并且复原整个世界
        /// </summary>
        /// <param name="collection"></param>
        public void RestoreKeyframes(PtFrames collection)
        {
            for (int i = 0; i < collection.KeyFrames.Count; ++i)
            {
                var info = collection.KeyFrames[i];

                switch (info.Cmd)
                {
                    //case FrameCommand.SYNC_MOVE:
                    //    AbstractComponent component = GetComponentByEntityId(info.EntityId, info.Cmd);
                    //    component?.UpdateParams(info.ParamContent);
                    //    break;
                    //case FrameCommand.SYNC_CREATE_ENTITY:
                    //    EntityManager.CreateEntityBySyncFrame(this, info);
                    //    break;
                }


            }
            //Need sort
        }

        //public void RollBack(FrameRawData data, PtFrames collection)
        //{
        //    m_FrameData.Components = data.Components;
        //    m_FrameData.EntityComponents = data.EntityComponents;
        //    m_FrameData.TypeComponents = data.TypeComponents;

        //    for (int i = 0; i < collection.KeyFrames.Count; ++i)
        //    {
        //        var info = collection.KeyFrames[i];
        //        switch (info.Cmd)
        //        {
        //            //case FrameCommand.SYNC_CREATE_ENTITY:
        //            //    EntityManager.CreateEntityBySyncFrame(this, info);
        //            //    break;
        //            //case FrameCommand.SYNC_MOVE:
        //            //    AbstractComponent component = GetComponentByEntityId(info.EntityId, info.Cmd);
        //            //    component?.UpdateParams(info.ParamContent);
        //            //    break;
        //        }

        //    }

        //    //Need sort
        //}
        public void Dispose()
        {
            
        }
    }
}
