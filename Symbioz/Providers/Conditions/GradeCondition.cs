using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [Condition("PP")]
    public class GradeCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
           return Condition.BasicEval(ConditionValue, ComparaisonSymbol, client.Character.Record.AlignmentGrade);
        }
    }
}
