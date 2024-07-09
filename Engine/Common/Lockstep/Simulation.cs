using System.Collections.Generic;
using System.Linq;

namespace Engine.Common.Lockstep
{
    /// <summary>
    /// Simulate all behaviour in order.
    /// </summary>
    public class Simulation
    {
        List<ISimulativeBehaviour> m_Behaviours;
        public Simulation()
        {
            m_Behaviours = new List<ISimulativeBehaviour>();
        }

        public void Start()
        {
            foreach (ISimulativeBehaviour beh in m_Behaviours)
                beh.Start();
        }
        public T GetBehaviour<T>() where T : ISimulativeBehaviour
        {
            foreach (ISimulativeBehaviour beh in m_Behaviours)
            {
                if (beh.GetType() == typeof(T)) return (T)beh;
            }
            return default(T);
        }
        public bool ContainBehaviour(ISimulativeBehaviour beh)
        {
            foreach (ISimulativeBehaviour item in m_Behaviours)
            {
                if (item == beh) return true;
                if (item.GetType() == beh.GetType()) return true;
            }
            return false;
        }
        public void AddBehaviour(ISimulativeBehaviour beh)
        {
            if (!ContainBehaviour(beh))
            {
                m_Behaviours.Add(beh);
                beh.Sim = this;
            }
        }
        public void RemoveBehaviour(ISimulativeBehaviour beh)
        {
            if (ContainBehaviour(beh))
            {
                m_Behaviours.Remove(beh);
                beh.Sim = null;
            }
        }
        public void Run()
        {
            int behaviourCount = m_Behaviours.Count;
            for (int i = 0; i < behaviourCount; ++i)
                m_Behaviours[i].Update();
        }
        public virtual void Dispose()
        {
            m_Behaviours.Clear();
            m_Behaviours = null;
        }

        public override string ToString()
        {
            return $"ISimulativeBehaviour Count:{m_Behaviours.Count} [{string.Join(",",m_Behaviours.Select(b=>b.GetType().ToString()))}]";
        }
    }
}
