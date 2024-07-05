using Engine.Common;
using Engine.Common.Log;
using Engine.Common.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Modules
{
    public class EntityInitializerModule:AbstractModule
    {
        Context m_Context;
        ILogger m_Logger;
        public EntityInitializerModule() 
        {
            m_Context = Context.Retrieve(Context.CLIENT);
            m_Logger = m_Context.Logger;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
