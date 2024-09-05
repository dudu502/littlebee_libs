using Engine.Common.Event;
using Engine.Common.Lockstep;
using Engine.Common.Misc;
using Engine.Common.Protocol.Pt;
using System.Collections.Generic;
namespace Engine.Client.Lockstep.Behaviours
{
    public class ReplayLogicFrameBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        private int FrameIdx;
        List<List<PtFrame>> m_Frames;
        public void Start()
        {
            FrameIdx = -1;
        }
        public void SetFrameIdxInfos(List<List<PtFrame>> infos)
        {
            m_Frames = infos;
        }
        public override string ToString()
        {
            return $"{nameof(ReplayLogicFrameBehaviour)} FrameIdx:{FrameIdx}";
        }
        public List<PtFrame> GetFrameIdxInfoAtCurrentFrame()
        {
            if (FrameIdx < m_Frames.Count)
                return m_Frames[FrameIdx];
            return null;
        }
        public void Stop()
        {
            m_Frames = null;
        }

        public void Update()
        {
            if (FrameIdx < m_Frames.Count)
            {
                ++FrameIdx;
                if (FrameIdx == m_Frames.Count)
                    EventDispatcher<SimulationEventId, int>.DispatchEvent(SimulationEventId.TheLastFrameHasBeenPlayed, FrameIdx);
            }
        }
    }
}
