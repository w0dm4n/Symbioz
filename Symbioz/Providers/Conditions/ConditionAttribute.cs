using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    public class ConditionAttribute : Attribute
    {
        public string Identifier { get; set; }
        public ConditionAttribute(string indentifier)
        {
            this.Identifier = indentifier;
        }
    }
}
