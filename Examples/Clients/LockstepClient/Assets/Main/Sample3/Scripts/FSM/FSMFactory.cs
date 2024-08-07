using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using System;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public sealed class FSMFactory
{
    public enum NormalMonsterFsmState : int
    {
        Idle = 1,
        Pursuit = 2,
        Return = 4,
        Attack = 8,
    }
    static Dictionary<byte, FSM> fsms = new Dictionary<byte, FSM>();
    public static FSM Retrieve(EntityWorld world, byte fsmInfoType)
    {
        if (!fsms.ContainsKey(fsmInfoType))
        {
            FSM componentBasedFsm = new FSM(world);
            componentBasedFsm.OnReadComponents = (id,comps) =>
            {
                comps.Clear();
                comps.AddRange(world.ReadComponent(id,typeof(Position)));
            };
            FSM.State idle = componentBasedFsm.NewState((int)NormalMonsterFsmState.Idle);
            
            idle.OnEnter = (comps) => { };
            idle.OnUpdate = (comps) =>
            {
                world.RetrieveComponents<Position>(componentBasedFsm.EntityComponents);
                foreach (var pos in componentBasedFsm.EntityComponents)
                {
                    if (componentBasedFsm.Info != null && pos.Id != componentBasedFsm.EntityId)
                    {
                        Position selfPos = componentBasedFsm.Components[0] as Position;
                        if (selfPos != null)
                        {
                            if (TSVector2.Distance(selfPos.Pos, ((Position)pos.Component).Pos) < 1)
                            {
                                Debug.LogError("Distance < 1");
                            }
                        }
                    }
                }
            };
            FSM.State pursuit = componentBasedFsm.NewState((int)NormalMonsterFsmState.Pursuit);
            FSM.State retrn = componentBasedFsm.NewState((int)NormalMonsterFsmState.Return);
            FSM.State attack = componentBasedFsm.NewState((int)NormalMonsterFsmState.Attack);
            fsms[fsmInfoType] = componentBasedFsm;
        }
        return fsms[fsmInfoType];
    }
}