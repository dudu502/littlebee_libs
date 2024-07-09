using Engine.Client.Ecsr.Entitas;
using Engine.Common.Protocol.Pt;

namespace Engine.Client.Lockstep.Behaviours
{
    public class RollbackBehaviour : EntityBehaviour
    {
        ComponentsBackupBehaviour backupBehaviour;
        public override void Start()
        {
            base.Start();
            backupBehaviour = Sim.GetBehaviour<ComponentsBackupBehaviour>();
        }

        public override void Update()
        {
            var roomSession = battleServiceModule.GetRoomSession();
            if (roomSession.DictKeyFrames != null) return;
            while (roomSession.QueueKeyFrames.Count > 0)
            {
                bool rollState = false;
                if(roomSession.QueueKeyFrames.TryPeek(out PtFrames frames) && frames.FrameIdx < logicBehaviour.CurrentFrameIdx)
                {
                    if (roomSession.QueueKeyFrames.TryDequeue(out PtFrames keyFrames))
                        rollState = RollImpl(keyFrames);
                    else
                        break;
                }
                else
                {
                    break;
                }
                if(!rollState)
                {
                    break;
                }
            }
        }

        bool RollImpl(PtFrames keyFrames)
        {
            if (keyFrames == null || keyFrames.FrameIdx == 0) return false;
            int frameIdx = keyFrames.FrameIdx;
            logicBehaviour.UpdateKeyFrameIdxInfoCollectionAtFrameIdx(keyFrames);

            EntityWorld.FrameRawData frawPrevRawData = backupBehaviour.GetFrameData(frameIdx - 1);
            if(frawPrevRawData != null )
            {
                simulation.GetEntityWorld().RollBack(frawPrevRawData, keyFrames);

                while (frameIdx < logicBehaviour.CurrentFrameIdx)
                {
                    base.Update();
                    backupBehaviour.SetFrameData(frameIdx++, simulation.GetEntityWorld().CloneFrameData());
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
