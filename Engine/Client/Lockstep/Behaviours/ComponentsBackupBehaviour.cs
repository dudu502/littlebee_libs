using Engine.Client.Ecsr.Entitas;
using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System.Collections.Generic;

namespace Engine.Client.Lockstep.Behaviours
{
    public class ComponentsBackupBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        const int BackupEntityWorldFrameLength = 128;
        LogicFrameBehaviour logicFrameBehaviour;
        BattleServiceModule battleServiceModule;
        Dictionary<int, EntityWorld.FrameRawData> dictFrameData;
        EntityWorld world;
        public void Start()
        {
            world = ((DefaultSimulation)Sim).GetEntityWorld();
            dictFrameData = new Dictionary<int, EntityWorld.FrameRawData>();
            logicFrameBehaviour = Sim.GetBehaviour<LogicFrameBehaviour>();
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
        }

        public EntityWorld.FrameRawData GetFrameData(int frameIdx) 
        {
            if (dictFrameData.TryGetValue(frameIdx, out EntityWorld.FrameRawData frameData))
                return frameData;
            return null;
        }
        public void SetFrameData(int frameIdx, EntityWorld.FrameRawData frameData)
        {
            dictFrameData[frameIdx] = frameData;
        }
        public Dictionary<int,EntityWorld.FrameRawData> GetFrameData()
        {
            return dictFrameData;
        }
        public void Stop()
        {
            if(dictFrameData != null)
                dictFrameData.Clear();
            dictFrameData = null;
        }
        void SendFrame(int frameIdx)
        {
            PtFrames frames = battleServiceModule.GetRoomSession().GetKeyFrameCached();
            if (frames.KeyFrames.Count > 0)
            {
                battleServiceModule.RequestSyncClientKeyframes(frameIdx, frames);
                battleServiceModule.GetRoomSession().ClearKeyFrameCached();
            }
        }
        void ClearFrameDataAt(int frameIdx)
        {
            dictFrameData.Remove(frameIdx - BackupEntityWorldFrameLength);
        }
        public void Update()
        {
            int frameIdx = logicFrameBehaviour.CurrentFrameIdx;
            SetFrameData(frameIdx, world.CloneFrameData());
            SendFrame(frameIdx);
            ClearFrameDataAt(frameIdx);
        }
        public override string ToString()
        {
            return $"{nameof(ComponentsBackupBehaviour)}";
        }
    }
}
