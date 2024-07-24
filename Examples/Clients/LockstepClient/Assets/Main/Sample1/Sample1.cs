using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using Engine.Client.Ecsr.Systems;
using Engine.Client.Lockstep;
using Engine.Client.Lockstep.Behaviours;
using Engine.Client.Modules;
using Engine.Client.Network;
using Engine.Client.Protocol.Pt;
using Engine.Common;
using Engine.Common.Log;
using Engine.Common.Misc;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TrueSync;
using UnityEngine;

public class GameEntityInitializer : EntityInitializer
{
    public GameEntityInitializer(EntityWorld world) : base(world)
    {

    }

    public override List<Entity> OnCreateSelfEntityComponents(Guid entityId)
    {
        TSRandom tSRandom = TSRandom.New(GetHashCode());
        List<Entity> entities = new List<Entity>();

        for (int i = 0; i < 3; ++i)
        {
            Entity entity = new Entity();
            Appearance appearance = new Appearance();
            appearance.Resource = "Player";
            appearance.ShaderR = (byte)tSRandom.Next(0, 255);
            appearance.ShaderG = (byte)tSRandom.Next(0, 255);
            appearance.ShaderB = (byte)tSRandom.Next(0, 255);
            entity.AddComponent(appearance);
            Circle circle = new Circle();
            circle.IsRigid = true;
            circle.Radius = 0.5f;
            entity.AddComponent(circle);
            Position position = new Position();
            position.Pos = new TrueSync.TSVector2(i, 0);
            entity.AddComponent(position);

            Movement movement = new Movement();
            movement.Speed = 1f * tSRandom.Next(1f, 2f);
            movement.Direction = new TrueSync.TSVector2(0.1f * tSRandom.Next(-1f, 1f), 0.1f * tSRandom.Next(-1f, 1f));
            entity.AddComponent(movement);

          

            entities.Add(entity);
        }

        return entities;
    }

    public override void OnCreateEntities(uint mapId)
    {
        var path = Path.Combine(Context.Retrieve(Context.CLIENT).GetMeta(ContextMetaId.PERSISTENT_DATA_PATH), "map", mapId + ".bytes");
        Debug.LogWarning("OnCreateEntities "+path);
        byte[] bytes = File.ReadAllBytes(path);
        try
        {
            PtMap map = PtMap.Read(bytes); 
            Debug.LogWarning("OnCreateEntities " + map);
            if (map.HasEntities())
            {
                foreach (Entity entity in map.Entities.Elements)
                {
                    World.CreateEntity(entity);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
        }
        
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
        if(resPrefab.TryGetComponent<AppearanceRenderer>(out var component))
        {
            component.EntityId = request.EntityId;
            component.World = Context.Retrieve(Context.CLIENT).GetSimulationController<DefaultSimulationController>()
                .GetSimulation<DefaultSimulation>().GetEntityWorld();
            resPrefab.transform.SetParent(gameContainer);
        }
    }
}
public class Sample1 : Sample
{
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
        defaultSimulationController.CreateSimulation(new DefaultSimulation(),new EntityWorld(),
            new Engine.Common.Lockstep.ISimulativeBehaviour[] {
                new LogicFrameBehaviour(),
                new RollbackBehaviour(),
                new EntityBehaviour(),
                new SelectionInputBehaviour(),
                new ButtonInputBehaviour(),
                new ComponentsBackupBehaviour(),
            },
            new Engine.Client.Ecsr.Systems.IEntitySystem[]
            {
                new AppearanceSystem(),
                new FsmSystem(),
                new MovementSystem(),
                new ReboundSystem(),
            });
        EntityWorld entityWorld = defaultSimulationController.GetSimulation<DefaultSimulation>().GetEntityWorld();
        entityWorld.SetEntityInitializer(new GameEntityInitializer(entityWorld));
        entityWorld.SetEntityRenderSpawner(new GameEntityRenderSpawner(GameContainer));
    }

    private void Start()
    {
        
    }


    [TerminalCommand("drawmap","save map asset")]
    public void DrawMap(uint mapId)
    {
        TSRandom tSRandom = TSRandom.New(GetHashCode());
        var path = Path.Combine(Application.persistentDataPath, "map", mapId + ".bytes");
        EntityList entityList = new EntityList()
            .SetElements(new List<Entity>());
        entityList.Elements.Add(new Entity()
            .AddComponent(new Position() { Pos = new TSVector2(12,0)})
            .AddComponent(new Engine.Client.Ecsr.Components.Wall() { Width = 0.2f,Height = 18,IsRigid=true,Dir = 1})
            .AddComponent(new Appearance() { Resource = "Wall"}));
        entityList.Elements.Add(new Entity()
            .AddComponent(new Position() { Pos = new TSVector2(-12, 0) })
            .AddComponent(new Engine.Client.Ecsr.Components.Wall() { Width = 0.2f, Height = 18, IsRigid = true, Dir = 2 })
            .AddComponent(new Appearance() { Resource = "Wall" }));
        entityList.Elements.Add(new Entity()
            .AddComponent(new Position() { Pos = new TSVector2(0, 9) })
            .AddComponent(new Engine.Client.Ecsr.Components.Wall() { Width = 24f, Height = 0.2f, IsRigid = true, Dir = 4 })
            .AddComponent(new Appearance() { Resource = "Wall" }));
        entityList.Elements.Add(new Entity()
           .AddComponent(new Position() { Pos = new TSVector2(0, -9) })
           .AddComponent(new Engine.Client.Ecsr.Components.Wall() { Width = 24f, Height = 0.2f, IsRigid = true, Dir = 8 })
           .AddComponent(new Appearance() { Resource = "Wall" }));

        entityList.Elements.Add(new Entity()
           .AddComponent(new Position() { Pos = new TSVector2(7, -5) })
           .AddComponent(new Tile() { Size = new TSVector2(3,4)})
           .AddComponent(new Appearance() { Resource = "Tile", ShaderR = (byte)tSRandom.Next(0, 255), ShaderG = (byte)tSRandom.Next(0, 255), ShaderB = (byte)tSRandom.Next(0, 255) }));

        PtMap ptMap = new PtMap().SetVersion("0.0.1")
            .SetEntities(entityList);

        File.WriteAllBytes(path,PtMap.Write(ptMap));
    }

    //[TerminalCommand("loadmap","load map asset")]
    //public void LoadMap(uint mapId)
    //{
    //    var path = Path.Combine(Application.persistentDataPath, "map", mapId + ".bytes");
    //    if (File.Exists(path))
    //    {
    //        PtMap.Read(File.ReadAllBytes(path));
    //    }
    //}
    private void Update()
    {
        Handler.Update();
    }
}
