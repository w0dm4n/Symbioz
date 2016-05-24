using Symbioz.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.CustomSpellsEffectsProvider
{
    public class CustomSpell : Attribute
    {
        public SpellIdEnum SpellId { get; set; }
        public CustomSpell(SpellIdEnum spellid)
        {
            this.SpellId = spellid;
        }
    }
}
