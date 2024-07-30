using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;

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

        BattleServiceModule battleServiceModule;
        public void Start()
        {
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
            CurrentFrameIdx = battleServiceModule.GetRoomSession().InitIndex;
            m_Frames = new List<List<PtFrame>>();
            if (CurrentFrameIdx > -1)
            {
                for (int i = 0; i <= CurrentFrameIdx; ++i)
                {
                    m_Frames.Add(m_DefaultFrame);
                    if (battleServiceModule.GetRoomSession().DictKeyFrames != null && battleServiceModule.GetRoomSession().DictKeyFrames.TryGetValue(i, out PtFrames frames))
                    {
                        UpdateKeyFrameIdxInfoCollectionAtFrameIdx(frames);
                    }
                }
            }
        }

        public void UpdateKeyFrameIdxInfoCollectionAtFrameIdx(PtFrames frames)
        {
            foreach (PtFrame frame in frames.KeyFrames)
                UpdateKeyFrameIdxInfoAtFrameIdx(frames.FrameIdx, frame);
        }

        private void UpdateKeyFrameIdxInfoAtFrameIdx(int frameIdx, PtFrame frame)
        {
            if(frameIdx >= m_Frames.Count) 
                throw new Exception("Error " + frameIdx);
            if (m_Frames[frameIdx] == m_DefaultFrame)
                m_Frames[frameIdx] = new List<PtFrame>();
            List<PtFrame> frames = m_Frames[frameIdx];
            bool updateState = false;

            for(int i = 0; i < frames.Count; ++i)
            {
                if (frames[i].EntityId == frame.EntityId)
                {
                    updateState = true;
                    frames[i] = frame;
                    break;
                }
            }

            if (!updateState)
            {
                frames.Add(frame);
            }
        }
        public override string ToString()
        {
            return $"{nameof(LogicFrameBehaviour)} FrameIdx:{CurrentFrameIdx}";
        }
        public void Stop()
        {
            
        }

        public void Update()
        {
            ++CurrentFrameIdx;
            m_Frames.Add(m_DefaultFrame);

            if(battleServiceModule.GetRoomSession() != null
                && battleServiceModule.GetRoomSession().WriteKeyFrameIndex > CurrentFrameIdx)
            {
                if(battleServiceModule.GetRoomSession().DictKeyFrames.TryGetValue(CurrentFrameIdx,out PtFrames frames))
                {
                    ((DefaultSimulation)Sim).GetEntityWorld().RestoreFrames(frames);
                    UpdateKeyFrameIdxInfoCollectionAtFrameIdx(frames);
                }
                else
                {
                    battleServiceModule.GetRoomSession().WriteKeyFrameIndex = -1;
                    battleServiceModule.GetRoomSession().DictKeyFrames = null;
                }
            }
        }
    }
}
