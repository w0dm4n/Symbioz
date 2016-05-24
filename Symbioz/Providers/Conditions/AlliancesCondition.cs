using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Network.Clients;

namespace Symbioz.Providers.Conditions
{
    [Condition("Ow")]
    class AlliancesCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return client.Character.HasAlliance;
        }
    }
}
