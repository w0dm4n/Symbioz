using Symbioz.Providers.ActorIA.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA
{
    public class IAAction : Attribute
    {
        public IAActionsEnum Action;
        public IAAction(IAActionsEnum action)
        {
            this.Action = action;
        }
    }
}
