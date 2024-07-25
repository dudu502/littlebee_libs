using Engine.Client.Lockstep.Behaviours.Data;
using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;

namespace Engine.Client.Lockstep.Behaviours
{
    public class InputBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        BattleServiceModule battleServiceModule;
        LogicFrameBehaviour logicFrameBehaviour;
        public void Start()
        {
            logicFrameBehaviour = Sim.GetBehaviour<LogicFrameBehaviour>();
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
        }

        public void Stop()
        {
            
        }

        public void Update()
        {
            while (Input.InputFrames.TryDequeue(out PtFrame frame))
            {
                battleServiceModule.GetRoomSession().AddCurrentFrameCommand(logicFrameBehaviour.CurrentFrameIdx, frame);
            }
        }
    }
}
