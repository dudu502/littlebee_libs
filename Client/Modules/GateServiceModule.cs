using Engine.Common;
using Engine.Common.Event;
using Engine.Common.Log;
using Engine.Common.Module;
using Engine.Common.Network;
using Engine.Common.Network.Integration;
using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Modules
{
    public class GateServiceModule:AbstractModule
    {
        public PtRoom SelfRoom { private set; get; }
        INetworkClient NetworkClient;
        ILogger Logger;
        public GateServiceModule()
        {
            Logger = Context.Retrieve(Context.CLIENT).Logger;
            NetworkClient = Context.Retrieve(Context.CLIENT).Client;
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_ClientConnected, OnResponseGateServerCliented);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.UGS_SearchAvailableGate, OnSearchAvailableGate);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_UpdateRoom, OnResponseUpdateRoom);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_RoomList, OnResponseRoomList);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_CreateRoom, OnResponseCreateRoom);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_JoinRoom, OnResponseJoinRoom);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_ErrorCode, OnResponseErrorCode);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LeaveRoom, OnResponseLeaveRoom);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LaunchGame, OnResponseLaunchGame);
            //EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LaunchRoomInstance, OnResponseLaunchRoomInstance);
        }
        public void RequestRoomList()
        {
            NetworkClient.Send((ushort)RequestMessageId.GS_RoomList, null);
        }

        public void RequestCreateRoom(uint mapId)
        {

        }

        public void RequestJoinRoom()
        {

        }

        public void RequestLaunchGame()
        {

        }

        public void ConnectGate(string ip, int port, string key)
        {
            NetworkClient.Connect(ip, port, key);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
