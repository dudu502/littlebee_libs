using Engine.Common.Lockstep;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Server.Lockstep.Behaviours
{
    public class ServerLogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        int m_CurrentFrameIdx;
        public void Start()
        {
            m_CurrentFrameIdx = -1;
        }

        public void Stop()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
