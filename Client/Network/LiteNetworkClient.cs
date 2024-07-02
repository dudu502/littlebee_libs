using Engine.Common;
using Engine.Common.Event;
using Engine.Common.Network;
using Engine.Common.Network.Integration;
using Engine.Common.Protocol;
using LiteNetLib;
using System.Collections.Concurrent;
using System.Threading;

namespace Engine.Client.Network
{
    public class LiteNetworkClient : INetworkClient
    {
        NetManager manager;
        EventBasedNetListener listener;
        Thread runnerThread;
        bool isRunning;
        ConcurrentQueue<byte[]> queueMessages;

        public LiteNetworkClient()
        {
            queueMessages = new ConcurrentQueue<byte[]>();
            listener = new EventBasedNetListener();
            manager = new NetManager(listener);
            manager.UnconnectedMessagesEnabled = true;
            listener.NetworkReceiveEvent += (peer, reader, method) =>
            {
                byte[] raw = reader.GetRemainingBytes();
                queueMessages.Enqueue(raw);
                reader.Recycle();
            };
        }

        public void Connect()
        {
            manager.Start(50000);
            CreateThreads();
        }
        public void Connect(string ip, int port, string key)
        {
            manager.Start();
            manager.Connect(ip, port, key);
            Context.Retrieve(Context.CLIENT).Logger.Info($"Client Connect {ip}:{port} Key:{key}");
            CreateThreads();
        }

        void CreateThreads()
        {
            if (runnerThread == null)
            {
                runnerThread = new Thread(OnPollEvent);
                runnerThread.IsBackground = true;
                isRunning = true;
                runnerThread.Start();
            }
        }
        void OnPollEvent(object obj)
        {
            while (isRunning)
            {
                manager.PollEvents();
                Thread.Sleep(15);
                TickDispatchMessages();
            }
        }

        void TickDispatchMessages()
        {
            while (queueMessages.Count > 0)
            {
                if (queueMessages.TryDequeue(out byte[] bytes))
                {
                    PtMessagePackage package = PtMessagePackage.Read(bytes);
                    Context.Retrieve(Context.CLIENT).Logger.Info($"{nameof(TickDispatchMessages)} messageId:{package.MessageId}");
                    EventDispatcher<ResponseMessageId, PtMessagePackage>
                        .DispatchEvent((ResponseMessageId)package.MessageId, package);
                }
            }
        }
        public void Close()
        {
            isRunning = false;
            listener.ClearNetworkReceiveEvent();
            listener = null;
            manager.DisconnectAll();
            manager = null;
            if (runnerThread != null)
                runnerThread.Abort();
            runnerThread = null;
        }
        public void Send(ushort messageId, byte[] bytes)
        {
            Context.Retrieve(Context.CLIENT).Logger.Info($"{nameof(Send)} messageId:{messageId}");
            manager.SendToAll(PtMessagePackage.Write(PtMessagePackage.Build(messageId, bytes)), DeliveryMethod.ReliableOrdered);
        }
    }
}
