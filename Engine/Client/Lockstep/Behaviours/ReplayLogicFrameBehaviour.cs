using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Lockstep.Behaviours
{
    public class ReplayLogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        public int CurrentFrameIdx { private set; get; }
        List<List<PtFrame>> m_Frames;
        public void Start()
        {
            CurrentFrameIdx = -1;
        }
        public void SetFrameIdxInfos(List<List<PtFrame>> infos)
        {
            m_Frames = infos;
        }
        public List<PtFrame> GetFrameIdxInfoAtCurrentFrame()
        {
            if (CurrentFrameIdx < m_Frames.Count)
                return m_Frames[CurrentFrameIdx];
            return null;
        }
        public void Stop()
        {
            
        }

        public void Update()
        {
            ++CurrentFrameIdx;
        }
    }
}
