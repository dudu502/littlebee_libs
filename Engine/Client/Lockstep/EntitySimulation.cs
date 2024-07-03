using Engine.Client.Ecsr.Entitas;
using Engine.Common.Lockstep;

namespace Engine.Client.Lockstep
{
    public class EntitySimulation : Simulation
    {
        EntityWorld m_EntityWorld;
        public EntitySimulation(byte id) : base(id)
        {
        }
        public EntityWorld GetEntityWorld() { return m_EntityWorld; }
        public void SetEntityWorld(EntityWorld world)
        {
            m_EntityWorld = world;
        }
        public override void Dispose()
        {
            m_EntityWorld.Dispose();
            m_EntityWorld = null;
            base.Dispose();
        }
    }
}
