using Engine.Common.Protocol.Pt;
using System.Diagnostics;

namespace Engine.Server.Modules.Data
{
    public class RoomProcess
    {
        public Process CurrentProcess { private set; get; }
        public uint RoomId { set; get; }
        public int Port { set; get; }
        public PtLaunchData LaunchData { set; get; }
        public void Set(Process proc)
        {
            CurrentProcess = proc;
        }
        public void Kill()
        {
            if (CurrentProcess != null)
                CurrentProcess.Kill();
        }
    }
}
