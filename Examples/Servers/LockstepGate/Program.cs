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
                .SetMeta(ContextMetaId.RoomModuleFullPath, @"D:\dudu502\littlebee_libs\Examples\Servers\Battle\bin\Debug\net6.0\Battle.dll")
                .SetMeta(ContextMetaId.MaxConnectionCount,"16")
                .SetMeta(ContextMetaId.ServerAddress,"127.0.0.1")
                .SetModule(new RoomModule());
            context.Server.Run(9030);
            Console.ReadKey();
        }
    }
}