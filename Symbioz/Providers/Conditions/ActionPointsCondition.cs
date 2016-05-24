using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [Condition("CP")]
    public class ActionPointsCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return BasicEval(ConditionValue, ComparaisonSymbol, client.Character.StatsRecord.ActionPoints);
        }
    }
}
