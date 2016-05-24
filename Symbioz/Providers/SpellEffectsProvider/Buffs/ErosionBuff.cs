using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class ErosionBuff : Buff
    {
        public ErosionBuff(uint uid, short delta, short duration, int sourceid, short sourcespellid, int delay)
            : base(uid, delta, duration, sourceid, sourcespellid, delay)
        {

        }
        public override void SetBuff()
        {
            Fighter.Fight.Send(DefaultMessage(EffectsEnum.Eff_AddErosion));
            Fighter.FighterStats.ErosionPercentage += Delta;
        }

        public override void RemoveBuff()
        {
            Fighter.FighterStats.ErosionPercentage -= Delta;
        }
    }
}
