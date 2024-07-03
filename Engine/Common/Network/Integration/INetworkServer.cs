using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Common.Network.Integration
{
    public interface INetworkServer
    {
        void Send(int clientId, ushort messageId,byte[] data);
        void Send(int[] clientIds, ushort messageId, byte[] data);
        void Run(int port);
    }
}
