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
                    return "P1";
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

        public short[] GetDamagedCell(int currentCell, WeaponRecord template, MapRecord CurrentMap, short CasterCell)
        {
            short[] allCell = new short[4];

            int x = 14;
            bool quatorze = true;
            byte direction = 0;

            int line = 13;
            while (x < (currentCell + 1))
            {
                if (quatorze == false)
                {
                    quatorze = true;
                    x += 13;
                }
                else
                {
                    quatorze = false;
                    x += 14;
                }
            }
            if (quatorze)
                line = 14;
            if (CasterCell + 15 == currentCell)//bas droite (line == 13)
            {
                Logger.Log("bas droite");
            }
            else if (CasterCell - 15 == currentCell)//haut gauche (line == 13)
            {
                Logger.Log("hautgauche");
            }
            else if (CasterCell + 13 == currentCell && line == 13)//bas
            {
                //bas gauche
                direction = 3;
            }
            else if (CasterCell + 13 == currentCell && line == 14)//bas
            {
                Logger.Log("bas13/14");
            }
            else if (CasterCell + 14 == currentCell && line == 13)//bas
            {
                //bas gauche
                direction = 3;
            }
            else if (CasterCell + 14 == currentCell && line == 14)//bas
            {
               // bas droite
                direction = 1;
            }
            else if (CasterCell - 13 == currentCell && line == 13)//haut
            {
                //haut droite
                direction = 7;
            }
            else if (CasterCell - 13 == currentCell && line == 14)//haut
            {
                Logger.Log("haut13/14");
            }
            else if (CasterCell - 14 == currentCell && line == 13)//haut
            {
                //haut gauche
                direction = 5;
            }
            else if (CasterCell - 14 == currentCell && line == 14)//haut
            {
                Logger.Log("haut14/14");
            }
            else
                Logger.Log("INCONNUE");
            allCell[0] = (short)currentCell;
            Logger.Log("DIRECTION = " + direction);
            switch (direction)
            {
                case 1://bas droit

                    break;

                case 3: // bas gauche
                    if (line == 13)
                        Logger.Log("13");
                    else if (line == 14)
                        Logger.Log("14");
                    //allCell[1] = (short)(currentCell + line);//bas gauche
                    allCell[1] = (short)(CasterCell - 1);//haut gauche
                    allCell[2] = (short)((currentCell + line) + 1);//bas gauche
                    allCell[3] = (short)((currentCell + line) + 2);//bas gauche
                    break;
                case 5://haut gauche

                    break;

                case 7://haut drote

                    break;
                
            }
            return (allCell);
        }
    }
}
