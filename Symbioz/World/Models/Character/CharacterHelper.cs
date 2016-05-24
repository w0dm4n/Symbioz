using Symbioz.DofusProtocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models
{
    class CharacterHelper
    {
        public static ActorRestrictionsInformations GetDefaultRestrictions()
        {
            return new ActorRestrictionsInformations(false, false, false, false, false, false, false, false, true, false, false, false, false, true, true, true, false, false, false, false, false);
        }
    }
}
