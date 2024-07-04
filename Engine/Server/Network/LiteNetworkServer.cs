using Engine.Common;
using Engine.Common.Event;
using Engine.Common.Log;
using Engine.Common.Network;
using Engine.Common.Network.Integration;
using Engine.Common.Protocol;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace Engine.Server.Network
{
    public class LiteNetworkServer : INetworkServer
    {
        readonly string connectionKey;
        readonly NetManager manager;
        readonly EventBasedNetListener listener;
        bool isRunning = true;
        public LiteNetworkServer(string key)
        {
            connectionKey = key;
            listener = new EventBasedNetListener();
            manager = new NetManager(listener);
            manager.DisconnectTimeout = 5000;
            manager.UnconnectedMessagesEnabled = true;
        }
        public void Run(int port)
        {
            listener.ConnectionRequestEvent += request =>
            {
                Context.Retrieve(Context.SERVER).Logger.Info("ConnectionRequestEvent "+request.ToString());
                request.AcceptIfKey(connectionKey);
            };
            listener.PeerConnectedEvent += peer =>
            {
                EventDispatcher<NetworkEventId, int>.DispatchEvent(NetworkEventId.PeerConnected, peer.Id);
            };
            listener.NetworkReceiveEvent += (peer, reader, method) =>
            {
                byte[] raw = reader.GetRemainingBytes();
                PtMessagePackage package = PtMessagePackage.Read(raw);
                package.ExtraPeerId = peer.Id;
                Context.Retrieve(Context.SERVER).Logger.Info($"NetworkReceiveEvent id:{peer.Id} messageId:{package.MessageId}");
                EventDispatcher<RequestMessageId, PtMessagePackage>
                    .DispatchEvent((RequestMessageId)package.MessageId, package);
                reader.Recycle();
            };
            listener.PeerDisconnectedEvent += (peer, info) =>
            {
                EventDispatcher<NetworkEventId, int>.DispatchEvent(NetworkEventId.PeerDisconnected, peer.Id);
            };
            manager.Start(port);
            ThreadPool.QueueUserWorkItem(PollEvents, null);
        }

        void PollEvents(object obj)
        {
            while (isRunning)
            {
                manager.PollEvents();
                Thread.Sleep(15);
            }
        }
        public void Send(int clientId, ushort messageId, byte[] data)
        {
            byte[] raw = PtMessagePackage.Write(PtMessagePackage.Build(messageId, data));
            Context.Retrieve(Context.SERVER).Logger.Info($"Send clientId:{clientId} messageId:{messageId} isInPeerList:{manager.ConnectedPeerList.Find(i => i.Id == clientId) != null}");
            for (int i = 0; i < manager.ConnectedPeerList.Count; ++i)
            {
                NetPeer netPeer = manager.ConnectedPeerList[i];
                if (netPeer != null && netPeer.Id == clientId)
                {
                    netPeer.Send(raw, DeliveryMethod.ReliableOrdered);
                    break;
                }
            }
        }

        public void Send(int[] clientIds, ushort messageId, byte[] data)
        {
            foreach (var clientId in clientIds)
            {
                Send(clientId, messageId, data);
            }
        }

        public int GetActivePort()
        {
            return manager.LocalPort;
        }
    }
}
