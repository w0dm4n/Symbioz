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
    public class MPBuff : Buff
    {
        public EffectsEnum Effect { get; set; }
        public MPBuff(uint uid,short delta, short duration, int sourceid, short sourcespellid,EffectsEnum effect,int delay) : base(uid,delta, duration, sourceid, sourcespellid,delay)
        {
            this.Effect = effect;
        }
        public override void SetBuff()
        {
            Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)Effect,
                SourceId, new FightTemporaryBoostEffect(UID, Fighter.ContextualId,
                    Duration, 1, (ushort)SourceSpellId,(uint)Effect, 0, Delta)));
            Fighter.FighterStats.Stats.MovementPoints += Delta;
            Fighter.FighterStats.RealStats.MovementPoints += Delta;
        }

        public override void RemoveBuff()
        {
            Fighter.FighterStats.Stats.MovementPoints -= Delta;
            Fighter.FighterStats.RealStats.MovementPoints -= Delta;
        }
    }
}
