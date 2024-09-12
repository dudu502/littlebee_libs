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
        bool inited;
        public LiteNetworkClient()
        {
            Initialize();
        }

        void Initialize()
        {
            if (!inited)
            {
                queueMessages = new ConcurrentQueue<byte[]>();
                listener = new EventBasedNetListener();
                manager = new NetManager(listener);
                manager.UnconnectedMessagesEnabled = true;
                listener.NetworkReceiveEvent += (peer, reader, chnl,method) =>
                {
                    byte[] raw = reader.GetRemainingBytes();
                    queueMessages.Enqueue(raw);
                    reader.Recycle();
                };
                inited = true;
            }
        }

        public void Connect()
        {
            Initialize();
            manager.Start(50000);
            CreateThreads();
        }
        public void Connect(string ip, int port, string key)
        {
            Initialize();
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
                TickDispatchMessages();
                Thread.Sleep(15);
            }
        }

        void TickDispatchMessages()
        {
            while (queueMessages != null && queueMessages.TryDequeue(out byte[] bytes))
            {
                PtMessagePackage package = PtMessagePackage.Read(bytes);
                Context.Retrieve(Context.CLIENT).Logger.Info($"{nameof(TickDispatchMessages)} messageId:{(ResponseMessageId)package.MessageId} Length:{bytes.Length}");
                EventDispatcher<ResponseMessageId, PtMessagePackage>
                    .DispatchEvent((ResponseMessageId)package.MessageId, package);
            }
        }
        public void Close()
        {
            isRunning = false;
            runnerThread = null;
            queueMessages = null;
            manager.DisconnectAll();
            manager.Stop();
            manager = null;
            listener.ClearNetworkReceiveEvent();
            listener = null;
            inited = false;
        }
        public void Send(ushort messageId, byte[] bytes)
        {
            Context.Retrieve(Context.CLIENT).Logger.Info($"{nameof(Send)} messageId:{(RequestMessageId)messageId}");
            manager.SendToAll(PtMessagePackage.Write(PtMessagePackage.Build(messageId, bytes)), DeliveryMethod.ReliableOrdered);
        }

        public int GetActivePort()
        {
            return manager.LocalPort;
        }
    }
}
