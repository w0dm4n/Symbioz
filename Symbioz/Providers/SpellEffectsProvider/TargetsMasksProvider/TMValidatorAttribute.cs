using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.TargetsMasksProvider
{
    public class TMValidator : Attribute
    {
        public string Identifier { get; set; }
        public TMValidator(string identifier)
        {
            this.Identifier = identifier;
        }
    }
}
