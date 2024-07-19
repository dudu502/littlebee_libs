using Engine.Common;
using Engine.Common.Log;
using Engine.Server.Log;
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
}