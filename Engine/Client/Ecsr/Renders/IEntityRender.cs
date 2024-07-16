using System;
using Engine.Client.Ecsr.Entitas;

namespace Engine.Client.Ecsr.Renders
{
    public interface IEntityRender
    {
        Guid EntityId { set; get; }
        EntityWorld World { set; get; }
    }
}
