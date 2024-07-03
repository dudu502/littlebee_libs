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

namespace Engine.Client.Modules
{
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
        public void RequestRoomList()
        {
            m_Client.Send((ushort)RequestMessageId.GS_RoomList, null);
        }
        void OnResponseLeaveRoom(PtMessagePackage message)
        {
            SelfRoom = null;
            m_Logger.Info($"{nameof(OnResponseLeaveRoom)}");
        }
        void OnResponseLaunchRoomInstance(PtMessagePackage message)
        {
            PtLaunchData launchData = PtLaunchData.Read(message.Content);
            DispatchHandlerEvent(MainLoopLoadingEvent.UpdateLoading, new Tuple<LoadingType, float>(LoadingType.CreateRoomServiceComplete,0.4f));
        }
        void OnResponseLaunchGame(PtMessagePackage message)
        {

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
            DispatchHandlerEvent(MainLoopGateEvent.RoomUpdate, room);
        }
        void OnResponseRoomList(PtMessagePackage message)
        {
            PtRoomList rooms = PtRoomList.Read(message.Content);
            DispatchHandlerEvent(MainLoopGateEvent.RoomListUpdated, rooms);
            m_Logger.Info(rooms);
        }
        void OnResponseCreateRoom(PtMessagePackage message)
        {
            PtRoom room = PtRoom.Read(message.Content);
            SelfRoom = room;
            DispatchHandlerEvent(MainLoopGateEvent.RoomCreated, room);
            m_Logger.Info(room);
        }
        void OnResponseJoinRoom(PtMessagePackage message)
        {
            using(ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                byte errorCode = buffer.ReadByte();
                if (0 == errorCode)
                    DispatchHandlerEvent<MainLoopGateEvent,object>(MainLoopGateEvent.RoomJoined, null);
            }
        }
        void OnResponseGateServerCliented(PtMessagePackage message)
        {
            string userId = m_Context.GetMeta(ContextMetaId.UserId);
            if (userId == string.Empty)
                userId = message.GetHashCode().ToString();
            m_Logger.Info(nameof(OnResponseGateServerCliented)+ " userId:"+userId);

            m_Client.Send((ushort)RequestMessageId.GS_EnterGate, new ByteBuffer().WriteString(userId).GetRawBytes());
        }

        public void RequestCreateRoom(uint mapId)
        {
            string userId = m_Context.GetMeta(ContextMetaId.UserId);
            string userName = userId;
            byte teamId = 1;
            m_Client.Send((ushort)RequestMessageId.GS_CreateRoom, new ByteBuffer()
                .WriteUInt32(mapId).WriteString(userId).WriteString(userName).WriteByte(teamId).GetRawBytes());
        }

        public void RequestJoinRoom(PtRoom room)
        {
            string userId = m_Context.GetMeta(ContextMetaId.UserId);
            string userName = userId;
            byte teamId = 1;
            m_Client.Send((ushort)RequestMessageId.GS_JoinRoom, new ByteBuffer()
               .WriteUInt32(room.RoomId).WriteString(userId).WriteString(userName).WriteByte(teamId).GetRawBytes());
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

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
