using Engine.Common;
using Engine.Common.Log;
using Engine.Server.Modules;
using Engine.Server.Network;

namespace LockstepGate
{
   
    internal class Program
    {
        const string TAG = "gate-room";
        static void Main(string[] args)
        {
            Context context = new Context(Context.SERVER, new LiteNetworkServer(TAG), new DefaultConsoleLogger(TAG))
                .SetMeta(ContextMetaId.ROOM_MODULE_FULL_PATH, @"D:\dudu502\littlebee_libs\Examples\Servers\Battle\bin\Debug\net6.0\Battle.dll")
                .SetMeta(ContextMetaId.MAX_CONNECTION_COUNT,"16")
                .SetMeta(ContextMetaId.SERVER_ADDRESS,"127.0.0.1")
                .SetModule(new RoomModule());
            context.Server.Run(9030);
            Console.ReadLine();
        }
    }

    public class DefaultConsoleLogger : ILogger
    {
        private string _tag;
        private readonly string _TimeFormatPatten = "yyyy/MM/dd HH:mm:ss.fff";

        public bool IsDebugEnabled { get; set; } = true;

        public DefaultConsoleLogger(string tag)
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

        public void Info(object message)
        {
            Info(message.ToString());
        }

        public void Warn(object message)
        {
            Warn(message.ToString());
        }

        public void Error(object message)
        {
            Error(message.ToString());
        }
    }
}