using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [ConditionAttribute("cs")]
    public class StrengthCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return Condition.BasicEval(ConditionValue, ComparaisonSymbol, client.Character.StatsRecord.PermanentStrenght);
        }
    }
    [ConditionAttribute("CS")]
    public class TotalStrengthCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return Condition.BasicEval(ConditionValue, ComparaisonSymbol, client.Character.StatsRecord.Strength);
        }
    }
}
