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
using Engine.Common.Lockstep;
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


public class Sample1 : Sample
{
    public class GameEntityInitializer : EntityInitializer
    {
        public GameEntityInitializer(EntityWorld world) : base(world)
        {

        }

        public override EntityList OnCreateSelfEntityComponents(Guid entityId)
        {
            TSRandom tSRandom = TSRandom.New(GetHashCode());
            List<Entity> entities = new List<Entity>();
            EntityList result = new EntityList();
            result.SetElements(entities);


            for (int i = 0; i < 100; ++i)
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
                position.Pos = new TrueSync.TSVector2(tSRandom.Next(-10f, 10f), tSRandom.Next(-7f, 7f));
                entity.AddComponent(position);

                Movement movement = new Movement();
                movement.Speed = 1f * tSRandom.Next(0.1f, 0.5f);
                movement.Direction = new TrueSync.TSVector2(0.1f * tSRandom.Next(-1f, 1f), 0.1f * tSRandom.Next(-1f, 1f));
                entity.AddComponent(movement);



                entities.Add(entity);
            }

            return result;
        }

        public override void CreateEntities(uint mapId)
        {
            var path = Path.Combine(Context.Retrieve(Context.CLIENT).GetMeta(ContextMetaId.PERSISTENT_DATA_PATH), "map", mapId + ".map");
            byte[] bytes = File.ReadAllBytes(path);
            try
            {
                PtMap map = PtMap.Read(bytes);
                if (map.HasEntities())
                {
                    foreach (Entity entity in map.Entities.Elements)
                    {
                        World.CreateEntity(entity);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }
    public class GameEntityRenderSpawner : EntityRenderSpawner
    {
        private Transform gameContainer;
        private EntityWorld entityWorld;
        public GameEntityRenderSpawner(EntityWorld world, Transform container)
        {
            entityWorld = world;
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
            if (resPrefab.TryGetComponent<AppearanceRenderer>(out var component))
            {
                component.EntityId = request.EntityId;
                component.World = entityWorld;
                resPrefab.transform.SetParent(gameContainer);
            }
        }
    }
    private void Awake()
    {
        MainContext = new Context(Context.CLIENT, new LiteNetworkClient(), new UnityLogger("Unity"));
        MainContext.SetMeta(ContextMetaId.STANDALONE_MODE_PORT, "50000")
                   .SetMeta(ContextMetaId.PERSISTENT_DATA_PATH, Application.persistentDataPath);
        MainContext.SetModule(new GateServiceModule())
                   .SetModule(new BattleServiceModule());
        DefaultSimulationController defaultSimulationController = new DefaultSimulationController();
        MainContext.SetSimulationController(defaultSimulationController);
        // create a default simulation.
        defaultSimulationController.CreateSimulation(new DefaultSimulation(),new EntityWorld(),
            new ISimulativeBehaviour[] {
                new LogicFrameBehaviour(),
                new RollbackBehaviour(),
                new EntityBehaviour(),
                new ComponentsBackupBehaviour(),
            },
            new IEntitySystem[]
            {
                new AppearanceSystem(),
                new MovementSystem(),
                new ReboundSystem(),
            });
        EntityWorld entityWorld = defaultSimulationController.GetSimulation<DefaultSimulation>().GetEntityWorld();
        entityWorld.SetEntityInitializer(new GameEntityInitializer(entityWorld));
        entityWorld.SetEntityRenderSpawner(new GameEntityRenderSpawner(entityWorld,GameContainer));
    }

    private void Start()
    {
        
    }

    public override void DrawMap(uint mapId)
    {
        TSRandom tSRandom = TSRandom.New(GetHashCode());
        var path = Path.Combine(Application.persistentDataPath, "map", mapId + ".map");
        EntityList entityList = new EntityList()
            .SetElements(new List<Entity>());
        entityList.Elements.Add(new Entity()
            .AddComponent(new Position() { Pos = new TSVector2(12,0)})
            .AddComponent(new Wall() { Width = 0.2f,Height = 18,IsRigid=true,Dir = 1})
            .AddComponent(new Appearance() { Resource = "Wall"}));
        entityList.Elements.Add(new Entity()
            .AddComponent(new Position() { Pos = new TSVector2(-12, 0) })
            .AddComponent(new Wall() { Width = 0.2f, Height = 18, IsRigid = true, Dir = 2 })
            .AddComponent(new Appearance() { Resource = "Wall" }));
        entityList.Elements.Add(new Entity()
            .AddComponent(new Position() { Pos = new TSVector2(0, 9) })
            .AddComponent(new Wall() { Width = 24f, Height = 0.2f, IsRigid = true, Dir = 4 })
            .AddComponent(new Appearance() { Resource = "Wall" }));
        entityList.Elements.Add(new Entity()
           .AddComponent(new Position() { Pos = new TSVector2(0, -9) })
           .AddComponent(new Wall() { Width = 24f, Height = 0.2f, IsRigid = true, Dir = 8 })
           .AddComponent(new Appearance() { Resource = "Wall" }));

        PtMap ptMap = new PtMap().SetVersion("0.0.1")
            .SetEntities(entityList);

        File.WriteAllBytes(path,PtMap.Write(ptMap));
    }

    public override void PlayReplay(string name)
    {
        if (MainContext.GetSimulationController().State == SimulationController.RunState.Running)
            return;
        string path = Path.Combine(Application.persistentDataPath, name + ".rep");
        byte[] result = SevenZip.Helper.DecompressBytesAsync(File.ReadAllBytes(path)).Result;
        PtReplay replay = PtReplay.Read(result);

        DefaultReplaySimulationController simulationController = new DefaultReplaySimulationController();
        MainContext.SetSimulationController(simulationController);
        simulationController.CreateSimulation(new DefaultSimulation(), new EntityWorld(),
            new ISimulativeBehaviour[]
            {
                new ReplayLogicFrameBehaviour(),
                new ReplayInputBehaviour(),
                new EntityBehaviour(),
            },
            new IEntitySystem[] 
            {
                new AppearanceSystem(),
                new MovementSystem(),   
                new ReboundSystem(),
            });
        EntityWorld entityWorld = simulationController.GetSimulation<DefaultSimulation>().GetEntityWorld();
        entityWorld.SetEntityInitializer(new GameEntityInitializer(entityWorld));
        entityWorld.SetEntityRenderSpawner(new GameEntityRenderSpawner(entityWorld, GameContainer));
        // load map
        entityWorld.GetEntityInitializer().CreateEntities(replay.MapId);
        // load init entities;
        entityWorld.GetEntityInitializer().InitEntities = replay.InitEntities;
        entityWorld.GetEntityInitializer().CreateEntities(replay.InitEntities);
        simulationController.GetSimulation().GetBehaviour<ReplayLogicFrameBehaviour>().SetFrameIdxInfos(replay.Frames);
        simulationController.Start(DateTime.Now);
    }

    protected override void Update()
    {
        base.Update();
    }
}
