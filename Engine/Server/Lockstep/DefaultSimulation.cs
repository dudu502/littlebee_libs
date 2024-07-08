using Engine.Common.Lockstep;

namespace Engine.Server.Lockstep
{
    public class DefaultSimulation : Simulation
    {
        public DefaultSimulation(byte id) : base(id)
        {
            
        }

        protected override void CreateEntities()
        {
            base.CreateEntities();
        }
    }
}
