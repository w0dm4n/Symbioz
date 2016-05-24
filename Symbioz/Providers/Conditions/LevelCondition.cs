using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [ConditionAttribute("PL")]
    public class LevelCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return Condition.BasicEval(ConditionValue, ComparaisonSymbol, client.Character.Record.Level);
        }
    }
}
