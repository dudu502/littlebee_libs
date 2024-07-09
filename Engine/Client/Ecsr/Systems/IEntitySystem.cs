using Engine.Client.Ecsr.Entitas;

namespace Engine.Client.Ecsr.Systems
{
    public interface IEntitySystem
    {
        EntityWorld World { get; set; }
        void Execute();
    }
}
