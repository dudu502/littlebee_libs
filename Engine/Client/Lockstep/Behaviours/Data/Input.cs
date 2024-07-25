using Engine.Common.Protocol.Pt;
using System.Collections.Concurrent;

namespace Engine.Client.Lockstep.Behaviours.Data
{
    public enum InputSupportType 
    {
        Direction,
        KeyboardInput,
    }

    public class Input
    {
        public static ConcurrentQueue<PtFrame> InputFrames = new ConcurrentQueue<PtFrame>();
    }
}
