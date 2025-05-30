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
using System.Threading.Tasks;

public class Sample2 : Sample
{
    class SampleEntityInitializer : EntityInitializer
    {
        public SampleEntityInitializer(EntityWorld world) : base(world)
        {
        }
        public override EntityList OnCreateSelfEntityComponents(Guid entityId)
        {
            Debug.LogWarning("OnCreateSelfEntityComponents" + entityId+" hash:"+entityId.GetHashCode()) ;
            TSRandom tSRandom = TSRandom.New(entityId.GetHashCode());

            EntityList result = new EntityList();
            result.SetElements(new List<Entity>());

            Entity entity = new Entity();
            BasicAttributes attributes = new BasicAttributes();
            attributes.SetEntityId(entityId.ToString());
            attributes.SetSelectable(true);
            entity.AddComponent(attributes);
            Appearance appearance = new Appearance();
            appearance.SetResource("SelectablePlayer");
            appearance.SetShaderR ( 255);
            appearance.SetShaderG(255);
            appearance.SetShaderB(255);
            entity.AddComponent(appearance);
            Position position = new Position();
            position.SetPos(new TrueSync.TSVector2(tSRandom.Next(-10f, 10f), tSRandom.Next(-7f, 7f)));
            entity.AddComponent(position);
            Movement movement = new Movement();
            movement.SetSpeed(0.01f);
            movement.SetDirection(TSVector2.zero);
            entity.AddComponent(movement);
            result.Elements.Add(entity);
            Debug.LogWarning("Init Pos:"+position.Pos.ToString());
            return result;
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
            if (request.Status == Appearance.StatusStartLoading)
            {
                GameObject resPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(request.ResourcePath));
                if (resPrefab.TryGetComponent<AppearanceRenderer>(out var component))
                {
                    component.EntityId = request.EntityId;
                    component.World = World;
                    resPrefab.transform.SetParent(GameContainer);
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
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
                new FrameReceiverBehaviour(),
                new EntityBehaviour(),
                new InputBehaviour(),
                new FrameSenderBehaviour(),
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
    protected override void Start()
    {
        base.Start();
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

    private float selectedObjectSpeed = 0.03f;
    [TerminalCommand("setspeed", "setspeed(spd) set selected object speed")]
    public void SetSpeed(float speed)
    {
        selectedObjectSpeed = speed;
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

    public override void DrawMap()
    {
        base.DrawMap();
        var path = Path.Combine(Application.persistentDataPath, "map", MapId + ".map");
        EntityList entityList = new EntityList().SetElements(new List<Entity>());
        PtMap map = new PtMap().SetVersion("0.0.1").SetEntities(entityList);
        File.WriteAllBytes(path, PtMap.Write(map));
    }

    async public override void PlayReplay(string name)
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
            });
        EntityWorld entityWorld = simulationController.GetSimulation<DefaultSimulation>().GetEntityWorld();
        entityWorld.SetEntityInitializer(new SampleEntityInitializer(entityWorld));
        entityWorld.SetEntityRenderSpawner(new SampleEntityRenderSpawner(entityWorld, GameContainer));
        // load map
        await entityWorld.GetEntityInitializer().CreateEntities(replay.MapId);
        // load init entities;
        entityWorld.GetEntityInitializer().InitEntities = replay.InitEntities;
        entityWorld.GetEntityInitializer().CreateEntities(replay.InitEntities);
        simulationController.GetSimulation().GetBehaviour<ReplayLogicFrameBehaviour>().SetFrameIdxInfos(replay.Frames);
        simulationController.Start(DateTime.Now);
    }

    List<PtFrame> ptFrames = new List<PtFrame>();
    protected override void Update()
    {
        base.Update();

        UpdateInputs();
        
    }


    void UpdateInputs()
    {
        bool hasCommand = false;
        if (MainContext == null) return;
        if (MainContext.GetSimulationController() == null) return;
        if (MainContext.GetSimulationController().GetSimulation<DefaultSimulation>() == null) return;
        if (MainContext.GetSimulationController().GetSimulation<DefaultSimulation>().GetEntityWorld() == null) return;
        EntityWorld entityWorld = MainContext.GetSimulationController().GetSimulation<DefaultSimulation>().GetEntityWorld();
        ptFrames.Clear();
        if (Selection.SelectedIds.Count > 0)
        {
    
            var dir = new TSVector2(0, 0);
            if (Input.GetKeyDown(KeyCode.W))
            {
                dir.Set(0, 1);
                hasCommand = true;
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                dir.Set(0, 0);
                hasCommand = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                dir.Set(0, -1);
                hasCommand = true;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                dir.Set(0, 0);
                hasCommand = true;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                dir.Set(-1, 0);
                hasCommand = true;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                dir.Set(0, 0);
                hasCommand = true;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                dir.Set(1, 0);
                hasCommand = true;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                dir.Set(0, 0);
                hasCommand = true;
            }

            if (hasCommand)
            {
                foreach (Guid entityId in Selection.SelectedIds)
                {
                    var comps = entityWorld.ReadComponent<BasicAttributes, Movement>(entityId);
                    if (comps.Item1 != null && comps.Item1.Selectable && comps.Item2 != null)
                    {
                        Movement deltaMovement = new Movement().SetDirection(dir).SetSpeed(selectedObjectSpeed);

                        PtFrame frame = new PtFrame().SetEntityId(entityId.ToString());
                        if (frame.Updaters == null)
                            frame.SetUpdaters(new PtComponentUpdaterList().SetElements(new List<PtComponentUpdater>()));
                        frame.Updaters.Elements.Add(new PtComponentUpdater()
                                                                    .SetComponentClsName(typeof(Movement).AssemblyQualifiedName)
                                                                    .SetParamContent(deltaMovement.Serialize()));
                        ptFrames.Add(frame);

                        Debug.LogWarning("Push PtFrame Id:"+ frame.EntityId+" " + frame.Updaters.Elements[0].ComponentClsName);
                    }
                }
            }
            if (ptFrames.Count > 0)
            {
                foreach (var frame in ptFrames)
                {
                    Engine.Client.Lockstep.Behaviours.Data.Input.InputFrames.Enqueue(frame);
                }              
            }

            //Debug.LogWarning("hasCommand "+ hasCommand+ " ptFramesCount:"+ ptFrames.Count);
        }
    }
}
