using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shader.SSync.Threading
{
    public class Message : IMessage
    {
        public Action Callback
        {
            get;
            private set;
        }

        public static Message Obtain(Action callback)
        {
            return new Message(callback);
        }

        public Message()
        {
        }

        public Message(Action callback)
        {
            this.Callback = callback;
        }

        public virtual void Execute()
        {
            Action callback = this.Callback;
            if (callback != null)
            {
                callback();
            }
        }

        public static implicit operator Message(Action dele)
        {
            return new Message(dele);
        }

        public override string ToString()
        {
            return this.Callback.ToString();
        }
    }
}
