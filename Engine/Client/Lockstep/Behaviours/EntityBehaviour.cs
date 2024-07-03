using Engine.Client.Ecsr.Systems;
using Engine.Common.Lockstep;
using System;
using System.Collections.Generic;

namespace Engine.Client.Lockstep.Behaviours
{
    public class EntityBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }

        public List<IEntitySystem> Systems = new List<IEntitySystem>();
        public virtual void Start()
        {
            
        }

        public bool HasSystem(IEntitySystem system)
        {
            foreach(IEntitySystem item in Systems)
            {
                if(item == system) return true;
                if(item.GetType() == system.GetType()) return true;
            }
            return false;
        }

        public EntityBehaviour AddSystem(IEntitySystem system)
        {
            if(!HasSystem(system))
            {
                Systems.Add(system);
            }
            return this;
        }
        public virtual void Stop()
        {
            Systems.Clear();
        }

        public virtual void Update()
        {
            int systemCount = Systems.Count;
            for (int i = 0; i < systemCount; ++i)
                Systems[i].Execute();
        }
    }
}
