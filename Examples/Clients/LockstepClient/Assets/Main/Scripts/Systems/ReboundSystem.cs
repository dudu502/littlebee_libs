using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Systems;
using System.Diagnostics;
using System.Security.Cryptography;
using TrueSync;

public class ReboundSystem : IEntitySystem
{
    public EntityWorld World { get; set; }

    public void Execute()
    {
        World.ForEach<Circle>((id, circle) =>
        {
            var cmove = World.GetEntity(id).Components[typeof(Movement)] as Movement;
            var cPos = World.GetEntity(id).Components[typeof(Position)] as Position;
            //World.ForEach<Tile>((tid, tile) =>
            //{
            //    var tPos = World.GetEntity(tid).Components[typeof(Position)] as Position;
            //    var xMin = (tPos.Pos - tile.Size / 2).x;
            //    var yMin = (tPos.Pos - tile.Size / 2).y;
            //    var xMax = (tPos.Pos + tile.Size / 2).x;
            //    var yMax = (tPos.Pos + tile.Size / 2).y;

                


            //});
            World.ForEach<Wall>((wId, wall) =>
            {                
                var wPos = World.GetEntity(wId).Components[typeof(Position)] as Position;
                var wW = wall.Width;
                var wH = wall.Height;
                var cRad = circle.Radius;
                if((wall.Dir&1) == 1 && (cPos.Pos.x + cRad) >= wPos.Pos.x - wW / 2)
                {
                    var d = cmove.Direction;
                    d.x *= -1;
                    cmove.Direction = d;
                }
                if ((wall.Dir & 2) == 2 && (cPos.Pos.x - cRad) <= wPos.Pos.x + wW / 2)
                {
                    var d = cmove.Direction;
                    d.x *= -1;
                    cmove.Direction = d;
                }
                if ((wall.Dir & 4) == 4 && (cPos.Pos.y + cRad) >= wPos.Pos.y - wH / 2)
                {
                    var d = cmove.Direction;
                    d.y *= -1;
                    cmove.Direction = d;
                }
                if ((wall.Dir & 8) == 8 && (cPos.Pos.y - cRad) <= wPos.Pos.y + wH / 2)
                {
                    var d = cmove.Direction;
                    d.y *= -1;
                    cmove.Direction = d;
                }
            });
        });
    }
}

