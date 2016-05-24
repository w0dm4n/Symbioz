using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    [ConditionAttribute("PO")]
    public class HasItemCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            ushort gid = ushort.Parse(ConditionFull.Remove(0,3));
            if (client.Character.Inventory.HasItem(gid))
                return true;
            else
                return false;
        }
    }
}
