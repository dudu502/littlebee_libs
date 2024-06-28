using Engine.Client.Ecsr.Components;
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
