using Engine.Client.Lockstep;
using Engine.Client.Modules.Data;
using Engine.Common;
using Engine.Common.Event;
using Engine.Common.Log;
using Engine.Common.Misc;
using Engine.Common.Module;
using Engine.Common.Network;
using Engine.Common.Network.Integration;
using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.Client.Modules
{
    public class BattleServiceModule : AbstractModule
    {
        Context m_Context;
        INetworkClient m_NetworkClient;
        ILogger m_Logger;
        RoomSession m_RoomSession;
        public BattleServiceModule()
        {
            m_RoomSession = new RoomSession();
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
            string userId = m_Context.GetMeta(ContextMetaId.UserId);
            RequestEnterRoom(userId);
        }
        void OnResponseEnterRoom(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                string userEntityId = buffer.ReadString();
                string userId = buffer.ReadString();
                m_RoomSession.EntityId = userEntityId;
                m_RoomSession.UserId = userId;
                m_Logger.Info($"{nameof(OnResponseEnterRoom)} userEntityId:{userEntityId} userId:{userId}");
            }
        }
        void OnResponseSyncKeyframes(PtMessagePackage message)
        {
            m_RoomSession.QueueKeyFrames.Enqueue(PtFrames.Read(message.Content));
        }
        void OnResponseInitPlayer(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                uint id = buffer.ReadUInt32();
                m_Logger.Info($"{nameof(OnResponseInitPlayer)} id:{id}");
            }
        }
        void OnResponseInitSelfPlayer(PtMessagePackage message)
        {
            RequestPlayerReady();
        }
        void OnResponsePlayerReady(PtMessagePackage message)
        {
            m_Logger.Info($"{nameof(OnResponsePlayerReady)}");
        }
        async void OnResponseAllUserState(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                UserState state = (UserState)buffer.ReadByte();
                switch (state)
                {
                    case UserState.EnteredRoom:
                        uint mapId = buffer.ReadUInt32(); 
                        // save mapId from battle server.
                        m_Context.SetMeta(ContextMetaId.SelectedRoomMapId, mapId.ToString());
                        // let EntityInitializer to load all entitys

                        m_Context.SetSimulationController(new DefaultSimulationController());

                        RequestInitPlayer();
                        break;
                    case UserState.Re_EnteredRoom:
                        uint re_mapId = buffer.ReadUInt32();
                        break;
                    case UserState.BeReadyToEnterScene:
                        break;
                    case UserState.Re_BeReadyToEnterScene:
                        break;
                    default:
                        break;
                }
            }
        }
        async void OnResponseHistoryKeyframes(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                DateTime startDate = DateTime.Now;
                m_RoomSession.InitIndex = buffer.ReadInt32();
                long encodingTicks = buffer.ReadInt64();
                PtFramesList list = PtFramesList.Read(await SevenZip.Helper.DecompressBytesAsync(buffer.ReadBytes()));
                m_RoomSession.WriteKeyFrameIndex = list.FrameIdx;
                m_RoomSession.DictKeyFrames = new Dictionary<int, PtFrames>();
                list.Elements.ForEach(e=>m_RoomSession.DictKeyFrames.Add(e.FrameIdx,e));
                int offset = m_RoomSession.InitIndex == -1 ? 0 : m_RoomSession.InitIndex;
                startDate -= DateTime.Now - startDate + new TimeSpan(encodingTicks);
                // start simulation
            }
        }
        public void RequestEnterRoom(string userId)
        {
            m_NetworkClient.Send((ushort)RequestMessageId.RS_EnterRoom, new ByteBuffer().WriteString(userId).GetRawBytes());
        }
        public void RequestInitPlayer()
        {
            m_NetworkClient.Send((ushort)RequestMessageId.RS_InitPlayer, new ByteBuffer().WriteString(m_RoomSession.EntityId).GetRawBytes());
        }
        public void RequestPlayerReady()
        {
            m_NetworkClient.Send((ushort)RequestMessageId.RS_PlayerReady, new ByteBuffer().WriteString(m_RoomSession.EntityId).GetRawBytes());
        }
        public void RequestHistoryKeyframes()
        {
            int startIndex = -1;
            m_NetworkClient.Send((ushort)RequestMessageId.RS_HistoryKeyframes,new ByteBuffer().WriteInt32(startIndex).GetRawBytes());
        }
        public void RequestSyncClientKeyframes(int frameIdx,PtFrames frames)
        {
            frames.SetFrameIdx(frameIdx);
            m_NetworkClient.Send((ushort)RequestMessageId.RS_SyncClientKeyframes,PtFrames.Write(frames));
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
