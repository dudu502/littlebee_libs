using Engine.Client.Ecsr.Components;
using Engine.Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Ecsr.Entitas
{
    public class Entity
    {
        public Guid Id { private set; get; }
        public readonly Dictionary<Type, AbstractComponent> Components = new Dictionary<Type, AbstractComponent>();
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Entity(Guid id)
        {
            Id = id;
        }
        public AbstractComponent GetComponent(Type type)
        {
            if (Components.TryGetValue(type, out AbstractComponent component))
                return component;
            return null;
        }
   
        public AbstractComponent GetComponent<T>()where T : AbstractComponent
        {
            Type type = typeof(T);
            return GetComponent(type);
        }
        public static Entity Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                string entityId = buffer.ReadString();
                Entity entity = new Entity(new Guid(entityId));
                int count = buffer.ReadInt32();
                for(int i = 0; i < count; ++i)
                {
                    Type componentType = Type.GetType(buffer.ReadString());
                    AbstractComponent componentInstance = Activator.CreateInstance(componentType) as AbstractComponent;
                    if (componentInstance != null)
                    {
                        componentInstance.Deserialize(buffer.ReadBytes());
                        entity.AddComponent(componentInstance);
                    }
                }
                return entity;
            }
        }
        public static byte[] Write(Entity entity)
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteString(entity.Id.ToString());
                buffer.WriteInt32(entity.Components.Count);
                foreach(Type componentType in entity.Components.Keys)
                {
                    buffer.WriteString(componentType.AssemblyQualifiedName);
                    buffer.WriteBytes(entity.Components[componentType].Serialize());
                }
                return buffer.GetRawBytes();
            }
        }
        public Entity Clone()
        {
            Entity entity = new Entity(Id);
            foreach(var component in Components.Values)
                entity.AddComponent(component.Clone());
            return entity;
        }
        public Entity AddComponent(AbstractComponent component)
        {
            Components[component.GetType()] = component;
            return this;
        }

        public bool RemoveComponent(AbstractComponent component)
        {
            return Components.Remove(component.GetType());
        }
    }
}
