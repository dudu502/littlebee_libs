using Engine.Common;
using Engine.Common.Log;
using Engine.Common.Module;
using Engine.Common.Network;
using Engine.Common.Network.Integration;
using Engine.Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Modules
{
    public class BattleServiceModule : AbstractModule
    {
        Context m_Context;
        INetworkClient m_NetworkClient;
        ILogger m_Logger;
        public BattleServiceModule()
        {
            m_Context = Context.Retrieve(Context.CLIENT);
            m_Logger = m_Context.Logger;
            m_NetworkClient = m_Context.Client;
        }

        public void RequestEnterRoom(string userId)
        {
            m_NetworkClient.Send((ushort)RequestMessageId.RS_EnterRoom, new ByteBuffer().WriteString(userId).GetRawBytes());
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
