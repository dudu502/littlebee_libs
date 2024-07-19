using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using Engine.Client.Lockstep;
using Engine.Client.Modules;
using Engine.Client.Network;
using Engine.Common;
using Engine.Common.Log;
using Engine.Common.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public class UnityLogger : Engine.Common.Log.ILogger
{
    private string _tag;
    private readonly string _TimeFormatPatten = "yyyy/MM/dd HH:mm:ss.fff";

    public bool IsDebugEnabled { get; set; } = true;

    public UnityLogger(string tag)
    {
        _tag = tag;
    }
    public void Error(string message)
    {
        if (IsDebugEnabled)
            Debug.LogError($"[{DateTime.Now.ToString(_TimeFormatPatten)}]E[{_tag}]\t{message}");
    }

    public void Info(string message)
    {
        if (IsDebugEnabled)
            Debug.Log($"[{DateTime.Now.ToString(_TimeFormatPatten)}]I[{_tag}]\t{message}");
    }

    public void Warn(string message)
    {
        if (IsDebugEnabled)
            Debug.LogWarning($"[{DateTime.Now.ToString(_TimeFormatPatten)}]W[{_tag}]\t{message}");
    }

    public void Info(object message)
    {
        Info(LitJson.JsonMapper.ToJson(message));
    }

    public void Warn(object message)
    {
        Warn(LitJson.JsonMapper.ToJson(message));
    }

    public void Error(object message)
    {
        Error(LitJson.JsonMapper.ToJson(message));
    }
}
public class GameEntityInitializer : EntityInitializer
{
    public GameEntityInitializer(EntityWorld world) : base(world)
    {

    }

    public override List<Entity> CreateSelfEntityComponents(Guid entityId)
    {
        TSRandom tSRandom = TSRandom.New(GetHashCode());
        List<Entity> entities = new List<Entity>();
        Entity entity = new Entity();
        Appearance appearance = new Appearance();
        appearance.Resource = "Player";
        appearance.ShaderR = (byte)tSRandom.Next(0, 255);
        appearance.ShaderG = (byte)tSRandom.Next(0, 255);
        appearance.ShaderB = (byte)tSRandom.Next(0, 255);
        entity.AddComponent(appearance);

        Position position = new Position();
        position.Pos = new TrueSync.TSVector2(0, 0);
        entity.AddComponent(position);

        Movement movement = new Movement();
        movement.Speed = 0.1f * tSRandom.Next(-1f, 1f);
        movement.Direction = new TrueSync.TSVector2(0.1f * tSRandom.Next(-1f, 1f), 0.1f * tSRandom.Next(-1f, 1f));
        entity.AddComponent(movement);
        entities.Add(entity);
        return entities;
    }
}
public class GameEntityRenderSpawner : EntityRenderSpawner
{
    private Transform gameContainer;
    public GameEntityRenderSpawner(Transform container)
    {
        gameContainer = container;
    }
    /// <summary>
    /// Called in Unity main thread.
    /// </summary>
    /// <param name="request"></param>
    protected override void CreateEntityRendererImpl(CreateEntityRendererRequest request)
    {
        base.CreateEntityRendererImpl(request);
        GameObject resPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(request.ResourcePath)); 
        if(resPrefab.TryGetComponent<PlayerAppearanceRenderer>(out var component))
        {
            component.EntityId = request.EntityId;
            component.World = Context.Retrieve(Context.CLIENT).GetSimulationController<DefaultSimulationController>()
                .GetSimulation<DefaultSimulation>().GetEntityWorld();
            component.transform.SetParent(gameContainer);
        }
    }
}
public class MainCommand : MonoBehaviour
{
    public Context MainContext;
    public Transform GameContainer;
    private void Awake()
    {
        Handler.Initialize();
        MainContext = new Context(Context.CLIENT, new LiteNetworkClient(), new UnityLogger("Unity"));
        MainContext.SetMeta(ContextMetaId.STANDALONE_MODE_PORT, "50000")
                   .SetMeta(ContextMetaId.PERSISTENT_DATA_PATH, Application.persistentDataPath);
        MainContext.SetModule(new GateServiceModule())
                   .SetModule(new BattleServiceModule());
        DefaultSimulationController defaultSimulationController = new DefaultSimulationController();
        MainContext.SetSimulationController(defaultSimulationController);
        defaultSimulationController.CreateSimulation();
        EntityWorld entityWorld = defaultSimulationController.GetSimulation<DefaultSimulation>().GetEntityWorld();
        entityWorld.SetEntityInitializer(new GameEntityInitializer(entityWorld));
        entityWorld.SetEntityRenderSpawner(new GameEntityRenderSpawner(GameContainer));
    }

    private void Start()
    {
        
    }
    [TerminalCommand("setuid","setuid(id)")]
    public void SetUserId(string uid)
    {
        MainContext.SetMeta(ContextMetaId.USER_ID, uid);
    }

    [TerminalCommand("connect-ip-port-key", "connect(ip,port,key)")]
    public void ConnectIP_Port_Key(string ip,int port,string key)
    {
        MainContext.Client.Connect(ip,port,key);
    }

    [TerminalCommand("connect","connect default")]
    public void Connect()
    {
        ConnectIP_Port_Key("127.0.0.1",9030,"gate-room");
    }

    [TerminalCommand("create","create_room()")]
    public void CreateRoom()
    {
        MainContext.GetModule<GateServiceModule>().RequestCreateRoom(1);
    }

    [TerminalCommand("join","join(roomId)")]
    public void JoinRoom(uint roomId)
    {
        MainContext.GetModule<GateServiceModule>().RequestJoinRoom(roomId);
    }

    [TerminalCommand("leave", "leave_room()")]
    public void LeaveRoom()
    {
        MainContext.GetModule<GateServiceModule>().RequestLeaveRoom();
    }

    [TerminalCommand("roomlist","fetch all rooms")]
    public void RoomList()
    {
        MainContext.GetModule<GateServiceModule>().RequestRoomList();
    }
    [TerminalCommand("update-team", "update-team(roomId,userId,teamId)")]
    public void UpdateTeam(uint roomId,string userId,byte teamId)
    {
        MainContext.GetModule<GateServiceModule>().RequestUpdatePlayerTeam(roomId,userId,teamId);
    }
    [TerminalCommand("update-map", "update-map(roomId,mapId,maxPlayerCount)")]
    public void UpdateMap(uint roomId, uint mapId, byte maxPlayerCount)
    {
        MainContext.GetModule<GateServiceModule>().RequestUpdateMap(roomId, mapId, maxPlayerCount);
    }

    [TerminalCommand("launch","launch game")]
    public void Launch()
    {
        MainContext.GetModule<GateServiceModule>().RequestLaunchGame();
    }
    private void Update()
    {
        Handler.Update();
    }
}
