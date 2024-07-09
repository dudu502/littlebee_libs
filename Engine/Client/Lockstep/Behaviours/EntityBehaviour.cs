using Engine.Client.Ecsr.Systems;
using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;
using System;
using System.Collections.Generic;

namespace Engine.Client.Lockstep.Behaviours
{
    public class EntityBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }

        public List<IEntitySystem> Systems = new List<IEntitySystem>();
        protected LogicFrameBehaviour logicBehaviour;
        protected BattleServiceModule battleServiceModule; 
        protected DefaultSimulation simulation;
        public virtual void Start()
        {
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
            logicBehaviour = Sim.GetBehaviour<LogicFrameBehaviour>();
            simulation = (DefaultSimulation)Sim;
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
                system.World = simulation.GetEntityWorld();
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
