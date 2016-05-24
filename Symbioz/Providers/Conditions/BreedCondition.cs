using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [Condition("PG")]
    public class BreedCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            if (client.Character.Record.Breed == sbyte.Parse(ConditionValue))
                return true;
            else
                return false;
        }
    }
}
