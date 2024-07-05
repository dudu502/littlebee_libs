using Engine.Common;
using Engine.Common.Event;
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

            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_ClientConnected, OnResponseRoomServerClientConnected);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_EnterRoom, OnResponseEnterRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_SyncKeyframes, OnResponseSyncKeyframes);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_InitPlayer, OnResponseInitPlayer);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_InitSelfPlayer, OnResponseInitSelfPlayer);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_PlayerReady, OnResponsePlayerReady);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_AllUserState, OnResponseAllUserState);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.RS_HistoryKeyframes, OnResponseHistoryKeyframes);
        }
        void OnResponseRoomServerClientConnected(PtMessagePackage message)
        {
            
        }
        void OnResponseEnterRoom(PtMessagePackage message)
        {

        }
        void OnResponseSyncKeyframes(PtMessagePackage message)
        {

        }
        void OnResponseInitPlayer(PtMessagePackage message)
        {

        }
        void OnResponseInitSelfPlayer(PtMessagePackage message)
        {

        }
        void OnResponsePlayerReady(PtMessagePackage message)
        {

        }
        void OnResponseAllUserState(PtMessagePackage message)
        {

        }
        void OnResponseHistoryKeyframes(PtMessagePackage message)
        {

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
