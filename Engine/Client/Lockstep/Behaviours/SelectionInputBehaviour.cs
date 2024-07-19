using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Lockstep.Behaviours
{
    public class SelectionInputBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        LogicFrameBehaviour logicBehaviour;
        BattleServiceModule battleServiceModule;
        ComponentsBackupBehaviour componentsBackupBehaviour;
        public void Start()
        {
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
            logicBehaviour = Sim.GetBehaviour<LogicFrameBehaviour>();
            componentsBackupBehaviour = Sim.GetBehaviour<ComponentsBackupBehaviour>();
        }

        public void Stop()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
