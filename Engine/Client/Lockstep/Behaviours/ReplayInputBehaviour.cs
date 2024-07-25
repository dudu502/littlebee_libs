using Engine.Client.Ecsr.Entitas;
using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;

namespace Engine.Client.Lockstep.Behaviours
{
    public class ReplayInputBehaviour:ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        private DefaultSimulation DefaultSimulation;
        ReplayLogicFrameBehaviour replayLogic;
        public void Start()
        {
            replayLogic = Sim.GetBehaviour<ReplayLogicFrameBehaviour>();
            DefaultSimulation = Sim as DefaultSimulation;
        }

        public void Stop()
        {
                
        }

        public void Update()
        {
            List<PtFrame> frames = replayLogic.GetFrameIdxInfoAtCurrentFrame();
            if(frames != null)
                DefaultSimulation.GetEntityWorld().RestoreFrames(frames);
        }
    }
}
