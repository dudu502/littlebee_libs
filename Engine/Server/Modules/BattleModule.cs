using Engine.Common.Log;
using Engine.Common;
using Engine.Common.Module;
using Engine.Common.Network.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using Engine.Common.Network;
using Engine.Common.Event;
using Engine.Common.Protocol;
using static LiteNetLib.EventBasedNetListener;
using Engine.Server.Modules.Data;

namespace Engine.Server.Modules
{
    public class BattleModule:AbstractModule
    {
        BattleSession Session;
        Context m_Context;
        ILogger m_Logger;
        INetworkServer m_Server;
        public BattleModule()
        {
            Session = new BattleSession();
            m_Context = Context.Retrieve(Context.SERVER);
            m_Server = m_Context.Server;
            m_Logger = m_Context.Logger;
            EventDispatcher<NetworkEventId, int>.AddListener(NetworkEventId.PeerConnected, OnPeerConnected);
            EventDispatcher<NetworkEventId, int>.AddListener(NetworkEventId.PeerDisconnected, OnPeerDisconnected);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.RS_EnterRoom, OnEnterRoom);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.RS_InitPlayer, OnInitPlayer);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.RS_PlayerReady, OnPlayerReady);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.RS_SyncClientKeyframes, OnSyncClientKeyframes);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.RS_HistoryKeyframes, OnHistoryKeyframes);
        }
        void OnPeerConnected(int peerId)
        {

        }
        void OnPeerDisconnected(int peerId) 
        { 

        }
        void OnEnterRoom(PtMessagePackage message)
        {

        }
        void OnInitPlayer(PtMessagePackage message)
        {

        }
        void OnPlayerReady(PtMessagePackage message)
        {

        }
        void OnSyncClientKeyframes(PtMessagePackage message)
        {

        }
        void OnHistoryKeyframes(PtMessagePackage message)
        {

        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
