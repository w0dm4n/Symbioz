using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers
{
    public class ItemEditor  
    {
        public static CharacterItemRecord AddEffectsAndClone(CharacterItemRecord baseItem,List<ObjectEffect> addedeffects,uint newQuantity)
        {
            var newItem = baseItem.CloneAndGetNewUID();
            newItem.AddEffects(addedeffects);
            newItem.Quantity = newQuantity;
            return newItem;
        }
    }
}
