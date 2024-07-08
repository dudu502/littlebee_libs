using Engine.Common.Lockstep;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Server.Lockstep
{
    public class DefaultSimulation : Simulation
    {
        public DefaultSimulation(byte id) : base(id)
        {
            
        }

        protected override void CreateEntities()
        {
            base.CreateEntities();
        }
    }
}
