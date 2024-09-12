using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Client.Lockstep.Behaviours
{
    public class FrameReceiverBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { set; get; }
        BattleServiceModule battleServiceModule;
        DefaultSimulation defaultSimulation;
        List<List<PtFrame>> totalFrames;
        readonly List<PtFrame> defaultFrame = new List<PtFrame>();
        public int FrameIdx { get; private set; }
        public FrameReceiverBehaviour()
        {

        }
        public List<List<PtFrame>> GetFrames() { return totalFrames; }
        public void Start()
        {
            totalFrames = new List<List<PtFrame>>();
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
            defaultSimulation = Sim as DefaultSimulation;
        }

        public void Stop()
        {
            battleServiceModule = null;
        }
        public override string ToString()
        {
            return $"{nameof(FrameReceiverBehaviour)} FrameIdx:{FrameIdx}";
        }
        void RestoreAndAddFrames(PtFrames frames)
        {
            FrameIdx = frames.FrameIdx;
            defaultSimulation.GetEntityWorld().RestoreFrames(frames);
            totalFrames.Add(frames.KeyFrames ?? defaultFrame);
        }
        public void Update()
        {
            var roomSession = battleServiceModule.GetRoomSession();
            if (roomSession.HistoryFramesList != null &&
                roomSession.HistoryFramesList.TryDequeue(out var first))
            {
                RestoreAndAddFrames(first);

                if (roomSession.QueueKeyFrames.TryPeek(out var queueKeyFrame) && queueKeyFrame!=null && queueKeyFrame.FrameIdx==FrameIdx)
                {
                    roomSession.QueueKeyFrames.TryDequeue(out _);
                }
            }
            else if (roomSession.QueueKeyFrames.TryDequeue(out PtFrames frames))
            {
                RestoreAndAddFrames(frames);
            }
        }
    }
}
