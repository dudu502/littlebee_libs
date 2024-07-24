using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client.Lockstep.Behaviours.Data
{
    public class Input
    {
        public static ConcurrentQueue<PtFrame> InputFrames = new ConcurrentQueue<PtFrame>();
    }
}
