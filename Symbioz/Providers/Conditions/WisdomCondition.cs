using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [Condition("cw")]
    public class WisdomCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
           return Condition.BasicEval(ConditionValue, ComparaisonSymbol, client.Character.StatsRecord.PermanentWisdom);
        }
    }
    [Condition("CW")]
    public class TotalWisdomCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return Condition.BasicEval(ConditionValue, ComparaisonSymbol, client.Character.StatsRecord.Wisdom);
        }
    }
}
