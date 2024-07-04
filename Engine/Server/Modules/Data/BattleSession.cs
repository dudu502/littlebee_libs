using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Engine.Server.Modules.Data
{
    public class BattleSession
    {
        public readonly PtFramesList KeyFrameList = new PtFramesList();
        public readonly ConcurrentQueue<PtFramesList> QueueKeyFrameCollection = new ConcurrentQueue<PtFramesList>();

    }
}
