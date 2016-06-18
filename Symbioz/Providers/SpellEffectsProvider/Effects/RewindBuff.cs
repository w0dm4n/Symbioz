﻿using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Effects
{
    /// <summary>
    /// Poutch Iop
    /// </summary>
    public class RewindBuff : Buff
    {
        public const int REWIND_PERCENTAGE = 20;

        public override FighterEventType EventType
        {
            get
            {
                return FighterEventType.AFTER_ATTACKED;
            }
        }
        public RewindBuff(uint uid, short delta, short duration, int sourceid, short sourcespellid, int delay)
            : base(uid, delta, duration, sourceid, sourcespellid, delay)
        {

        }

        public override void SetBuff()
        {
            Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)EffectsEnum.Eff_Rewind, SourceId, new FightTemporaryBoostEffect((uint)Fighter.BuffIdProvider.Pop(), Fighter.ContextualId, Duration, 1, (ushort)SourceSpellId, (uint)EffectsEnum.Eff_Rewind, 0, Delta)));
        }

        public override void RemoveBuff()
        {
           
        }
        //Poutch Event dommages en zones sans event pour Evité boucle infini
        public override bool OnEventCalled(object arg1, object arg2, object arg3)
        {
            if (arg1 == null || arg2 == null)
                return false;

            int sourceId = (int)arg1;
            TakenDamages damages = (TakenDamages)arg2;
            Fighter source = Fighter.Fight.GetFighter(SourceId);
            List<short> cellIds = ShapesProvider.GetCrossCells(Fighter.CellId, Fighter.CellId, 1);
            damages.Delta = damages.GetDeltaPercentage(REWIND_PERCENTAGE);

            if (source == null)
                return false;
            foreach (var cellId in cellIds)
            {
                Fighter target = Fighter.Fight.GetFighter(cellId);
                if (target != null)
                {
                    target.LoseLifeNoEvent(new TakenDamages(damages.Delta, ElementType.Neutral), source.ContextualId);
                }
            }
            return false;
        }
    }
}
