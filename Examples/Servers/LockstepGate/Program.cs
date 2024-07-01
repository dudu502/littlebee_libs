using Engine.Common;
using Engine.Common.Log;
using Engine.Server.Modules;
using Engine.Server.Network;

namespace LockstepGate
{
    public class ConsoleLogger : ILogger
    {
        private string _tag;
        private readonly string _TimeFormatPatten = "yyyy/MM/dd HH:mm:ss.fff";

        public bool IsDebugEnabled { get; set; } = true;

        public ConsoleLogger(string tag)
        {
            _tag = tag;
        }
        public void Error(string message)
        {
            if (IsDebugEnabled)
                Console.WriteLine($"[{DateTime.Now.ToString(_TimeFormatPatten)}]E[{_tag}]\t{message}");
        }

        public void Info(string message)
        {
            if (IsDebugEnabled)
                Console.WriteLine($"[{DateTime.Now.ToString(_TimeFormatPatten)}]I[{_tag}]\t{message}");
        }

        public void Warn(string message)
        {
            if (IsDebugEnabled)
                Console.WriteLine($"[{DateTime.Now.ToString(_TimeFormatPatten)}]W[{_tag}]\t{message}");
        }
    }

    internal class Program
    {
        const string TAG = "GS";
        static void Main(string[] args)
        {
            Context context = new Context(Context.SERVER, new LiteNetworkServer(TAG),new ConsoleLogger(TAG));
            context.SetModule(new RoomModule());
            context.Server.Run(9030);
            Console.ReadKey();
        }
    }
}