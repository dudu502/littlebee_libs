using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Systems;
using Engine.Client.Lockstep.Behaviours;
using Engine.Common.Lockstep;

namespace Engine.Client.Lockstep
{
    public class DefaultReplaySimulationController:SimulationController
    {
        public void CreateSimulation(Simulation sim, EntityWorld world, ISimulativeBehaviour[] behaviours, IEntitySystem[] systems)
        {
            base.CreateSimulation(sim, behaviours);
            GetSimulation<DefaultSimulation>().SetEntityWorld(world);
            for (int i = 0; i < systems.Length; ++i)
            {
                sim.GetBehaviour<EntityBehaviour>().AddSystem(systems[i]);
            }
        }
    }
}
