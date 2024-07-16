using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System.Collections.Generic;

namespace Engine.Client.Lockstep.Behaviours
{
    public class ReplayInputBehaviour:ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        ReplayLogicFrameBehaviour replayLogic;
        public void Start()
        {
            replayLogic = Sim.GetBehaviour<ReplayLogicFrameBehaviour>();
        }

        public void Stop()
        {
                
        }

        public void Update()
        {
            List<PtFrame> frames = replayLogic.GetFrameIdxInfoAtCurrentFrame();
            if (frames != null)
            {
                foreach(PtFrame frame in frames)
                {
                    switch(frame.Cmd)
                    {
                        
                    }
                }
            }
            
        }
    }
}
