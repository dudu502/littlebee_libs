using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Systems;

public class FSMSystem : IEntitySystem
{
    public EntityWorld World { set; get; }

    public void Execute()
    {
        World.ForEach<FSMInfo>((id, fsmInfo) =>
        {
            FSM componentBasedFsm = FSMFactory.Retrieve(World, fsmInfo.InfoType);
            componentBasedFsm.EntityId = id;
            componentBasedFsm.Info = fsmInfo;
            componentBasedFsm.Execute();         
        });
    }
}
