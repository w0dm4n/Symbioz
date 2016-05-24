using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Symbioz.Helper
{
    /// <summary>
    /// From Bouh2
    /// </summary>
    public class UIDProvider
    {
        protected readonly ConcurrentQueue<int> m_freeIds = new ConcurrentQueue<int>();
        protected int m_highestId;

        public UIDProvider()
        {
            this.m_highestId = 0;
        }

        public UIDProvider(int lastId)
        {
            this.m_highestId = lastId;
        }

        public UIDProvider(IEnumerable<int> freeIds)
        {
            foreach (int current in freeIds)
            {
                this.m_freeIds.Enqueue(current);
            }
        }

        public virtual int Pop()
        {
            int result;
            if (!this.m_freeIds.IsEmpty)
            {
                int num;
                if (!this.m_freeIds.TryDequeue(out num))
                {
                    result = this.Next();
                }
                else
                {
                    result = num;
                }
            }
            else
            {
                result = this.Next();
            }
            return result;
        }

        public virtual int Peek()
        {
            int result;
            if (!this.m_freeIds.IsEmpty)
            {
                int num;
                if (!this.m_freeIds.TryPeek(out num))
                {
                    result = this.m_highestId + 1;
                }
                else
                {
                    result = num;
                }
            }
            else
            {
                result = this.m_highestId + 1;
            }
            return result;
        }

        public virtual void Push(int freeId)
        {
            this.m_freeIds.Enqueue(freeId);
        }

        protected virtual int Next()
        {
            return Interlocked.Increment(ref this.m_highestId);
        }
    }
}