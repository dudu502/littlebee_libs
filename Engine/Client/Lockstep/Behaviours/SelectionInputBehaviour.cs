using Client.Lockstep.Behaviours.Data;
using Engine.Client.Lockstep.Behaviours.Data;
using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;

namespace Engine.Client.Lockstep.Behaviours
{
    public class SelectionInputBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        LogicFrameBehaviour logicBehaviour;
        BattleServiceModule battleServiceModule;

        public void Start()
        {
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
            logicBehaviour = Sim.GetBehaviour<LogicFrameBehaviour>();
        }

        public void Stop()
        {
            battleServiceModule = null;
            logicBehaviour = null;
        }

        public void Update()
        {
            if(Selection.SelectedIds.Count > 0)
            {
                while(Input.InputFrames.TryDequeue(out var frame))
                {
                    battleServiceModule.GetRoomSession().AddCurrentFrameCommand(logicBehaviour.CurrentFrameIdx,frame);
                }
            }
        }
    }
}
