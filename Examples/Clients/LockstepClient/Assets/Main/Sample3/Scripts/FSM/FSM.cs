using Engine.Client.Ecsr.Components;
using Engine.Client.Ecsr.Entitas;
using System;
using System.Collections.Generic;


public class FSM
{
    public class State
    {
        public int PreviousId;
        public int Id;
        public Action<List<AbstractComponent>> OnEarlyUpdate;
        public Action<List<AbstractComponent>> OnEnter;
        public Action<List<AbstractComponent>> OnUpdate;
        public Action<List<AbstractComponent>> OnExit;
        public State(int id)
        {
            Id = id;
        }
    }
    public class Transition
    {
        public int NextId;
        public Action<List<AbstractComponent>> OnTransfer;
        public Func<List<AbstractComponent>, bool> OnValidate;
        public Transition(int nextId, Func<List<AbstractComponent>, bool> onValidate, Action<List<AbstractComponent>> onTransfer)
        {
            NextId = nextId;
            OnTransfer = onTransfer;
            OnValidate = onValidate;
        }
    }
    public EntityWorld World { get; private set; }
    public Guid EntityId { get; set; }

    public FSMInfo Info { get; set; }

    public Dictionary<int, State> States = new Dictionary<int, State>();

    private Dictionary<int, List<Transition>> m_Transitions = new Dictionary<int, List<Transition>>();
    public FSM(EntityWorld world)
    {
        World = world;
    }

    private FSM() { }

    public State NewState(int id)
    {
        States[id] = new State(id);
        return States[id];
    }
    public void AddReturnTransition(int id, Func<List<AbstractComponent>, bool> onValidate, Action<List<AbstractComponent>> onTransfer)
    {
        AddTransition(id, -1, onValidate, onTransfer);
    }
    public void AddTransition(int id, int nextId, Func<List<AbstractComponent>, bool> onValidate, Action<List<AbstractComponent>> onTransfer)
    {
        if (!m_Transitions.ContainsKey(id))
            m_Transitions[id] = new List<Transition>();
        m_Transitions[id].Add(new Transition(nextId, onValidate, onTransfer));
    }
    public List<AbstractComponent> Components = new List<AbstractComponent>();
    public Action<Guid,List<AbstractComponent>> OnReadComponents;
    public List<EntityComponent> EntityComponents = new List<EntityComponent>();
    public void Execute()
    {
        if (Info != null && States.TryGetValue(Info.CurrentId, out State current) && m_Transitions.TryGetValue(Info.CurrentId, out List<Transition> transitions))
        {
            OnReadComponents?.Invoke(EntityId, Components);
            current.OnEarlyUpdate?.Invoke(Components);
            foreach (Transition transition in transitions)
            {
                if (transition.OnValidate != null && transition.OnValidate(Components))
                {
                    current.OnExit?.Invoke(Components);
                    int nextId = transition.NextId == -1 ? current.PreviousId : transition.NextId;
                    if (States.TryGetValue(nextId, out State next))
                    {
                        int previousId = current.Id;
                        Info.SetCurrentId(nextId);
                        next.PreviousId = previousId;
                        transition.OnTransfer?.Invoke(Components);
                        next.OnEnter?.Invoke(Components);
                    }
                    else
                    {
                        throw new Exception("User NewState to define a state." + nextId);
                    }
                    return;
                }
            }
            current.OnUpdate?.Invoke(Components);
        }
    }
}