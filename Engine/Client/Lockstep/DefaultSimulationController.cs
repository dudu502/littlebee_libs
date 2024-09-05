using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Systems;
using Engine.Client.Lockstep.Behaviours;
using Engine.Common.Lockstep;
using System.Threading;
using System;
using System.Collections.Concurrent;
using Engine.Common.Protocol.Pt;
using Engine.Client.Modules;
using Engine.Common;

namespace Engine.Client.Lockstep
{
    public class DefaultSimulationController:SimulationController
    {
        
        public void CreateSimulation(Simulation sim, EntityWorld world,ISimulativeBehaviour[] behaviours,
            IEntitySystem[] systems)
        {
            base.CreateSimulation(sim, behaviours);
            GetSimulation<DefaultSimulation>().SetEntityWorld(world);
            for (int i = 0; i < systems.Length; ++i)
            {
                sim.GetBehaviour<EntityBehaviour>().AddSystem(systems[i]);
            }
        }

        protected override void Run(object runner)
        {
            Action action = runner as Action;
            BattleServiceModule battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
            while (State == RunState.Running)
            {
                while(battleServiceModule.HasKeyFrames())
                {
                    m_SimulationInstance.Run();
                    Thread.Sleep(10);
                    if (action != null)
                    {
                        action();
                        action = null;
                    }
                }  
            }
            State = RunState.Stopped;
        }
    }
}
