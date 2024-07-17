using Engine.Client.Lockstep.Behaviours.Data;
using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;
using Engine.Common.Misc;
using System;

namespace Engine.Client.Lockstep.Behaviours
{
    public class FunctionButtonInputBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        LogicFrameBehaviour logicFrameBehaviour;
        BattleServiceModule battleServiceModule;
        Guid currentEntityId;
        ButtonInputRecord currentRecord;
        ButtonInputRecord record;
        public void Start()
        {
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
            currentEntityId = new Guid(battleServiceModule.GetRoomSession().EntityId);
            logicFrameBehaviour = Sim.GetBehaviour<LogicFrameBehaviour>();
            currentRecord = new ButtonInputRecord();
            currentRecord.EntityId = currentEntityId;
            record = new ButtonInputRecord();
            record.EntityId = currentEntityId;
        }

        public void Stop()
        {
            
        }

        public void Update()
        {
            var func = ButtonInputRecord.CurrentFunc;
            if(func == ButtonFunction.Fire)
            {
                ButtonInputRecord.CurrentFunc = ButtonFunction.None;
                battleServiceModule.GetRoomSession().AddCurrentFrameCommand(logicFrameBehaviour.CurrentFrameIdx,
                    FrameCommand.SYNC_CREATE_ENTITY, currentEntityId.ToString(), new byte[] { 0 });
            }
        }
    }
}
