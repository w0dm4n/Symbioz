using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class ShieldBuff : Buff
    {
        public ShieldBuff(uint uid,short delta, short duration,int sourceid,short sourcespellid,int delay):base(uid,delta,duration,sourceid,sourcespellid,delay)
        {
         
        }
        public override void SetBuff()
        {
            Fighter.AddShieldPoints(UID,Delta, Duration, SourceId,(ushort)SourceSpellId);
        }

        public override void RemoveBuff()
        {
            Fighter.FighterStats.ShieldPoints -= (int)Delta;
            if (Fighter.FighterStats.ShieldPoints < 0)
                Fighter.FighterStats.ShieldPoints = 0;
        }
    }
}
