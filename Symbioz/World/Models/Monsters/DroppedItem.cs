using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Monsters
{
    public class DroppedItem
    {
        public DroppedItem(ushort gid,uint quantity)
        {
            this.GID = gid;
            this.Quantity = quantity;
        }
        public ushort GID { get; set; }
        public uint Quantity { get; set; }
    }
}
