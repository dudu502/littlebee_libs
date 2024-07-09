using Engine.Common;
using Engine.Common.Lockstep;
using Engine.Server.Modules;
namespace Engine.Server.Lockstep.Behaviours
{
    public class ServerLogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        int m_CurrentFrameIdx;
        BattleModule battleModule;
        public void Start()
        {
            battleModule = Context.Retrieve(Context.SERVER).GetModule<BattleModule>();
            m_CurrentFrameIdx = -1;
        }

        public void Stop()
        {
            battleModule = null;
        }

        public void Update()
        {
            // flush the syncframe_data to all clients at same frame.
            battleModule.FlushKeyFrame(++m_CurrentFrameIdx);
        }
    }
}
