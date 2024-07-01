using Engine.Common;
using Engine.Common.Log;
using Engine.Common.Module;
using Engine.Common.Network.Integration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Modules
{
    public class RoomServiceModule:AbstractModule
    {
        INetworkClient NetworkClient;
        ILogger Logger;
        public RoomServiceModule()
        {
            Logger = Context.Retrieve(Context.CLIENT).Logger;
            NetworkClient = Context.Retrieve(Context.CLIENT).Client;
        }

        public void RequestEnterRoom()
        {

        }
        public void RequestInitPlayer()
        {

        }
        public void RequestPlayerReady()
        {

        }
        public void RequestHistoryKeyframes()
        {

        }
        public void RequestSyncClientKeyframes()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
