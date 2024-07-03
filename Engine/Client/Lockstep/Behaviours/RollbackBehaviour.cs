using Engine.Common.Lockstep;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Lockstep.Behaviours
{
    public class RollbackBehaviour : EntityBehaviour
    {
        ComponentsBackupBehaviour backupBehaviour;
        public override void Start()
        {
            base.Start();
            backupBehaviour = Sim.GetBehaviour<ComponentsBackupBehaviour>();
        }

        public override void Update()
        {

        }
    }
}
