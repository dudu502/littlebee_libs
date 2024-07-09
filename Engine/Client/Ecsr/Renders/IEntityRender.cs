using Engine.Common.Lockstep;

namespace Engine.Client.Ecsr.Renders
{
    public interface IEntityRender
    {
        Simulation Sim { set; get; }
    }
}
