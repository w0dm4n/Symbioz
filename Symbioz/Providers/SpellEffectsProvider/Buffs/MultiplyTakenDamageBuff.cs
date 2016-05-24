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
    /// <summary>
    /// Chance d'écaflip, dommages subis *X%
    /// </summary>
    public class MultiplyTakenDamageBuff : Buff
    {
        public override FighterEventType EventType
        {
            get
            {
                return FighterEventType.BEFORE_ATTACKED;
            }
        }
        public MultiplyTakenDamageBuff(uint UID, short delta, short duration, int sourceid, short sourcespellid, int delay)
            : base(UID, delta, duration, sourceid, sourcespellid, delay)
        {

        }
        public override void SetBuff()
        {
            Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)EffectsEnum.Eff_MultiplyTakenDamages, SourceId,
                new FightTemporaryBoostEffect(UID, Fighter.ContextualId,
                  Duration, 1, (ushort)SourceSpellId, (uint)EffectsEnum.Eff_MultiplyTakenDamages, 0, Delta)));
        }

        public override void RemoveBuff()
        {

        }
        public override bool OnEventCalled(object arg1, object arg2, object arg3)
        {
            TakenDamages damages = (TakenDamages)arg2;
            damages.Delta *= (short)(Delta / 100);
            Fighter.LoseLife(damages,(int)arg1);
            return true;
        }
    }
}