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
        internal Entity()
        {
            Id = Guid.NewGuid();
        }

        internal Entity(Guid id)
        {
            Id = id;
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
