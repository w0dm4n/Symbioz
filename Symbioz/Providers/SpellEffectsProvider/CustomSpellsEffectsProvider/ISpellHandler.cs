using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.CustomSpellsEffectsProvider
{
    public interface ISpellHandler
    {
        void Cast(Fighter fighter,List<ExtendedSpellEffect> effects);
    }
}
