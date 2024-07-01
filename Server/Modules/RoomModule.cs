using Engine.Common;
using Engine.Common.Event;
using Engine.Common.Log;
using Engine.Common.Module;
using Engine.Common.Network;
using Engine.Common.Protocol;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Server.Modules
{
    public class RoomModule:AbstractModule
    {
        PtRoomList m_RoomList = new PtRoomList().SetRooms(new List<PtRoom>());
        ILogger Logger;
        public RoomModule() 
        {
            Logger = Context.Retrieve(Context.SERVER).Logger;
            EventDispatcher<RequestMessageId,PtMessagePackage>.AddListener(RequestMessageId.GS_EnterGate, OnEnterGate);
        }

        void OnEnterGate(PtMessagePackage message)
        {
            using (ByteBuffer buffer = new ByteBuffer(message.Content))
            {
                string userId = buffer.ReadString();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
