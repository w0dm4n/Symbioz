using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class StatBuff : Buff
    {
        /// <summary>
        /// Exemple Transe Zobal => Delta = 75 (% de vie en mois) RealDelta = valeur soustraite (75% des pvs)
        /// </summary>
        short RealDelta { get; set; }
        uint DisplayableId { get; set; }
        UInt16ReflectedStat StatDefiniton { get; set; }
        public StatBuff(uint uid, UInt16ReflectedStat statdefinition,uint displayableid, short delta, short duration, int sourceid, short sourcespellid, short realdelta,int delay)
            : base(uid, delta, duration, sourceid, sourcespellid,delay)
        {
            this.StatDefiniton = statdefinition;
            this.DisplayableId = displayableid;
            this.RealDelta = realdelta;
        }
        public override void SetBuff()
        {
            this.Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)DisplayableId, SourceId,
                new FightTemporaryBoostEffect(UID, Fighter.ContextualId, Duration, 1, (ushort)SourceSpellId,DisplayableId, 0, Delta)));
            this.StatDefiniton.AddValue(RealDelta);

        }

        public override void RemoveBuff()
        {
            this.StatDefiniton.AddValue((short)-RealDelta);

        }

    }
}
