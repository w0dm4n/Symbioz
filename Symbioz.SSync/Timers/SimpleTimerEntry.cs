using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shader.SSync.Timers
{
    public class SimpleTimerEntry
    {
        public readonly bool IsOneShot;
        public long LastCallTime
        {
            get;
            private set;
        }
        public Action Callback
        {
            get;
            set;
        }
        public int Delay
        {
            get;
            set;
        }

        internal SimpleTimerEntry(int delayMillis, Action callback, long time, bool isOneShot)
        {
            this.Callback = callback;
            this.Delay = delayMillis;
            this.LastCallTime = time;
            this.IsOneShot = isOneShot;
        }

        internal void Execute(SelfRunningTaskPool queue)
        {
            try
            {
                this.Callback();
            }
            finally
            {
                this.LastCallTime = queue.LastUpdateTime;
            }
            if (this.IsOneShot)
            {
                queue.CancelSimpleTimer(this);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is SimpleTimerEntry && this.Callback == ((SimpleTimerEntry)obj).Callback;
        }

        public override int GetHashCode()
        {
            return this.Callback.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} (Callback = {1}, Delay = {2})", base.GetType(), this.Callback, this.Delay);
        }
    }
}
