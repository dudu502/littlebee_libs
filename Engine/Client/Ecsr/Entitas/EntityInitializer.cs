using Engine.Common.Event;

namespace Engine.Client.Ecsr.Entitas
{
    public class EntityInitializer
    {
        public EntityWorld World { get; private set; }
        public EntityInitializer(EntityWorld world) 
        {
            World = world;
        }

        /// <summary>
        /// Load component byte array
        /// </summary>
        /// <param name="bin"></param>
        public void CreateEntities(byte[] bin)
        {

        }
    }
}
