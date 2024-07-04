using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Common.Network.Integration
{
    public interface INetworkClient
    {
        void Send(ushort messageId,byte[] bytes);
        void Connect();
        void Connect(string ip, int port, string key);
        void Close();
        int GetActivePort();
    }
}
