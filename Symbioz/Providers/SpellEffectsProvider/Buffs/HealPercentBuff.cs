using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class HealPercentBuff : Buff
    {
        public override FighterEventType EventType
        {
            get
            {
                return FighterEventType.ON_TURN_STARTED;
            }
        }
        public HealPercentBuff(uint uid, short delta, short duration, int sourceid, short sourcespellid, int delay)
            : base(uid, delta, duration, sourceid, sourcespellid, delay)
        {

        }
        public override void SetBuff()
        {
            Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)EffectsEnum.Eff_RestoreHPPercent, SourceId, new FightTemporaryBoostEffect((uint)Fighter.BuffIdProvider.Pop(), Fighter.ContextualId, Duration, 1, (ushort)SourceSpellId, (uint)EffectsEnum.Eff_RestoreHPPercent, 0, Delta)));
        }

        public override void RemoveBuff()
        {
            
        }
        public override bool OnEventCalled(object arg1, object arg2, object arg3)
        {
            short num = (short)((double)Fighter.FighterStats.RealStats.LifePoints * ((double)Delta / 100.0));
            Fighter.Heal(num, SourceId);
            return base.OnEventCalled(arg1, arg2, arg3);
        }
    }
}
