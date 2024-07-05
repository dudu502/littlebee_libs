using Engine.Common;
using Engine.Server.Log;
using Engine.Server.Modules;
using Engine.Server.Network;

namespace Battle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string key = "SomeConnectionKey";
            int port = 50000;
            uint mapId = 1;
            ushort playerNumber = 100;
            int gsPort = 9030;

            if (args.Length > 0)
            {
                if (Array.IndexOf(args, "-key") > -1) key = args[Array.IndexOf(args, "-key") + 1];
                if (Array.IndexOf(args, "-port") > -1) port = Convert.ToInt32(args[Array.IndexOf(args, "-port") + 1]);
                if (Array.IndexOf(args, "-mapId") > -1) mapId = Convert.ToUInt32(args[Array.IndexOf(args, "-mapId") + 1]);
                if (Array.IndexOf(args, "-playernumber") > -1) playerNumber = Convert.ToUInt16(args[Array.IndexOf(args, "-playernumber") + 1]);
                if (Array.IndexOf(args, "-gsPort") > -1) gsPort = Convert.ToInt32(args[Array.IndexOf(args, "-gsPort") + 1]);
            }
            Context context = new Context(Context.SERVER, new LiteNetworkServer(key), new DefaultConsoleLogger(key))
               .SetMeta(ContextMetaId.MaxConnectionCount, playerNumber.ToString())
               .SetMeta(ContextMetaId.SelectedRoomMapId, mapId.ToString())
               .SetMeta(ContextMetaId.GateServerPort, gsPort.ToString())
               .SetModule(new BattleModule());
            context.Server.Run(port);
            Console.ReadKey();
        }
    }
}