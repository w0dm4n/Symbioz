using Shader.SSync.Threading;
using Shader.SSync.Timers;
using Symbioz;
using Symbioz.SSync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shader.SSync
{
    public class SelfRunningTaskPool 
    {
       // private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly LockFreeQueue<IMessage> m_messageQueue;
        private readonly Stopwatch m_queueTimer;
        private readonly List<SimpleTimerEntry> m_simpleTimers;
        private readonly ManualResetEvent m_stoppedAsync = new ManualResetEvent(false);
        private readonly List<TimerEntry> m_timers;
        private int m_currentThreadId;
        private int m_lastUpdate;
        private Task m_updateTask;
        public string Name
        {
            get;
            set;
        }
        public int UpdateInterval
        {
            get;
            set;
        }
        public long LastUpdateTime
        {
            get
            {
                return (long)m_lastUpdate;
            }
        }
        public bool IsRunning
        {
            get;
            protected set;
        }
        public ReadOnlyCollection<TimerEntry> Timers
        {
            get
            {
                return m_timers.AsReadOnly();
            }
        }
        public ReadOnlyCollection<SimpleTimerEntry> SimpleTimers
        {
            get
            {
                return m_simpleTimers.AsReadOnly();
            }
        }
        public bool IsInContext
        {
            get
            {
                return Thread.CurrentThread.ManagedThreadId == m_currentThreadId;
            }
        }

        public SelfRunningTaskPool(int interval, string name)
        {
            m_messageQueue = new LockFreeQueue<IMessage>();
            m_queueTimer = Stopwatch.StartNew();
            m_simpleTimers = new List<SimpleTimerEntry>();
            m_timers = new List<TimerEntry>();
            UpdateInterval = interval;
            Name = name;
            Start();
        }

        public void AddMessage(IMessage message)
        {
            m_messageQueue.Enqueue(message);
        }

        public void AddMessage(Action action)
        {
            m_messageQueue.Enqueue(new Message(action));
        }

        public bool ExecuteInContext(Action action)
        {
            bool result;
            if (IsInContext)
            {
                action();
                result = true;
            }
            else
            {
                AddMessage(action);
                result = false;
            }
            return result;
        }

        public void EnsureContext()
        {
            if (!IsInContext)
            {
                throw new InvalidOperationException("Not in context");
            }
        }

        public void Start()
        {
            IsRunning = true;
            m_updateTask = Task.Factory.StartNewDelayed(UpdateInterval, new Action<object>(ProcessCallback), this);
        }

        public void Stop(bool async = false)
        {
            IsRunning = false;
            if (async && m_currentThreadId != 0)
            {
                m_stoppedAsync.WaitOne();
            }
        }

        public void EnsureNotContext()
        {
            if (IsInContext)
            {
                throw new InvalidOperationException("Forbidden context");
            }
        }

        public void AddTimer(TimerEntry timer)
        {
            AddMessage(delegate
            {
                m_timers.Add(timer);
            });
        }

        public void RemoveTimer(TimerEntry timer)
        {
            AddMessage(delegate
            {
                m_timers.Remove(timer);
            });
        }

        public void CancelSimpleTimer(SimpleTimerEntry timer)
        {
            m_simpleTimers.Remove(timer);
        }

        public SimpleTimerEntry CallPeriodically(int delayMillis, Action callback)
        {
            SimpleTimerEntry simpleTimerEntry = new SimpleTimerEntry(delayMillis, callback, LastUpdateTime, false);
            m_simpleTimers.Add(simpleTimerEntry);
            return simpleTimerEntry;
        }

        public SimpleTimerEntry CallDelayed(int delayMillis, Action callback)
        {
            SimpleTimerEntry simpleTimerEntry = new SimpleTimerEntry(delayMillis, callback, LastUpdateTime, true);
            m_simpleTimers.Add(simpleTimerEntry);
            return simpleTimerEntry;
        }

        internal int GetDelayUntilNextExecution(SimpleTimerEntry timer)
        {
            return timer.Delay - (int)(LastUpdateTime - timer.LastCallTime);
        }

        protected void ProcessCallback(object state)
        {
            if (IsRunning && Interlocked.CompareExchange(ref m_currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) == 0)
            {
                long num = 0L;
                try
                {
                    num = m_queueTimer.ElapsedMilliseconds;
                    int dtMillis = (int)(num - (long)m_lastUpdate);
                    m_lastUpdate = (int)num;
                    foreach (TimerEntry current in m_timers)
                    {
                        try
                        {
                            current.Update(dtMillis);
                        }
                        catch (Exception argument)
                        {
                            Console.WriteLine(string.Format("Failed to update {0} : {1}", current, argument));
                            
                        }
                        if (!IsRunning)
                        {
                            return;
                        }
                    }
                    int count = m_simpleTimers.Count;
                    int num2 = count - 1;
                    while (num2 >= 0)
                    {
                        SimpleTimerEntry simpleTimerEntry = m_simpleTimers[num2];
                        if (GetDelayUntilNextExecution(simpleTimerEntry) > 0)
                        {
                            goto IL_107;
                        }
                        try
                        {
                            simpleTimerEntry.Execute(this);
                            goto IL_107;
                        }
                        catch (Exception argument)
                        {
                            Console.WriteLine(string.Format("Failed to execute timer {0} : {1}", simpleTimerEntry, argument));

                            goto IL_107;
                        }
                    IL_FF:
                        num2--;
                        continue;
                    IL_107:
                        if (!IsRunning)
                        {
                            return;
                        }
                        goto IL_FF;
                    }
                    IMessage message;
                    while (m_messageQueue.TryDequeue(out message))
                    {
                        try
                        {
                            message.Execute();
                        }
                        catch (Exception argument)
                        {
                            Console.WriteLine(string.Format("Failed to execute message {0} : {1}", message, argument));

                        }
                        if (!IsRunning)
                        {
                            break;
                        }
                    }
                }
                catch (Exception argument)
                {
                    Console.WriteLine(string.Format("Failed to run TaskQueue callback for \"{0}\" : {1}", Name, argument));

                }
                finally
                {
                    long elapsedMilliseconds = m_queueTimer.ElapsedMilliseconds;
                    bool flag;
                    long num3 = (flag = (elapsedMilliseconds - num > (long)UpdateInterval)) ? 0L : (num + (long)UpdateInterval - elapsedMilliseconds);
                    Interlocked.Exchange(ref m_currentThreadId, 0);
                    if (flag)
                    {
                        Console.WriteLine(string.Format("TaskPool '{0}' update lagged ({1}ms)", Name, elapsedMilliseconds - num));

                    }
                    if (IsRunning)
                    {
                        m_updateTask = Task.Factory.StartNewDelayed((int)num3, new Action<object>(ProcessCallback), this);
                    }
                    else
                    {
                        m_stoppedAsync.Set();
                    }
                }
            }
        }
    }
}
