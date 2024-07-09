using System;
using System.Threading;

namespace Engine.Common.Lockstep
{
    public class SimulationController
    {
        protected Simulation m_SimulationInstance;
        long m_AccumulatorTicks = 0;
        const int c_DefaultFrameMsLength = 20;
        int m_FrameMsLength = c_DefaultFrameMsLength;
        int FrameMsTickCount { get { return m_FrameMsLength * 10000; } }
        double m_FrameLerp = 0;
        public int GetFrameMsLength() { return m_FrameMsLength; }
        public double GetFrameLerp() { return m_FrameLerp; }
        public bool IsRunning { private set; get; }
        Thread m_RunnerThread;
        DateTime m_CurrentDateTime;
        
        public void UpdateFrameMsLength(float factor)
        {
            m_FrameMsLength = (int)(c_DefaultFrameMsLength / (factor + 0.5f));
        }
        public virtual void CreateSimulation()
        {

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
            IsRunning = true;
            m_RunnerThread = new Thread(Run);
            m_RunnerThread.IsBackground = true;
            m_RunnerThread.Priority = ThreadPriority.Highest;
            m_RunnerThread.Start(runner);
        }
        public void Stop()
        {
            IsRunning = false;
        }
        void Run(object runner)
        {
            while (IsRunning)
            {
                DateTime now = DateTime.Now;
                m_AccumulatorTicks += (now - m_CurrentDateTime).Ticks;
                m_CurrentDateTime = now;
                while (m_AccumulatorTicks >= FrameMsTickCount)
                {
                    m_SimulationInstance.Run();
                    m_AccumulatorTicks -= FrameMsTickCount;
                }
                m_FrameLerp = m_AccumulatorTicks/FrameMsTickCount;
                Thread.Sleep(30);
                if (runner != null)
                {
                    ((Action)runner)();
                    runner = null;
                }
            }
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
