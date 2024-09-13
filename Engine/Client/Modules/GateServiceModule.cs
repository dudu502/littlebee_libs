using Engine.Client.Event;
using Engine.Common;
using Engine.Common.Event;
using Engine.Common.Log;
using Engine.Common.Module;
using Engine.Common.Network;
using Engine.Common.Network.Integration;
using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using System;
using System.Threading.Tasks;

namespace Engine.Client.Modules
{
    public enum GateServiceEvent
    {
        ClientConnected,
        UpdateRoom,
        RoomList,
        CreateRoom,
        JoinRoom,
        ErrorCode,
        LeaveRoom,
        LaunchGame,
        LaunchRoomInstance,
    }

    public class GateServiceModule:AbstractModule
    {
        public PtRoom SelfRoom { private set; get; }
        INetworkClient m_Client;
        ILogger m_Logger;
        Context m_Context;

        public GateServiceModule()
        {
            m_Context = Context.Retrieve(Context.CLIENT);
            m_Logger = m_Context.Logger;
            m_Client = m_Context.Client;
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_ClientConnected, OnResponseGateServerCliented);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_UpdateRoom, OnResponseUpdateRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_RoomList, OnResponseRoomList);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_CreateRoom, OnResponseCreateRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_JoinRoom, OnResponseJoinRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_ErrorCode, OnResponseErrorCode);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LeaveRoom, OnResponseLeaveRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LaunchGame, OnResponseLaunchGame);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.AddListener(ResponseMessageId.GS_LaunchRoomInstance, OnResponseLaunchRoomInstance);
        }
     
        void OnResponseLeaveRoom(PtMessagePackage message)
        {
            SelfRoom = null;
            m_Logger.Info($"{nameof(OnResponseLeaveRoom)}");
            EventDispatcher<GateServiceEvent, object>.DispatchEvent(GateServiceEvent.LeaveRoom, null);
        }
        async void OnResponseLaunchRoomInstance(PtMessagePackage message)
        {
            PtLaunchData launchData = PtLaunchData.Read(message.Content);
            EventDispatcher<GateServiceEvent, PtLaunchData>.DispatchEvent(GateServiceEvent.LaunchRoomInstance, launchData);
            EventDispatcher<LoadingType, LoadingEventId>.DispatchEvent(LoadingType.Loading, new LoadingEventId(LoadingEventId.CreateRoomServiceComplete,0.4f));

            // disconnect gate server and connect to room server
            m_Logger.Warn($"{nameof(OnResponseLaunchRoomInstance)} Disconnect Gate");
            m_Client.Close();
            //await Task.Delay(100);
            await Task.Yield();
            m_Logger.Warn($"{nameof(OnResponseLaunchRoomInstance)} Connect Battle");
            m_Client.Connect(launchData.RoomServerAddr, launchData.RoomServerPort,launchData.ConnectionKey);
        }
        void OnResponseLaunchGame(PtMessagePackage message)
        {
            EventDispatcher<LoadingType, LoadingEventId>.DispatchEvent(LoadingType.Loading, new LoadingEventId(LoadingEventId.CreateRoomServiceComplete, 0.1f));
        }
        void OnResponseErrorCode(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                int errorCode = buffer.ReadInt32();
                m_Logger.Error($"{nameof(OnResponseErrorCode)} {errorCode}");
            }
        }
        void OnResponseUpdateRoom(PtMessagePackage message)
        {
            PtRoom room = PtRoom.Read(message.Content);
            SelfRoom = room;
            m_Logger.Info(room);
            EventDispatcher<GateServiceEvent,PtRoom>.DispatchEvent(GateServiceEvent.UpdateRoom,room);
        }
        void OnResponseRoomList(PtMessagePackage message)
        {
            PtRoomList rooms = PtRoomList.Read(message.Content);
            m_Logger.Info(rooms);
            EventDispatcher<GateServiceEvent,PtRoomList>.DispatchEvent(GateServiceEvent.RoomList,rooms);
        }
        void OnResponseCreateRoom(PtMessagePackage message)
        {
            PtRoom room = PtRoom.Read(message.Content);
            SelfRoom = room;
            m_Logger.Info(room);
            EventDispatcher<GateServiceEvent, PtRoom>.DispatchEvent(GateServiceEvent.CreateRoom, room);
        }
        void OnResponseJoinRoom(PtMessagePackage message)
        {
            using(ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                byte errorCode = buffer.ReadByte();
                if (0 == errorCode)
                    EventDispatcher<GateServiceEvent, byte>.DispatchEvent(GateServiceEvent.JoinRoom, errorCode);
            }
        }
        void OnResponseGateServerCliented(PtMessagePackage message)
        {
            string userId = m_Context.GetMeta(ContextMetaId.USER_ID);
            if (userId == string.Empty)
                userId = message.GetHashCode().ToString();
            m_Logger.Info(nameof(OnResponseGateServerCliented)+ " userId:"+userId);

            m_Client.Send((ushort)RequestMessageId.GS_EnterGate, new ByteBuffer().WriteString(userId).GetRawBytes());

            EventDispatcher<GateServiceEvent, string>.DispatchEvent(GateServiceEvent.ClientConnected, userId);
        }
        public void RequestLeaveRoom()
        {
            if (SelfRoom != null)
                m_Client.Send((ushort)RequestMessageId.GS_LeaveRoom, new ByteBuffer().WriteUInt32(SelfRoom.RoomId).GetRawBytes());
        }
        public void RequestCreateRoom(uint mapId)
        {
            string userId = m_Context.GetMeta(ContextMetaId.USER_ID);
            string userName = userId;
            byte teamId = 1;
            m_Client.Send((ushort)RequestMessageId.GS_CreateRoom, new ByteBuffer()
                .WriteUInt32(mapId).WriteString(userId).WriteString(userName).WriteByte(teamId).GetRawBytes());
        }
        public void RequestJoinRoom(uint roomId)
        {
            string userId = m_Context.GetMeta(ContextMetaId.USER_ID);
            string userName = userId;
            byte teamId = 1;
            m_Client.Send((ushort)RequestMessageId.GS_JoinRoom, new ByteBuffer()
                .WriteUInt32(roomId).WriteString(userId).WriteString(userName).WriteByte(teamId).GetRawBytes());
        }

        public void RequestLaunchGame()
        {
            if (SelfRoom != null)
                m_Client.Send((ushort)RequestMessageId.GS_LaunchGame, new ByteBuffer()
                    .WriteUInt32(SelfRoom.RoomId).GetRawBytes());
        }

        public void ConnectGate(string ip, int port, string key)
        {
            m_Client.Connect(ip, port, key);
        }
        public void RequestUpdatePlayerTeam(uint roomId, string userId, byte teamId)
        {
            m_Client.Send((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer()
                .WriteByte(0).WriteUInt32(roomId).WriteString(userId).WriteByte(teamId).GetRawBytes());
        }
        public void RequestUpdatePlayerColor(uint roomId, string userId, byte color)
        {
            m_Client.Send((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer()
                .WriteByte(1).WriteUInt32(roomId).WriteString(userId).WriteByte(color).GetRawBytes());
        }
        public void RequestUpdateMap(uint roomId, uint mapId, byte maxPlayerCount)
        {
            m_Client.Send((ushort)RequestMessageId.GS_UpdateRoom, new ByteBuffer()
                .WriteByte(2).WriteUInt32(roomId).WriteUInt32(mapId).WriteByte(maxPlayerCount).GetRawBytes());
        }
        public void RequestRoomList()
        {
            m_Client.Send((ushort)RequestMessageId.GS_RoomList, null);
        }
        public override void Dispose()
        {
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_ClientConnected, OnResponseGateServerCliented);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_UpdateRoom, OnResponseUpdateRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_RoomList, OnResponseRoomList);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_CreateRoom, OnResponseCreateRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_JoinRoom, OnResponseJoinRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_ErrorCode, OnResponseErrorCode);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_LeaveRoom, OnResponseLeaveRoom);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_LaunchGame, OnResponseLaunchGame);
            EventDispatcher<ResponseMessageId, PtMessagePackage>.RemoveListener(ResponseMessageId.GS_LaunchRoomInstance, OnResponseLaunchRoomInstance);
            base.Dispose();
        }
    }
}
