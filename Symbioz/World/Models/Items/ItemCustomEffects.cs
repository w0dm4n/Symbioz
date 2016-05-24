using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Items
{
    class ItemCustomEffects : Singleton<ItemCustomEffects>
    {
        public void Init(CharacterItemRecord item)
        {
            switch (item.GetTemplate().Type)
            {
                case ItemTypeEnum.LIVING_OBJECTS:
                    break;
                case ItemTypeEnum.MOUNT_CERTIFICATE:
                    break;
                case ItemTypeEnum.SOUL_STONE:
                    InitializeSoulStone(item);
                    break;
            }
        }

        private void InitializeSoulStone(CharacterItemRecord item)
        {
            List<ObjectEffect> effects = new List<ObjectEffect>();
            effects.Add(new ObjectEffectLadder(232, 1, 2));
  
            item.RemoveAllEffects();
            item.AddEffects(effects);
        }
    }
}
