using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;

namespace Engine.Client.Ecsr.Systems
{
    public class MovementSystem : IEntitySystem
    {
        public EntityWorld World { set; get; }

        public void Execute()
        {
            World.ForEach<Movement, Position>((id, movement, Position) =>
            {
                Position.Pos += movement.Direction * movement.Speed;
            });
        }
    }
}
