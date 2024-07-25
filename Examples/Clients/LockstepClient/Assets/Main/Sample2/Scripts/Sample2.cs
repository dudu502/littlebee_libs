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

public class Sample2 : Sample
{
    class SampleEntityInitializer : EntityInitializer
    {
        public SampleEntityInitializer(EntityWorld world) : base(world)
        {
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

    protected override void Update()
    {
        base.Update();
    }
}
