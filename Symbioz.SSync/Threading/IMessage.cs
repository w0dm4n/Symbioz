using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shader.SSync.Threading
{
    public interface IMessage
    {
        void Execute();
    }
}
