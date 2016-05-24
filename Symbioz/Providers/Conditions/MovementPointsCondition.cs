using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [Condition("CM")]
    class MovementPointsCondition : Condition 
    {
        public override bool Eval(WorldClient client)
        {
            return BasicEval(ConditionValue, ComparaisonSymbol, client.Character.StatsRecord.MovementPoints);
        }
    }
}
