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
using System.Text;

namespace Engine.Server.Modules
{
    public class RoomModule:AbstractModule
    {
        PtRoomList m_RoomList = new PtRoomList().SetRooms(new List<PtRoom>());
        ILogger Logger;
        INetworkServer Server;
        public RoomModule() 
        {
            Server = Context.Retrieve(Context.SERVER).Server;
            Logger = Context.Retrieve(Context.SERVER).Logger;
            EventDispatcher<NetworkEventId, int>.AddListener(NetworkEventId.PeerConnected, OnPeerConnected);
            EventDispatcher<RequestMessageId,PtMessagePackage>.AddListener(RequestMessageId.GS_EnterGate, OnEnterGate);
        }
        void OnPeerConnected(int peerId)
        {
            Logger.Info(nameof(OnPeerConnected)+"   "+peerId);
            Server.Send(peerId, (ushort)ResponseMessageId.GS_ClientConnected,null);
        }
        void OnEnterGate(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                string userId = buffer.ReadString();
                Logger.Info($"{nameof(OnEnterGate)} UserId:{userId}");
                User user = new User()
                {
                    Id = message.ExtraPeerId,
                    Name = userId
                };

            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
