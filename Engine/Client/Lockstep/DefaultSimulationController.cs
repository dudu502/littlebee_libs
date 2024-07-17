using Engine.Client.Ecsr.Systems;
using Engine.Client.Lockstep.Behaviours;
using Engine.Common.Lockstep;

namespace Engine.Client.Lockstep
{
    public class DefaultSimulationController:SimulationController
    {
        public override void CreateSimulation()
        {
            base.CreateSimulation();
            DefaultSimulation defaultSimulation = new DefaultSimulation();
            defaultSimulation.SetEntityWorld(new Ecsr.Entitas.EntityWorld());
            defaultSimulation.AddBehaviour(new LogicFrameBehaviour())
                            .AddBehaviour(new RollbackBehaviour())
                            .AddBehaviour(new EntityBehaviour())
                            .AddBehaviour(new FunctionButtonInputBehaviour())
                            .AddBehaviour(new ComponentsBackupBehaviour());

            AppearanceSystem appearanceSystem = new AppearanceSystem();
            FsmSystem fsmSystem = new FsmSystem();
            MovementSystem movementSystem = new MovementSystem();
            defaultSimulation.GetBehaviour<EntityBehaviour>()
                            .AddSystem(appearanceSystem)
                            .AddSystem(fsmSystem)
                            .AddSystem(movementSystem);
            defaultSimulation.GetBehaviour<RollbackBehaviour>()
                            .AddSystem(appearanceSystem)
                            .AddSystem(fsmSystem)
                            .AddSystem(movementSystem);
            m_SimulationInstance = defaultSimulation;
        }
    }
}
