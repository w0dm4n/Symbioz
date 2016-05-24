using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.FightLooksProvider
{
    public class FightLook : Attribute
    {
        public ushort SpellId { get; set; }
        public FightLook(ushort spellid)
        {
            this.SpellId = spellid;
        }
    }
}
