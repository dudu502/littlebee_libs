namespace Engine.Common.Lockstep
{
    public interface ISimulativeBehaviour
    {
        Simulation Sim { get; set; }
        void Start();
        void Update();
        void Stop();
    }
}
