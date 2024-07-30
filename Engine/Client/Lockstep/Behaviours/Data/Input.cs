using Engine.Common.Protocol.Pt;
using System.Collections.Concurrent;

namespace Engine.Client.Lockstep.Behaviours.Data
{
    public enum InputSupportType 
    {
        Direction,
        KeyboardInput,
    }

    public sealed class Input
    {
        public static readonly ConcurrentQueue<PtFrame> InputFrames = new ConcurrentQueue<PtFrame>();
    }
}
