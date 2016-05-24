using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [Condition("cc")]
    public class ChanceCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return Condition.BasicEval(ConditionValue, ComparaisonSymbol, client.Character.StatsRecord.PermanentChance);
        }
    }
    [Condition("CC")]
    public class TotalChanceCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return Condition.BasicEval(ConditionValue, ComparaisonSymbol, client.Character.StatsRecord.Chance);
        }
    }
}
