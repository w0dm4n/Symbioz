using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Marks
{
    public class InteractionAttribute : Attribute
    {
        public FighterEventType EventType { get; set; }
        public InteractionAttribute(FighterEventType type)
        {
            this.EventType = type;
        }
    }
}
