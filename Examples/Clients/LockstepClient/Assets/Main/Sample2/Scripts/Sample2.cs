using Client.Lockstep.Behaviours.Data;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using Engine.Client.Ecsr.Systems;
using Engine.Client.Lockstep.Behaviours;
using Engine.Client.Lockstep;
using Engine.Client.Modules;
using Engine.Client.Network;
using Engine.Common;
using Engine.Common.Event;
using UnityEngine;
using static SelectableObject;
using Engine.Common.Lockstep;
using System.IO;
using Engine.Client.Protocol.Pt;
using Engine.Client.Ecsr.Components;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections.Generic;
using TrueSync;

public class Sample2 : Sample
{
    class SampleEntityInitializer : EntityInitializer
    {
        public SampleEntityInitializer(EntityWorld world) : base(world)
        {
        }
        public override EntityList OnCreateSelfEntityComponents(Guid entityId)
        {
            TSRandom tSRandom = TSRandom.New(GetHashCode());

            EntityList result = new EntityList();
            result.SetElements(new List<Entity>());

            Entity entity = new Entity();
            Appearance appearance = new Appearance();
            appearance.Resource = "Player";
            appearance.ShaderR = (byte)tSRandom.Next(0, 255);
            appearance.ShaderG = (byte)tSRandom.Next(0, 255);
            appearance.ShaderB = (byte)tSRandom.Next(0, 255);
            entity.AddComponent(appearance);
            Position position = new Position();
            position.Pos = new TrueSync.TSVector2(tSRandom.Next(-10f, 10f), tSRandom.Next(-7f, 7f));
            entity.AddComponent(position);
            Movement movement = new Movement();
            movement.Speed = 0.2f;
            movement.Direction = TSVector2.zero;
            entity.AddComponent(movement);
            result.Elements.Add(entity);

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
    class SampleEntityRenderSpawner : EntityRenderSpawner
    {
        private EntityWorld World;
        private Transform GameContainer;
        public SampleEntityRenderSpawner(EntityWorld world, Transform container)
        {
            World = world;
            GameContainer = container;
        }

        protected override void CreateEntityRendererImpl(CreateEntityRendererRequest request)
        {
            base.CreateEntityRendererImpl(request);
            GameObject resPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(request.ResourcePath));
            if (resPrefab.TryGetComponent<AppearanceRenderer>(out var component))
            {
                component.EntityId = request.EntityId;
                component.World = World;
                resPrefab.transform.SetParent(GameContainer);
            }
        }
    }

    void Awake()
    {
        MainContext = new Context(Context.CLIENT, new LiteNetworkClient(), new UnityLogger("Unity"));
        MainContext.SetMeta(ContextMetaId.STANDALONE_MODE_PORT, "50000")
                   .SetMeta(ContextMetaId.PERSISTENT_DATA_PATH, Application.persistentDataPath);
        MainContext.SetModule(new GateServiceModule())
                   .SetModule(new BattleServiceModule());
        DefaultSimulationController defaultSimulationController = new DefaultSimulationController();
        MainContext.SetSimulationController(defaultSimulationController);
        // create a default simulation.
        defaultSimulationController.CreateSimulation(new DefaultSimulation(), new EntityWorld(),
            new ISimulativeBehaviour[] {
                new LogicFrameBehaviour(),
                new RollbackBehaviour(),
                new EntityBehaviour(),
                new SelectionInputBehaviour(),
                new ButtonInputBehaviour(),
                new ComponentsBackupBehaviour(),
            },
            new IEntitySystem[]
            {
                new AppearanceSystem(),
                new MovementSystem(),
            });
        EntityWorld entityWorld = defaultSimulationController.GetSimulation<DefaultSimulation>().GetEntityWorld();
        entityWorld.SetEntityInitializer(new SampleEntityInitializer(entityWorld));
        entityWorld.SetEntityRenderSpawner(new SampleEntityRenderSpawner(entityWorld, GameContainer));
    }
    void Start()
    {
        EventDispatcher<SelectionEvent, SelectableObject>.AddListener(SelectionEvent.Select, OnSelectObject);
        EventDispatcher<SelectionEvent, SelectableObject>.AddListener(SelectionEvent.Deselect, OnDeselectObject);
    }

    void OnSelectObject(SelectableObject selectableObject)
    {
        if (selectableObject != null)
        {
            if(selectableObject.TryGetComponent<AppearanceRenderer>(out var component))
            {
                if(!Selection.SelectedIds.Contains(component.EntityId))
                {
                    Selection.SelectedIds.Add(component.EntityId);
                }
            }
        }
    }

    void OnDeselectObject(SelectableObject deselectableObject)
    {
        if(deselectableObject != null)
        {
            if(deselectableObject.TryGetComponent<AppearanceRenderer>(out var component))
            {
                Selection.SelectedIds.Remove(component.EntityId);
            }
        }
    }

    public override void DrawMap(uint mapId)
    {
        base.DrawMap(mapId);
        var path = Path.Combine(Application.persistentDataPath, "map", mapId + ".map");
        EntityList entityList = new EntityList().SetElements(new List<Entity>());
        PtMap map = new PtMap().SetVersion("0.0.1").SetEntities(entityList);
        File.WriteAllBytes(path, PtMap.Write(map));
    }

    public override void PlayReplay(string name)
    {
        base.PlayReplay(name);
    }
    protected override void Update()
    {
        base.Update();
    }
}
