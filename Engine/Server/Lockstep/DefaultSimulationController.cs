using Engine.Common.Lockstep;
using Engine.Server.Lockstep.Behaviours;

namespace Engine.Server.Lockstep
{
    public class DefaultSimulationController:SimulationController
    {
        public override void CreateSimulation()
        {
            base.CreateSimulation();
            DefaultSimulation defaultSimulation = new DefaultSimulation(0);
            defaultSimulation.AddBehaviour(new ServerLogicFrameBehaviour());
            m_SimulationInstance = defaultSimulation;
        }
    }
}
