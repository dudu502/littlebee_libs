using Engine.Common.Lockstep;
using Engine.Common.Module;
using Engine.Common.Network.Integration;
using System;
using System.Collections.Generic;

namespace Engine.Common
{
    public sealed class Context
    {
        public const string CLIENT = "client";
        public const string SERVER = "server";

        private static readonly Dictionary<string, Context> s_instances = new Dictionary<string, Context>();
        public INetworkServer Server { get; private set; }
        public INetworkClient Client { get; private set; }

        public string Name; 

        SimulationController simulationController;

        Dictionary<Type, AbstractModule> modules;

        public Context(string name,INetworkClient client)
        {
            Client = client;
            Name = name;
            s_instances[name] = this;
            modules = new Dictionary<Type, AbstractModule>();
        }

        public Context(string name, INetworkServer server)
        {
            Server = server;
            Name = name;
            s_instances[name] = this;
            modules = new Dictionary<Type, AbstractModule>();
        }

        public static Context Retrieve(string name)
        {
            if(s_instances.TryGetValue(name,out Context value)) 
                return value;
            return null;
        }
        public Context SetSimulationController(SimulationController wrap)
        {
            simulationController = wrap;
            return this;
        }
        public SimulationController GetSimulationController()
        {
            return simulationController;
        }
        public Context RegisterModule(AbstractModule module)
        {
            modules[module.GetType()] = module;
            return this;
        }
        public M RetrieveModule<M>() where M : AbstractModule
        {
            if (modules.TryGetValue(typeof(M), out AbstractModule module))
                return (M)module;
            return default;
        }
        public Context UnregisterModule(Type type)
        {
            if (modules.TryGetValue(type, out var module))
            {
                module.Dispose();
                modules.Remove(type);
            }
            return this;
        }
    }
}
