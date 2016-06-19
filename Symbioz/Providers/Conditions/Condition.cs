using Shader.Helper;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    public abstract class Condition
    {
        public char ComparaisonSymbol { get { return ConditionFull.Remove(0, 2).Take(1).ToArray()[0]; } }
        public string ConditionValue { get { return ConditionFull.Remove(0, 3); } }
        public string ConditionFull { get; set; }
        public abstract bool Eval(WorldClient client);

        public static bool BasicEval(string conditionvalue,char comparaisonsymbol,int delta)
        {
            var conditionalDelta = 0;
            if (DateTimeUtils.IsNumeric(conditionvalue))
                conditionalDelta = int.Parse(conditionvalue);
            else
            {
                var Array = conditionvalue.Split('|');
                if (Array.Count() > 1)
                {
                    if (DateTimeUtils.IsNumeric(Array[0]))
                        conditionalDelta = int.Parse(Array[0]);
                }
            }
            switch (comparaisonsymbol)
            {
                case '=':
                    if (delta == conditionalDelta)
                        return true;
                    break;
                case '<':
                    if (delta < conditionalDelta)
                        return true;
                    break;
                case '>':
                    if (delta > conditionalDelta)
                        return true;
                    break;
            }
            return false;
        }
    }
}
