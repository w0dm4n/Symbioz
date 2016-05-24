using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.TargetsMasksProvider
{
    public class TargetMask : Attribute
    {
        public string Identifier { get; set; }

        public TargetMask(string identifier)
        {
            this.Identifier = identifier;
        }
    }
}
