using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models
{
    public class BasicStats
    {
        public BasicStats(ushort energy,uint lifepoints)
        {
            this.Energy = energy;
            this.LifePoints = lifepoints;
        }
        public ushort Energy { get; set; }
        public uint LifePoints { get; set; }
    }
}
