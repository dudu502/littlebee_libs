using Engine.Common.Log;
using Engine.Common;
using Engine.Common.Module;
using Engine.Common.Network.Integration;
using System;
using System.Collections.Generic;
using Engine.Common.Network;
using Engine.Common.Event;
using Engine.Common.Protocol;
using Engine.Server.Modules.Data;
using Engine.Common.Protocol.Pt;
using Engine.Common.Misc;
using Engine.Server.Lockstep;
using Engine.Common.Lockstep;

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
            m_Logger.Info($"{nameof(OnPeerConnected)} peerId:{peerId}");
            m_Server.Send(peerId, (ushort)ResponseMessageId.RS_ClientConnected, null);
        }
        void OnPeerDisconnected(int peerId) 
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                UserStateObject userState = Session.FindUserStateByPeerId(peerId);
                if(userState != null)
                {
                    userState.IsOnline = false;
                    buffer.WriteInt32(m_Server.GetActivePort());
                    buffer.WriteString(userState.UserId);
                    buffer.WriteBool(userState.IsOnline);
                    m_Server.UnconnectedSend((ushort)RequestMessageId.UGS_RoomPlayerDisconnect, buffer.GetRawBytes(),
                        new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), Convert.ToInt32(m_Context.GetMeta(ContextMetaId.GateServerPort,"9030"))));
                }
            }
        }
        void OnEnterRoom(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                string userId = buffer.ReadString();
                UserStateObject userState = Session.FindUserStateByUserId(userId);
                if(userState == null)
                {
                    string entityId = Guid.NewGuid().ToString();
                    Session.Users[entityId] = new UserStateObject(message.ExtraPeerId, UserState.EnteredRoom, userId, entityId);
                    bool isFull = Session.Users.Count == Convert.ToInt32(m_Context.GetMeta(ContextMetaId.MaxConnectionCount));
                    m_Logger.Info($"{nameof(OnEnterRoom)} Player Count:{Session.Users.Count} PlayerMaxConnectCount:{m_Context.GetMeta(ContextMetaId.MaxConnectionCount)} isFull:{isFull} EntityId:{entityId}");
                    m_Server.Send(message.ExtraPeerId, (ushort)ResponseMessageId.RS_EnterRoom, new ByteBuffer()
                        .WriteString(entityId).WriteString(userId).GetRawBytes());
                    if (isFull)
                        m_Server.Send((ushort)ResponseMessageId.RS_AllUserState, new ByteBuffer()
                            .WriteByte((byte)UserState.EnteredRoom).WriteUInt32(Convert.ToUInt32(m_Context.GetMeta(ContextMetaId.SelectedRoomMapId))).GetRawBytes());
                }
                else
                {
                    userState.Update(message.ExtraPeerId, UserState.Re_EnteredRoom);
                    userState.IsOnline = true;
                    m_Server.Send(message.ExtraPeerId, (ushort)ResponseMessageId.RS_EnterRoom, new ByteBuffer()
                        .WriteString(userState.UserEntityId).WriteString(userId).GetRawBytes());
                    m_Server.Send((ushort)ResponseMessageId.RS_AllUserState, new ByteBuffer()
                        .WriteByte((byte)UserState.Re_EnteredRoom).WriteUInt32(Convert.ToUInt32(m_Context.GetMeta(ContextMetaId.SelectedRoomMapId))).GetRawBytes());
                }
            }
        }
        void OnInitPlayer(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                string entityId = buffer.ReadString();
                m_Server.Send((ushort)ResponseMessageId.RS_InitPlayer, new ByteBuffer().WriteString(entityId).GetRawBytes());
                m_Server.Send(message.ExtraPeerId, (ushort)ResponseMessageId.RS_InitSelfPlayer, null);
            }
        }
        void OnPlayerReady(PtMessagePackage message)
        {
            using(ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                string entityId = buffer.ReadString();  
                if(Session.Users.TryGetValue(entityId,out var user))
                {
                    switch (user.StateFlag)
                    {
                        case UserState.EnteredRoom:
                            user.Update(message.ExtraPeerId, UserState.BeReadyToEnterScene);
                            bool allRdy = true;
                            m_Logger.Info($"{nameof(OnPlayerReady)} PeerId:"+message.ExtraPeerId);
                            foreach(UserStateObject userState in Session.Users.Values)
                            {
                                allRdy &= (userState.StateFlag == UserState.BeReadyToEnterScene);
                                m_Logger.Info("UserState.StateFlag == UserState.State.ReadyForInit:userStateId" + message.ExtraPeerId + " state:" + (userState.StateFlag));
                            }
                            m_Logger.Info($"{nameof(OnPlayerReady)} allRdy:{allRdy}");
                            if (allRdy)
                            {
                                PtStringList userEntityIds = new PtStringList().SetElement(new List<string>(Session.Users.Keys));
                                userEntityIds.Element.Sort((a,b)=>a.CompareTo(b));
                                m_Server.Send((ushort)ResponseMessageId.RS_AllUserState, new ByteBuffer()
                                    .WriteByte((byte)UserState.BeReadyToEnterScene).WriteBytes(PtStringList.Write(userEntityIds)).GetRawBytes());
                                // start simulation
                                SimulationController simulationController = new DefaultSimulationController();
                                simulationController.CreateSimulation();
                                m_Context.SetSimulationController(simulationController);
                                simulationController.Start(DateTime.Now);
                                m_Logger.Info("Start Simulation");
                            }
                            break;
                        case UserState.Re_EnteredRoom:
                            user.Update(message.ExtraPeerId, UserState.Re_BeReadyToEnterScene);
                            PtStringList newUserEntityIds = new PtStringList().SetElement(new List<string>(Session.Users.Keys));
                            newUserEntityIds.Element.Sort((a, b) => a.CompareTo(b));
                            m_Server.Send((ushort)ResponseMessageId.RS_AllUserState, new ByteBuffer()
                                    .WriteByte((byte)UserState.Re_BeReadyToEnterScene).WriteString(user.UserId).WriteBytes(PtStringList.Write(newUserEntityIds)).GetRawBytes());
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        void OnSyncClientKeyframes(PtMessagePackage message)
        {
            PtFrames collection = PtFrames.Read(message.Content);
            foreach(var keyFrame in collection.KeyFrames)
            {
                switch (keyFrame.Cmd)
                {
                    
                }
            }

            Session.QueueKeyFrameCollection.Enqueue(collection);
        }
        async void OnHistoryKeyframes(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                DateTime now = DateTime.Now;
                int startIndex = buffer.ReadInt32();
                byte[] keyframeRawSource = PtFramesList.Write(Session.KeyFrameList);
                byte[] compressedKeyFrameRawSource = await SevenZip.Helper.CompressBytesAsync(keyframeRawSource);
                long encodingTicks = (DateTime.Now - now).Ticks;
                byte[] keyframeBytes = new ByteBuffer().WriteInt32(startIndex)
                    .WriteInt64(encodingTicks).WriteBytes(compressedKeyFrameRawSource).GetRawBytes();
                m_Logger.Info($"KeyFrames compress rate:{1f * compressedKeyFrameRawSource.Length / keyframeRawSource.Length} rawLength:{keyframeRawSource.Length} compressedLength:{compressedKeyFrameRawSource.Length}");
                m_Server.Send(message.ExtraPeerId,(ushort)ResponseMessageId.RS_HistoryKeyframes,keyframeBytes);
            }
        }

        public void FlushKeyFrame(int currentFrameIdx)
        {
            Session.KeyFrameList.SetFrameIdx(currentFrameIdx);
            if (Session.QueueKeyFrameCollection.Count == 0) return;
            PtFrames flushCollection = new PtFrames().SetFrameIdx(currentFrameIdx).SetKeyFrames(new List<PtFrame>());
            while(Session.QueueKeyFrameCollection.TryDequeue(out PtFrames collection))
            {
                collection.SetFrameIdx(currentFrameIdx);
                flushCollection.KeyFrames.AddRange(collection.KeyFrames);
            }
            flushCollection.KeyFrames.Sort();
            Session.KeyFrameList.Elements.Add(flushCollection);
            if (flushCollection.KeyFrames.Count > 0)
                m_Server.Send((ushort)ResponseMessageId.RS_SyncKeyframes, PtFrames.Write(flushCollection));
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
