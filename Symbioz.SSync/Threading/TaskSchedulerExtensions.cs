using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shader.SSync.Threading
{
    public static class TaskSchedulerExtensions
    {
        private sealed class TaskSchedulerSynchronizationContext : SynchronizationContext
        {
            private readonly TaskScheduler m_scheduler;

            internal TaskSchedulerSynchronizationContext(TaskScheduler scheduler)
            {
                if (scheduler == null)
                {
                    throw new ArgumentNullException("scheduler");
                }
                this.m_scheduler = scheduler;
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                Task.Factory.StartNew(delegate
                {
                    d(state);
                }, CancellationToken.None, TaskCreationOptions.None, this.m_scheduler);
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                Task task = new Task(delegate
                {
                    d(state);
                });
                task.RunSynchronously(this.m_scheduler);
                task.Wait();
            }
        }

        public static SynchronizationContext ToSynchronizationContext(this TaskScheduler scheduler)
        {
            return new TaskSchedulerExtensions.TaskSchedulerSynchronizationContext(scheduler);
        }
    }
}
