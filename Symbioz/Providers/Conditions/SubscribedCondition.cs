using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [Condition("PZ")]
    class SubscribedCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return true;
        }
    }
}
