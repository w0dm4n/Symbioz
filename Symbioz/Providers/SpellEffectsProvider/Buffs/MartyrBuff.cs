using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class MartyrBuff  :Buff
    {
        public override FighterEventType EventType
        {
            get
            {
                return FighterEventType.BEFORE_ATTACKED;
            }
        }
        public MartyrBuff(uint uid,short delta,short duration,int sourceid,short sourcespellid,int delay):base(uid,delta,duration,sourceid,sourcespellid,delay)
        {

        }
        public override void SetBuff()
        {
            Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)EffectsEnum.Eff_765, SourceId, new FightTemporaryBoostEffect((uint)Fighter.BuffIdProvider.Pop(), Fighter.ContextualId, Duration, 1, (ushort)SourceSpellId, (uint)EffectsEnum.Eff_765, 0, Delta)));
        }

        public override void RemoveBuff()
        {
         
        }
        public override bool OnEventCalled(object arg1,object arg2,object arg3)
        {
            int casterId = (int)arg1;
            TakenDamages damages = (TakenDamages)arg2;
            var martyr = Fighter.Fight.GetFighter(SourceId);
            martyr.TakeDamages(damages, casterId);
            return true;
        }
    }
}
