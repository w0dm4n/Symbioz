using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class PunishementBuff : Buff
    {
        public override FighterEventType EventType
        {
            get
            {
                return FighterEventType.AFTER_ATTACKED;
            }
        }

        public short PunishementAction { get; set; }
        public short PunishementDelay { get; set; }
        public UInt16ReflectedStat StatDefinition { get; set; }

        public PunishementBuff(uint uid, short delta, short duration, int sourceid, short sourcespellid, short punishementaction,short punishementdelay,int delay)
            : base(uid, delta, duration, sourceid, sourcespellid,delay)
        {
            this.PunishementAction = punishementaction;
            this.PunishementDelay = punishementdelay;
        }

        public override void SetBuff()
        {
            StatDefinition = GetStatData();
            Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)EffectsEnum.Eff_Punishment,
              SourceId, new FightTriggeredEffect(UID, Fighter.ContextualId, Duration, 0, (ushort)SourceSpellId, 0, 0, 0, Delta, Duration, 0)));
        }

        public override void RemoveBuff()
        {

        }
        /// <summary>
        /// Called when the player takes damages
        /// </summary>
        /// <param name="obj">damages taken</param>
        public override bool OnEventCalled(object arg1,object arg2,object arg3) 
        {
            TakenDamages damages = (TakenDamages)arg2;
            if (Fighter.ContextualId == (int)arg1 || damages.Delta <= 0)
                return false;
            short statBuffDelta = (short)damages.Delta;
            int num = Fighter.GetAllBuffs<StatBuff>(x=>x.SourceSpellId == SourceSpellId).FindAll(x=>x.Duration == PunishementDelay).Sum(x=>x.Delta);
            if (num < Delta)
            {
                if (statBuffDelta + num > (int)Delta)
                    statBuffDelta = (short)(Delta - num);

                StatBuff buff = new StatBuff((uint)Fighter.BuffIdProvider.Pop(), StatDefinition, (uint)GetBuffEffectType(StatDefinition.FieldName), statBuffDelta, PunishementDelay, Fighter.ContextualId, SourceSpellId, statBuffDelta,0);
                Fighter.AddBuff(buff);
            }
            return false;

        }
        public UInt16ReflectedStat GetStatData()
        {
            switch (PunishementAction)
            {
                case 118:
                    return new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextStrength"), Fighter.FighterStats.Stats);
                case 119:
                    return new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextAgility"), Fighter.FighterStats.Stats);
                case 123:
                    return new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextChance"), Fighter.FighterStats.Stats);
                case 124:
                    return new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextWisdom"), Fighter.FighterStats.Stats);
                case 126:
                    return new UInt16ReflectedStat(StatsRecord.GetFieldInfo("ContextIntelligence"), Fighter.FighterStats.Stats);
                case 407:
                    return new UInt16ReflectedStat(StatsRecord.GetFieldInfo("LifePoints"), Fighter.FighterStats.Stats);
            }
            return null;
        }
        private static EffectsEnum GetBuffEffectType(string fieldtype)
        {
            EffectsEnum result = EffectsEnum.End;
            switch (fieldtype)
            {
                case "ContextStrength":
                    result = EffectsEnum.Eff_AddStrength;
                    break;
                case "LifePoints":
                    result = EffectsEnum.Eff_AddVitality;
                    break;
                case "ContextWisdom":
                    result = EffectsEnum.Eff_AddWisdom;
                    break;
                case "ContextChance":
                    result = EffectsEnum.Eff_AddChance;
                    break;
                case "ContextAgility":
                    result = EffectsEnum.Eff_AddAgility;
                    break;
                case "ContextIntelligence":
                    result = EffectsEnum.Eff_AddIntelligence;
                    break;
               
            }
            return result;
        }
    }
}
