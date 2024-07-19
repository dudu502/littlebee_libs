using Engine.Common;
using Engine.Common.Event;
using Engine.Common.Log;
using Engine.Common.Module;
using Engine.Common.Network;
using Engine.Common.Network.Integration;
using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using Engine.Server.Modules.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Engine.Server.Modules
{
    public class RoomModule : AbstractModule
    {
        PtRoomList m_RoomList = new PtRoomList().SetRooms(new List<PtRoom>());
        Context m_Context;
        ILogger m_Logger;
        INetworkServer m_Server;
        List<User> m_Users = new List<User>();
        Dictionary<int, RoomProcess> m_DictProcessId = new Dictionary<int, RoomProcess>();
        public RoomModule()
        {
            m_Context = Context.Retrieve(Context.SERVER);
            m_Server = m_Context.Server;
            m_Logger = m_Context.Logger;
            EventDispatcher<NetworkEventId, int>.AddListener(NetworkEventId.PeerConnected, OnPeerConnected);
            EventDispatcher<NetworkEventId, int>.AddListener(NetworkEventId.PeerDisconnected, OnPeerDisconnected);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.GS_EnterGate, OnEnterGate);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.GS_RoomList, OnRoomList);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.GS_CreateRoom, OnCreateRoom);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.GS_UpdateRoom, OnUpdateRoom);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.GS_JoinRoom, OnJoinRoom);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.GS_LeaveRoom, OnLeaveRoom);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.GS_LaunchGame, OnLaunchGame);
            EventDispatcher<RequestMessageId, PtMessagePackage>.AddListener(RequestMessageId.UGS_RoomPlayerDisconnect, OnRoomPlayerDisconnected);
        }
        void OnRoomPlayerDisconnected(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                int battleServerPort = buffer.ReadInt32();
                string userId = buffer.ReadString();
                bool hasOnlinePlayer = buffer.ReadBool();
               
                if (!hasOnlinePlayer)
                {
                    PtRoom room = m_RoomList.Rooms.Find(r => r.Players.Exists(p => p.UserId == userId));
                    if (room != null)
                    {
                        m_RoomList.Rooms.Remove(room);
                        m_Logger.Warn($"Remove Room id:{room.RoomId} by PlayerDisconnected Room Count:{m_RoomList.Rooms.Count}");
                    }
                    KillRoomProcessByPort(battleServerPort);
                }
                m_Logger.Warn($"{nameof(OnRoomPlayerDisconnected)} at Port:{battleServerPort} userId:{userId} hasOnlinePlayer:{hasOnlinePlayer}");
            }
        }
        void OnPeerConnected(int peerId)
        {
            m_Logger.Info(nameof(OnPeerConnected) + "   " + peerId);
            m_Server.Send(peerId, (ushort)ResponseMessageId.GS_ClientConnected, null);
        }
        void OnLeaveRoom(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                User user = m_Users.Find(u => u.PeerId == message.ExtraPeerId);
                if (user != null)
                {
                    uint roomId = buffer.ReadUInt32();
                    OnLeaveRoomImpl(roomId, user);
                }
            }
        }
        void OnLeaveRoomImpl(uint roomId, User user)
        {
            if (m_RoomList.HasRooms())
            {
                PtRoom room = m_RoomList.Rooms.Find(r => r.RoomId == roomId);
                if (room != null && room.HasPlayers())
                {
                    PtRoomPlayer player = room.Players.Find(p => p.UserId == user.UserId);
                    room.Players.Remove(player);
                    if (player.UserId == room.RoomOwnerUserId)
                    {
                        if (room.Players.Count > 0)
                            room.SetRoomOwnerUserId(room.Players[0].UserId);
                    }
                    m_Server.Send(user.PeerId, (ushort)ResponseMessageId.GS_LeaveRoom, null);
                    OnUpdateDataToRoomPlayer(room, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(room));
                    if (room.Players.Count == 0)
                    {
                        m_RoomList.Rooms.Remove(room);
                    }
                    OnRoomList(user.PeerId);
                }
            }
        }
        async void OnLaunchGame(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                uint roomId = buffer.ReadUInt32();
                if (m_RoomList.HasRooms())
                {
                    PtRoom room = m_RoomList.Rooms.Find(r => r.RoomId == roomId);
                    if (room != null)
                    {
                        if (room.HasPlayers())
                        {
                            PtLaunchData gameData = new PtLaunchData();
                            gameData.SetPlayerNumber((byte)room.Players.Count);
                            gameData.SetIsStandaloneMode(room.Players.Count == 1);
                            gameData.SetMapId(room.MapId);
                            gameData.SetConnectionKey("room-battle");
                            gameData.SetRoomServerAddr(m_Context.GetMeta(ContextMetaId.SERVER_ADDRESS));
                            gameData.SetRoomServerPort(GetEmptyRoomPort());
                            OnUpdateDataToRoomPlayer(room, (ushort)ResponseMessageId.GS_LaunchGame, null);
                            await CreateRoomProcess(m_Context.GetMeta(ContextMetaId.ROOM_MODULE_FULL_PATH), gameData, room);
                            // created room process 
                            OnUpdateDataToRoomPlayer(room, (ushort)ResponseMessageId.GS_LaunchRoomInstance, PtLaunchData.Write(gameData));
                            room.SetStatus(1);
                        }
                    }
                }
            }
        }

        public Task CreateRoomProcess(string dllPath,PtLaunchData launchData,PtRoom room)
        {
            if (m_DictProcessId.ContainsKey(launchData.RoomServerPort))
            {
                throw new Exception();
            }
            RoomProcess roomProcess = new RoomProcess();
            m_DictProcessId[launchData.RoomServerPort] = roomProcess;
            roomProcess.LaunchData = launchData;
            roomProcess.RoomId = room.RoomId;
            roomProcess.Port = launchData.RoomServerPort;
            m_Logger.Info($"{nameof(CreateRoomProcess)} dll:{dllPath}");
            var psi = new ProcessStartInfo("dotnet"," "+m_Context.GetMeta(ContextMetaId.ROOM_MODULE_FULL_PATH) +
                " -key "+launchData.ConnectionKey+
                " -port "+launchData.RoomServerPort+
                " -gsPort "+m_Context.Server.GetActivePort()+
                " -mapId "+launchData.MapId+
                " -playernumber "+launchData.PlayerNumber);
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;
        
            return Task.Run(() =>
            {
                DateTime now = DateTime.Now;
                var proc = Process.Start(psi);
                roomProcess.Set(proc);
                m_Logger.Info("Start Process in task"+(DateTime.Now-now).TotalMilliseconds);
            });
        }
        void OnPeerDisconnected(int peerId)
        {
            m_Logger.Info(nameof(OnPeerDisconnected) + "   " + peerId);
            var user = m_Users.Find(u => u.PeerId == peerId);
            if (user != null)
            {
                for(int i = m_RoomList.Rooms.Count - 1; i > -1; i--)
                {
                    PtRoom room = m_RoomList.Rooms[i];
                    if(room.Status == 0)
                    {
                        PtRoomPlayer player = room.Players.Find(p=>p.UserId == user.UserId);
                        if (player != null)
                            OnLeaveRoomImpl(room.RoomId, user);
                    }
                }
            }
        }
        void OnEnterGate(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                string userId = buffer.ReadString();
                m_Logger.Info($"{nameof(OnEnterGate)} UserId:{userId}");
                User user = new User()
                {
                    PeerId = message.ExtraPeerId,
                    UserId = userId
                };
                m_Users.Add(user);
            }
        }
        uint GetGuidUint()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            return BitConverter.ToUInt32(bytes, 0);
        }
        void OnRoomList(PtMessagePackage message)
        {
            OnRoomList(message.ExtraPeerId);
        }
        void OnRoomList(int peerId)
        {
            m_Server.Send(peerId, (ushort)ResponseMessageId.GS_RoomList, PtRoomList.Write(m_RoomList));
        }
        void OnUpdateRoom(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                byte type = buffer.ReadByte();
                uint roomId = buffer.ReadUInt32();
                string userId; PtRoomPlayer player;
                PtRoom room = m_RoomList.Rooms.Find(r => r.RoomId == roomId);
                if (room != null)
                {
                    switch (type)
                    {
                        case 0://change Team
                            userId = buffer.ReadString();
                            byte teamId = buffer.ReadByte();
                            player = room.Players.Find(p => p.UserId == userId);
                            if (player != null)
                                player.SetTeamId(teamId);
                            break;
                        case 1:

                            break;
                        case 2:
                            room.SetMapId(buffer.ReadUInt32());
                            room.SetMaxPlayerCount(buffer.ReadByte());
                            break;
                    }
                }
                OnUpdateDataToRoomPlayer(room, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(room));
            }
        }

        bool OnUpdateDataToRoomPlayer(PtRoom ptRoom, ushort messageId, byte[] bytes)
        {
            bool ret = false;
            if (ptRoom.HasPlayers())
            {
                foreach (PtRoomPlayer player in ptRoom.Players)
                {
                    foreach (User user in m_Users)
                    {
                        if (player.UserId == user.UserId)
                        {
                            ret = true;
                            m_Server.Send(user.PeerId, messageId, bytes);
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        void OnJoinRoom(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                uint roomId = buffer.ReadUInt32();
                string userId = buffer.ReadString();
                string userName = buffer.ReadString();
                byte teamdId = buffer.ReadByte();
                var room = m_RoomList.Rooms.Find(r => r.RoomId == roomId);
                if (room != null)
                {
                    if (room.Status == 0)
                    {
                        var player = room.Players.Find(p => p.UserId == userId);
                        if (player == null)
                        {
                            room.Players.Add(new PtRoomPlayer()
                                .SetEntityId(0)
                                .SetName(userName)
                                .SetStatus(1)
                                .SetTeamId(teamdId)
                                .SetColor(0)
                                .SetUserId(userId));
                        }
                        m_Server.Send(message.ExtraPeerId, (ushort)ResponseMessageId.GS_JoinRoom, new ByteBuffer().WriteByte(0).GetRawBytes());
                        OnUpdateDataToRoomPlayer(room, (ushort)ResponseMessageId.GS_UpdateRoom, PtRoom.Write(room));
                    }
                    else if (room.Players.Exists(p => p.Name == userName))
                    {
                        RoomProcess roomProcess = GetRoomProcess(room.RoomId);
                        if (roomProcess != null)
                        {
                            m_Server.Send(message.ExtraPeerId, (ushort)ResponseMessageId.GS_LaunchGame, null);
                            m_Server.Send(message.ExtraPeerId, (ushort)ResponseMessageId.GS_LaunchRoomInstance, PtLaunchData.Write(roomProcess.LaunchData));
                        }
                    }
                    else
                    {
                        m_Server.Send(message.ExtraPeerId, (ushort)ResponseMessageId.GS_ErrorCode, new ByteBuffer().WriteInt32(100000).GetRawBytes());
                    }
                }
            }
        }
        ushort GetEmptyRoomPort()
        {
            for (int i = 60000; i < 61000; ++i)
            {
                if (!m_DictProcessId.ContainsKey(i))
                    return (ushort)i;
            }
            return 0;
        }
        void KillRoomProcessByPort(int port)
        {
            m_Logger.Warn("KillRoomProcessByPort "+port+" contains:"+ m_DictProcessId.ContainsKey(port));
            if (m_DictProcessId.ContainsKey(port))
            {
                m_DictProcessId[port].Kill();
                m_DictProcessId.Remove(port);
            }
        }
        RoomProcess GetRoomProcess(uint roomId)
        {
            foreach (RoomProcess roomProcess in m_DictProcessId.Values)
            {
                if (roomProcess.RoomId == roomId)
                    return roomProcess;
            }
            return null;
        }
        void OnCreateRoom(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                uint mapId = buffer.ReadUInt32();
                string userId = buffer.ReadString();
                string userName = buffer.ReadString();
                byte teamId = buffer.ReadByte();
                PtRoom room = new PtRoom()
                    .SetMapId(mapId)
                    .SetStatus(0)
                    .SetRoomId(GetGuidUint())
                    .SetRoomOwnerUserId(userId)
                    .SetPlayers(new List<PtRoomPlayer>());
                PtRoomPlayer player = new PtRoomPlayer()
                    .SetEntityId(0)
                    .SetName(userName)
                    .SetStatus(1)
                    .SetUserId(userId)
                    .SetColor(0)
                    .SetTeamId(teamId);

                room.Players.Add(player);
                m_RoomList.Rooms.Add(room);
                m_Server.Send(message.ExtraPeerId, (ushort)ResponseMessageId.GS_CreateRoom, PtRoom.Write(room));
                m_Logger.Info($"{nameof(OnCreateRoom)} roomId:{room.RoomId}");
            }
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
