using Symbioz.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider
{
    public class EffectHandler : Attribute
    {
        public EffectsEnum Effect { get; set; }
        public EffectHandler(EffectsEnum effect)
        {
            this.Effect = effect;
        }
    }
}
