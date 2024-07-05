using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Engine.Common.Network.Integration
{
    public interface INetworkServer
    {
        void UnconnectedSend(ushort messageId, byte[] data, IPEndPoint endPoint);
        void Send(int clientId, ushort messageId,byte[] data);
        void Send(int[] clientIds, ushort messageId, byte[] data);
        void Send(ushort messageId, byte[] data);
        void Run(int port);

        int GetActivePort();
    }
}
