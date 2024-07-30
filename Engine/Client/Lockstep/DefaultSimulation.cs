using Engine.Client.Ecsr.Entitas;
using Engine.Common.Lockstep;

namespace Engine.Client.Lockstep
{
    public class DefaultSimulation : Simulation
    {
        EntityWorld m_EntityWorld;

        public DefaultSimulation()
        {
        }
        public EntityWorld GetEntityWorld() 
        { 
            return m_EntityWorld; 
        }
        public void SetEntityWorld(EntityWorld world)
        {
            m_EntityWorld = world;
        }
        public override string ToString()
        {
            return base.ToString()+$"World:{m_EntityWorld}";
        }
        public override void Dispose()
        {
            m_EntityWorld.Dispose();
            m_EntityWorld = null;
            base.Dispose();
        }
    }
}
