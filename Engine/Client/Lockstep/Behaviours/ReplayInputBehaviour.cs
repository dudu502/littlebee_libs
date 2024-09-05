using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System.Collections.Generic;

namespace Engine.Client.Lockstep.Behaviours
{
    public class ReplayInputBehaviour:ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        private DefaultSimulation defaultSimulation;
        private ReplayLogicFrameBehaviour replayLogic;
        public void Start()
        {
            replayLogic = Sim.GetBehaviour<ReplayLogicFrameBehaviour>();
            defaultSimulation = Sim as DefaultSimulation;
        }

        public void Stop()
        {
                
        }
        public override string ToString()
        {
            return $"{nameof(ReplayInputBehaviour)}";
        }
        public void Update()
        {
            List<PtFrame> frames = replayLogic.GetFrameIdxInfoAtCurrentFrame();
            defaultSimulation.GetEntityWorld().RestoreFrames(frames);
        }
    }
}
