using System;
using System.Threading;
using System.Threading.Tasks;
using static Engine.Common.Lockstep.SimulationController;

namespace Engine.Common.Lockstep
{
    public class SimulationController
    {
        public enum RunState
        {
            Default,
            Running,
            Stopping,
            Stopped,
        }
        protected Simulation m_SimulationInstance;
        long m_AccumulatorTicks = 0;
        const int c_DefaultFrameMsLength = 20;
        int m_FrameMsLength = c_DefaultFrameMsLength;
        int FrameMsTickCount { get { return m_FrameMsLength * 10000; } }
        double m_FrameLerp = 0;
        public int GetFrameMsLength() { return m_FrameMsLength; }
        public double GetFrameLerp() { return m_FrameLerp; }
        public RunState State = RunState.Default;
        Thread m_RunnerThread;
        DateTime m_CurrentDateTime;
        
        public void UpdateFrameMsLength(float factor)
        {
            m_FrameMsLength = (int)(c_DefaultFrameMsLength / (factor + 0.5f));
        }
        public virtual void CreateSimulation(Simulation sim, ISimulativeBehaviour[] behaviours)
        {
            m_SimulationInstance = sim;
            for(int i = 0; i < behaviours.Length; ++i)
                sim.AddBehaviour(behaviours[i]);
        }
        public void Start(DateTime startDateTime,
                            int history_keyframes_count = 0,
                            Action<float> process = null,
                            Action runner = null)
        {
            m_CurrentDateTime = startDateTime;
            m_SimulationInstance.Start();
            for(int i=0; i<history_keyframes_count; i++)
            {
                m_SimulationInstance.Run();
                process?.Invoke(1f*i/history_keyframes_count);
            }
            State = RunState.Running;
            m_RunnerThread = new Thread(Run);
            m_RunnerThread.IsBackground = true;
            m_RunnerThread.Priority = ThreadPriority.Highest;
            m_RunnerThread.Start(runner);
        }

        public void Stop()
        {
            State = RunState.Stopping;
        }
        void Run(object runner)
        {
            Action action = runner as Action;
            while (State == RunState.Running)
            {
                DateTime now = DateTime.Now;
                m_AccumulatorTicks += (now - m_CurrentDateTime).Ticks;
                m_CurrentDateTime = now;
                while (m_AccumulatorTicks >= FrameMsTickCount)
                {
                    m_SimulationInstance.Run();
                    m_AccumulatorTicks -= FrameMsTickCount;
                }
                m_FrameLerp = m_AccumulatorTicks / FrameMsTickCount;
                Thread.Sleep(30);
                if (action != null)
                {
                    action();
                    action = null;
                }
            }
            State = RunState.Stopped;
        }
        public override string ToString()
        {
            return $"DefaultFrameMsLentgh:{c_DefaultFrameMsLength} Simulation:{m_SimulationInstance}";
        }
        public S GetSimulation<S>() where S:Simulation
        {
            return (S)m_SimulationInstance;
        }
    }
}
