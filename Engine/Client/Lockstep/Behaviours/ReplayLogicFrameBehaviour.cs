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
        public override string ToString()
        {
            return $"{nameof(ReplayLogicFrameBehaviour)} FrameIdx:{CurrentFrameIdx}";
        }
        public List<PtFrame> GetFrameIdxInfoAtCurrentFrame()
        {
            if (CurrentFrameIdx < m_Frames.Count)
                return m_Frames[CurrentFrameIdx];
            return null;
        }
        public void Stop()
        {
            m_Frames = null;
        }

        public void Update()
        {
            if (CurrentFrameIdx < m_Frames.Count)
            {
                ++CurrentFrameIdx;
                if (CurrentFrameIdx == m_Frames.Count)
                    EventDispatcher<SimulationEventId, int>.DispatchEvent(SimulationEventId.TheLastFrameHasBeenPlayed, CurrentFrameIdx);
            }
        }
    }
}
