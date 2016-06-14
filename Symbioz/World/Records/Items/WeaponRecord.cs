using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.ORM;
using Symbioz.Helper;
using Symbioz.World.Models;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Items
{
    [Table("Weapons")]
    public class WeaponRecord : ITable
    {
        public static EffectsEnum[] WeaponsDamagesEffects = new EffectsEnum[] { EffectsEnum.Eff_DamageNeutral, EffectsEnum.Eff_DamageFire, EffectsEnum.Eff_DamageEarth, EffectsEnum.Eff_DamageWater, EffectsEnum.Eff_DamageAir };

        public const string DefaultWeaponEffectsTargetMask = "a,A";

        public static List<WeaponRecord> Weapons = new List<WeaponRecord>();

        public ushort Id;
        public string Name;
        public short MinRange;
        public short MaxRange;
        public sbyte CriticalHitBonus;
        public sbyte MaxCastPerTurn;
        public int DescriptionId;
        public short Level;
        public short RealWeight;
        public string Criteria;
        public sbyte CriticalHitProbability;
        public bool TwoHanded;
        public int Price;
        public sbyte ApCost;
        public bool CastInLine;
        public string Effects;
        [Ignore]
        public ItemEffectsParser RealEffects;
        public short TypeId;
        [Ignore]
        public ItemTypeEnum Type { get { return (ItemTypeEnum)TypeId; } }
        public short AppearenceId;

        public WeaponRecord(ushort id, string name, short minrange, short maxrange, sbyte criticalhitbonus, sbyte maxcastperturn,
            int descriptionid, short level, short realweight, string criteria, sbyte criticalhitprobability, bool twohanded,
            int price, sbyte apcost, bool castinline, string effects, short typeid,short apperanceid)
        {
            this.Id = id;
            this.Name = name;
            this.MinRange = minrange;
            this.MaxRange = maxrange;
            this.CriticalHitBonus = criticalhitbonus;
            this.MaxCastPerTurn = maxcastperturn;
            this.DescriptionId = descriptionid;
            this.Level = level;
            this.RealWeight = realweight;
            this.Criteria = criteria;
            this.CriticalHitProbability = criticalhitprobability;
            this.TwoHanded = twohanded;
            this.Price = price;
            this.ApCost = apcost;
            this.CastInLine = castinline;
            this.Effects = effects;
            this.TypeId = typeid;
            this.AppearenceId = apperanceid;
            this.RealEffects = new ItemEffectsParser(Effects);
            ItemRecord.Items.Add(ToItemRecord());
        }
        public ItemRecord ToItemRecord()
        {
            return new ItemRecord(Id, 0, Name, TypeId,AppearenceId, Level, Price, RealWeight, Effects, Criteria);
        }
        public List<ExtendedSpellEffect> GetWeaponEffects(FightSpellCastCriticalEnum critical)
        {
            List<ExtendedSpellEffect> results = new List<ExtendedSpellEffect>();
            foreach (var effect in RealEffects.ObjectEffects)
            {
                if (WeaponsDamagesEffects.Contains((EffectsEnum)effect.actionId))
                    results.Add(TransformEffect(this, effect, Type, critical));
            }
            return results;
        }

        public ObjectItem GenerateRandomObjectItem()
        {
            return new ObjectItem(63, (ushort)Id, GenerateRandomEffect(), CharacterItemRecord.PopNextUID(), 1);
        }

        public List<ObjectEffect> GenerateRandomEffect()
        {
            List<ObjectEffect> result = new List<ObjectEffect>();
            foreach (var eff in RealEffects.ObjectEffects)
            {
                if (WeaponRecord.WeaponsDamagesEffects.Contains((EffectsEnum)eff.actionId))
                {
                    result.Add(new ObjectEffectDice(eff.actionId, eff.diceNum, eff.diceSide, eff.diceConst));
                    continue;
                }
                if (eff.diceSide == 0 && eff.diceConst == 0)
                {
                    result.Add(new ObjectEffectInteger(eff.actionId, eff.diceNum));
                }
                else if (eff.diceConst != 0)
                {
                    result.Add(new ObjectEffectInteger(eff.actionId, eff.diceConst));

                }
                else
                {
                    if (eff.diceNum > eff.diceSide)
                    {
                        result.Add(new ObjectEffectInteger(eff.actionId, (ushort)eff.diceNum));
                    }
                    else
                    {
                        int value = (short)new AsyncRandom().Next(eff.diceNum, eff.diceSide + 1);
                        result.Add(new ObjectEffectInteger(eff.actionId, (ushort)value));
                    }
                }
            }
            return result;
        }

        public static WeaponRecord GetWeapon(ushort id)
        {
            return Weapons.Find(x => x.Id == id);
        }
        public static string GetWeaponRawZone(ItemTypeEnum type)
        {
            switch (type)
            {

                case ItemTypeEnum.BOW:
                    return "P1";
                case ItemTypeEnum.WAND:
                    return "P2";
                case ItemTypeEnum.STAFF:
                    return "P1";
                case ItemTypeEnum.DAGGER:
                    return "P1";
                case ItemTypeEnum.SWORD:
                    return "P1";
                case ItemTypeEnum.HAMMER:
                    return "P1";
                case ItemTypeEnum.SHOVEL:
                    return "P1";// pell
                case ItemTypeEnum.AXE:
                    return "P1";
                case ItemTypeEnum.TOOL:
                    return "P1";
                case ItemTypeEnum.PICKAXE:
                    return "P1";
                default:
                    return "P1";
            }
        }
        public static ExtendedSpellEffect TransformEffect(WeaponRecord record, ObjectEffectDice effect, ItemTypeEnum type, FightSpellCastCriticalEnum critical)
        {
            short dicenum = (short)(critical == FightSpellCastCriticalEnum.CRITICAL_HIT ? effect.diceNum + record.CriticalHitBonus : effect.diceNum);
            short diceside = (short)(critical == FightSpellCastCriticalEnum.CRITICAL_HIT ? effect.diceSide + record.CriticalHitBonus : effect.diceSide);
            return new ExtendedSpellEffect(new SpellEffectRecord(0, 0, (short)effect.actionId, GetWeaponRawZone(type), DefaultWeaponEffectsTargetMask, dicenum, diceside, 0, effect.diceConst, 0, 0));
        }
    }
}
