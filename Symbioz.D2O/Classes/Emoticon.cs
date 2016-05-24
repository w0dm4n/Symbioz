// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Emoticon : ID2OClass
    {
        [Cache]
        public static List<Emoticon> Emoticons = new List<Emoticon>();
        public Int32 id;
        public Int32 nameId;
        public Int32 shortcutId;
        public UInt32 order;
        public String defaultAnim;
        public Boolean persistancy;
        public Boolean eight_directions;
        public Boolean aura;
        public String[] anims;
        public UInt32 cooldown;
        public UInt32 duration;
        public UInt32 weight;
        public Emoticon(Int32 id, Int32 nameId, Int32 shortcutId, UInt32 order, String defaultAnim, Boolean persistancy, Boolean eight_directions, Boolean aura, String[] anims, UInt32 cooldown, UInt32 duration, UInt32 weight)
        {
            this.id = id;
            this.nameId = nameId;
            this.shortcutId = shortcutId;
            this.order = order;
            this.defaultAnim = defaultAnim;
            this.persistancy = persistancy;
            this.eight_directions = eight_directions;
            this.aura = aura;
            this.anims = anims;
            this.cooldown = cooldown;
            this.duration = duration;
            this.weight = weight;
        }
    }
}
