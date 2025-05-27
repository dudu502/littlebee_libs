using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using Engine.Client.Ecsr.Renders;
using Engine.Client.Ecsr.Systems;
using Engine.Client.Lockstep.Behaviours;
using Engine.Client.Lockstep;
using Engine.Client.Modules;
using Engine.Client.Network;
using Engine.Client.Protocol.Pt;
using Engine.Common;
using Engine.Common.Lockstep;
using Engine.Common.Protocol.Pt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TrueSync;
using UnityEngine;

public class Sample3 : Sample
{
    public class GameEntityInitializer : EntityInitializer
    {
        public GameEntityInitializer(EntityWorld world) : base(world)
        {

        }

        public override EntityList OnCreateSelfEntityComponents(Guid entityId)
        {
            int theSeed = entityId.GetHashCode(); //�������������Լ�����ʱ����Ҫʹ����ͬ�����������
            Debug.LogWarning("OnCreateSelfEntityComponents " + entityId.ToString());
            TSRandom tSRandom = TSRandom.New(theSeed);
            List<Entity> entities = new List<Entity>();
            EntityList result = new EntityList();
            result.SetElements(entities);


            for (int i = 0; i < 1; ++i)
            {
                Entity entity = new Entity();
                BasicAttributes attributes = new BasicAttributes();
                attributes.SetEntityId(entityId.ToString());
                entity.AddComponent(attributes);
                Appearance appearance = new Appearance();
                appearance.SetResource("Player");
                appearance.SetShaderR((byte)tSRandom.Next(0, 255));
                appearance.SetShaderG((byte)tSRandom.Next(0, 255));
                appearance.SetShaderB((byte)tSRandom.Next(0, 255));
                entity.AddComponent(appearance);
              
                Position position = new Position();
                position.SetPos(new TrueSync.TSVector2(tSRandom.Next(-10f, 10f), tSRandom.Next(-7f, 7f)));
                entity.AddComponent(position);

                Movement movement = new Movement();
                movement.SetSpeed(tSRandom.Next(0.1f, 0.5f));
                movement.SetDirection(new TrueSync.TSVector2(0.1f * tSRandom.Next(-1f, 1f), 0.1f * tSRandom.Next(-1f, 1f)));
                entity.AddComponent(movement);

                FSMInfo info = new FSMInfo();
                info.SetInfoType(1);
                info.SetCurrentId(0);
                entity.AddComponent(info);

                entities.Add(entity);
            }

            return result;
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
            if (request.Status == Appearance.StatusStartLoading)
            {
                GameObject resPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(request.ResourcePath));
                if (resPrefab.TryGetComponent<AppearanceRenderer>(out var component))
                {
                    component.EntityId = request.EntityId;
                    component.World = entityWorld;
                    resPrefab.transform.SetParent(gameContainer);
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
        defaultSimulationController.CreateSimulation(new DefaultSimulation(), new EntityWorld(),
            new ISimulativeBehaviour[] 
            {
                new FrameReceiverBehaviour(),
                new EntityBehaviour(),
                new InputBehaviour(),
                new FrameSenderBehaviour(),
            },
            new IEntitySystem[]
            {
                new AppearanceSystem(),
                new FSMSystem(),
                new PathFindingSystem(),
                new MovementSystem(),       
            });
        EntityWorld entityWorld = defaultSimulationController.GetSimulation<DefaultSimulation>().GetEntityWorld();
        entityWorld.SetEntityInitializer(new GameEntityInitializer(entityWorld));
        entityWorld.SetEntityRenderSpawner(new GameEntityRenderSpawner(entityWorld, GameContainer));

    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
}
