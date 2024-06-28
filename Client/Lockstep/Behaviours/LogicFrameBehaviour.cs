using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Lockstep.Behaviours
{
    public class LogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }

        public int CurrentFrameIdx { private set; get; }

        List<List<PtFrame>> m_Frames;
        List<PtFrame> m_DefaultFrame = new List<PtFrame>();

        public LogicFrameBehaviour() { }

        public List<List<PtFrame>> GetFrames() { return m_Frames; }
        public void Start()
        {
            m_Frames = new List<List<PtFrame>>();

        }

        public void Stop()
        {
            
        }

        public void Update()
        {
            ++CurrentFrameIdx;
            m_Frames.Add(m_DefaultFrame);
        }
    }
}
