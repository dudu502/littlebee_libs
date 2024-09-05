using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System.Collections.Generic;

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
        public void Update()
        {
            if (battleServiceModule.GetRoomSession().QueueKeyFrames.TryDequeue(out PtFrames frames))
            {
                FrameIdx = frames.FrameIdx;
                defaultSimulation.GetEntityWorld().RestoreFrames(frames);
                totalFrames.Add(frames.KeyFrames??defaultFrame);
            }
        }
    }
}
