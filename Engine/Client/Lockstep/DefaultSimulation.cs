using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using Engine.Common.Lockstep;

namespace Engine.Client.Lockstep
{
    public class DefaultSimulation : Simulation
    {
        EntityWorld m_EntityWorld;

        IEntityRenderSpawner m_EntityRenderSpawner;
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
        public void SetEntityRenderSpawner(IEntityRenderSpawner renderSpawner)
        {
            m_EntityRenderSpawner = renderSpawner;
        }
        public IEntityRenderSpawner GetRenderSpawner()
        {
            return m_EntityRenderSpawner;
        }
        public override void Dispose()
        {
            m_EntityWorld.Dispose();
            m_EntityWorld = null;
            base.Dispose();
        }
    }
}
