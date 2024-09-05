using Engine.Client.Modules;
using Engine.Common;
using Engine.Common.Lockstep;


namespace Engine.Client.Lockstep.Behaviours
{
    public class FrameSenderBehaviour : ISimulativeBehaviour
    {
        public Simulation Sim { get; set; }
        private BattleServiceModule battleServiceModule;
        public void Start()
        {
            battleServiceModule = Context.Retrieve(Context.CLIENT).GetModule<BattleServiceModule>();
        }

        public void Stop()
        {
            
        }

        public void Update()
        {
            battleServiceModule.SendKeyFrames();
        }
    }
}
