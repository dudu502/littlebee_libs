﻿using Engine.Common.Lockstep;
using Engine.Common.Log;
using Engine.Common.Module;
using Engine.Common.Network.Integration;
using System;
using System.Collections.Generic;

namespace Engine.Common
{
    public sealed class ContextMetaId
    {
        public const string UserId = "user_id";
    }
    public sealed class Context
    {
        public const string CLIENT = "client";
        public const string SERVER = "server";

        private static readonly Dictionary<string, Context> s_instances = new Dictionary<string, Context>();
        public INetworkServer Server { get; private set; }
        public INetworkClient Client { get; private set; }

        public ILogger Logger { get;private set; }

        public string Name; 

        SimulationController simulationController;

        Dictionary<Type, AbstractModule> modules;

        Dictionary<string, string> metas;

        public Context(string name,INetworkClient client, ILogger logger)
        {
            metas = new Dictionary<string, string>();
            Logger = logger;
            Client = client;
            Name = name;
            s_instances[name] = this;
            modules = new Dictionary<Type, AbstractModule>();
        
            Logger.Info($"Context Created Name:{name}");
        }

        public Context(string name, INetworkServer server,ILogger logger)
        {
            metas = new Dictionary<string, string>();
            Logger = logger;
            Server = server;
            Name = name;
            s_instances[name] = this;
            modules = new Dictionary<Type, AbstractModule>();
     
            Logger.Info($"Context Created Name:{name}");
        }

        public static Context Retrieve(string name)
        {
            if(s_instances.TryGetValue(name,out Context value)) 
                return value;
            return null;
        }
        public Context SetSimulationController(SimulationController controller)
        {
            simulationController = controller;
            Logger.Info("Context SetSimulationController " + controller);
            return this;
        }
        public SimulationController GetSimulationController()
        {
            return simulationController;
        }

        public Context SetMeta(string name,string value)
        {
            metas[name] = value;
            return this;
        }
        public string GetMeta(string name)
        {
            if(metas.TryGetValue(name,out var value)) return value;
            return string.Empty;
        }

        public Context SetModule(AbstractModule module)
        {
            modules[module.GetType()] = module; 
            Logger.Info("Context SetModule " + module);
            return this;
        }
        public M GetModule<M>() where M : AbstractModule
        {
            if (modules.TryGetValue(typeof(M), out AbstractModule module))
                return (M)module;
            return default;
        }
        public Context RemoveModule(Type type)
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