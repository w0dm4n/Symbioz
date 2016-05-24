using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Core.Startup
{
    public class StartupInvoke : Attribute
    {
        public StartupInvokeType Type { get; set; }
        public bool Hided { get; set; }
        public string Name { get; set; }
        public StartupInvoke(string name,StartupInvokeType type)
        {
            this.Type = type;
            this.Name = name;
            this.Hided = false;
        }
        public StartupInvoke(StartupInvokeType type)
        {
            this.Hided = true;
            this.Type = type;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
