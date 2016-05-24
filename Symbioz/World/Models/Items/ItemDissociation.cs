using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models
{
    class ItemCut
    {
        public CharacterItemRecord newItem { get; set; }
        public CharacterItemRecord BaseItem { get; set; }
        public ItemCut(CharacterItemRecord item,uint quantity,byte newItempos)
        {
            
            newItem = item.CloneAndGetNewUID();
            item.Position = newItempos;
            item.Quantity = quantity;

            newItem.Quantity -= quantity;
            BaseItem = item;
            SaveTask.UpdateElement(BaseItem);
        }
        public static ItemCut Cut(CharacterItemRecord item,uint quantity,byte newItempos)
        {
            return new ItemCut(item, quantity,newItempos);
        }
    }
}
