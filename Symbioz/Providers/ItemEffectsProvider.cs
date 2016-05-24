using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Provider
{
    class ItemEffectsProvider
    {
        private static Dictionary<EffectsEnum, Action<WorldClient, short>> Functions = new Dictionary<EffectsEnum, Action<WorldClient, short>>();
        [StartupInvoke(StartupInvokeType.Others)]
        public static void LoadFunctions()
        {
            Functions.Add(EffectsEnum.Eff_AddStrength, Strenght);
            Functions.Add(EffectsEnum.Eff_AddVitality, Vitality);
            Functions.Add(EffectsEnum.Eff_AddAgility, Agility);
            Functions.Add(EffectsEnum.Eff_AddIntelligence, Intelligence);
            Functions.Add(EffectsEnum.Eff_AddWisdom, Wisdom);
            Functions.Add(EffectsEnum.Eff_Followed, Followed);
            Functions.Add(EffectsEnum.Eff_AddChance, Chance);
            Functions.Add(EffectsEnum.Eff_AddAP_111, Ap);
            Functions.Add(EffectsEnum.Eff_AddMP_128, AddMp);
            Functions.Add(EffectsEnum.Eff_AddTitle, Title);
            Functions.Add(EffectsEnum.Eff_AddCriticalHit, CritHit);
            Functions.Add(EffectsEnum.Eff_AddSummonLimit, SummonLimit);
            Functions.Add(EffectsEnum.Eff_AddDamageBonus, DmgsBonus);
            Functions.Add(EffectsEnum.Eff_AddDamageBonusPercent, DmgsBonusPercent);
            Functions.Add(EffectsEnum.Eff_IncreaseDamage_138, DmgsBonusPercent);
            Functions.Add(EffectsEnum.Eff_AddProspecting, Prospecting);
            Functions.Add(EffectsEnum.Eff_AddAirElementReduction, AirReduction);
            Functions.Add(EffectsEnum.Eff_AddFireElementReduction, FireReduction);
            Functions.Add(EffectsEnum.Eff_AddEarthElementReduction, EarthReduction);
            Functions.Add(EffectsEnum.Eff_AddWaterElementReduction, WaterReduction);
            Functions.Add(EffectsEnum.Eff_AddEarthResistPercent, EarthResist);
            Functions.Add(EffectsEnum.Eff_AddWaterResistPercent, WaterResist);
            Functions.Add(EffectsEnum.Eff_AddAirResistPercent, AirResist);
            Functions.Add(EffectsEnum.Eff_AddFireResistPercent, FireResist);
            Functions.Add(EffectsEnum.Eff_AddInitiative, Initiative);
            Functions.Add(EffectsEnum.Eff_10, Emote);
            Functions.Add(EffectsEnum.Eff_SubMP, SubMp);
            Functions.Add(EffectsEnum.Eff_Companion,Companion);
            Functions.Add(EffectsEnum.Eff_AddWaterDamageBonus, WaterDamagesBonus);
            Functions.Add(EffectsEnum.Eff_AddAirDamageBonus, AirDamagesBonus);
            Functions.Add(EffectsEnum.Eff_AddEarthDamageBonus, EarthDamagesBonus);
            Functions.Add(EffectsEnum.Eff_AddFireDamageBonus, FireDamagesBonus);
            Functions.Add(EffectsEnum.Eff_AddNeutralDamageBonus, NeutralDamagesBonus);
            Functions.Add(EffectsEnum.Eff_AddNeutralResistPercent, NeutralResist);
            Functions.Add(EffectsEnum.Eff_SubAgility, SubAgility);
            Functions.Add(EffectsEnum.Eff_SubIntelligence, SubIntelligence);
            Functions.Add(EffectsEnum.Eff_SubVitality, SubVitality);
            Functions.Add(EffectsEnum.Eff_SubChance, SubChance);
            Functions.Add(EffectsEnum.Eff_SubStrength, SubStrength);
            Functions.Add(EffectsEnum.Eff_410, APReduction);
        }
        public static void RemoveEffects(WorldClient client, List<ObjectEffect> effects)
        {
            var effs = effects.FindAll(x => x is ObjectEffectInteger).ConvertAll<ObjectEffectInteger>(x => (ObjectEffectInteger)x);

            foreach (var effect in effs)
            {
                HandleEff(client, new ObjectEffectInteger(effect.actionId, (ushort)-effect.value));
            }

        }
        public static void AddEffects(WorldClient client, List<ObjectEffect> effect)
        {
            var effs = effect.FindAll(x => x is ObjectEffectInteger).ConvertAll<ObjectEffectInteger>(x => (ObjectEffectInteger)x);
            effs.ForEach(x => HandleEff(client, x));
        }
        static void HandleEff(WorldClient client, ObjectEffectInteger effect)
        {
            var function = Functions.ToList().Find(x => x.Key == (EffectsEnum)effect.actionId);
            if (function.Value != null)
            {
                function.Value(client, (short)effect.value);
            }
            else
            {
                client.Character.Reply((EffectsEnum)effect.actionId + " is not handled");
            }
        }
        static void NeutralDamagesBonus(WorldClient client, short value)
        {
            client.Character.StatsRecord.NeutralDamageBonus += value;
        }
        static void EarthDamagesBonus(WorldClient client, short value)
        {
            client.Character.StatsRecord.EarthDamageBonus += value;
        }
        static void AirDamagesBonus(WorldClient client, short value)
        {
            client.Character.StatsRecord.AirDamageBonus += value;
        }
        static void FireDamagesBonus(WorldClient client, short value)
        {
            client.Character.StatsRecord.FireDamageBonus += value;
        }
        static void WaterDamagesBonus(WorldClient client,short value)
        {
            client.Character.StatsRecord.WaterDamageBonus += value;
        }
        private static void Companion(WorldClient client,short value)
        {
            if (value > 0)
                client.Character.EquipCompanion(value);
            else
                client.Character.UnequipCompanion();
        }
        private static void Emote(WorldClient client,short value)
        {
            if (value > 0)
                client.Character.LearnEmote((byte)value);
            else
                client.Character.ForgetEmote((byte)-value);

        }
        private static void DmgsBonusPercent(WorldClient client,short value)
        {
            client.Character.StatsRecord.AllDamagesBonusPercent += value;
        }
        private static void Initiative(WorldClient client,short value)
        {
            client.Character.StatsRecord.Initiative += value;
        }
        private static void NeutralResist(WorldClient client, short value)
        {
            // 50 MAX
            client.Character.StatsRecord.NeutralResistPercent += value;
        }
        private static void FireResist(WorldClient client, short value)
        {
            client.Character.StatsRecord.FireResistPercent += value;
        }
        private static void AirResist(WorldClient client, short value)
        {
            client.Character.StatsRecord.AirResistPercent += value;
        }
        private static void WaterResist(WorldClient client, short value)
        {
            client.Character.StatsRecord.WaterResistPercent += value;
        }
        private static void EarthResist(WorldClient client,short value)
        {
            client.Character.StatsRecord.EarthResistPercent += value;
        }
        private static void FireReduction(WorldClient client, short value)
        {
            client.Character.StatsRecord.FireReduction += value;
        }
        private static void WaterReduction(WorldClient client, short value)
        {
            client.Character.StatsRecord.WaterReduction += value;
        }
        private static void EarthReduction(WorldClient client, short value)
        {
            client.Character.StatsRecord.EarthReduction += value;
        }
        private static void AirReduction(WorldClient client,short value)
        {
            client.Character.StatsRecord.AirReduction += value;
        }
        private static void Prospecting(WorldClient client,short value)
        {
            client.Character.StatsRecord.Prospecting += value;
        }
        private static void DmgsBonus(WorldClient client,short value)
        {
            client.Character.StatsRecord.AllDamagesBonus += value;
        }
        private static void SummonLimit(WorldClient client, short value)
        {
            client.Character.StatsRecord.SummonableCreaturesBoost += value;
        }
        private static void Title(WorldClient client, short value)
        {
            if (value > 0)
            {
                client.Character.SelectTitle((ushort)value);
            }
            else
            {
                client.Character.SelectTitle(0);
            }
        }
        private static void CritHit(WorldClient client, short value)
        {
            client.Character.StatsRecord.CriticalHit += value;
        }
        private static void SubMp(WorldClient client,short value)
        {
            client.Character.StatsRecord.MovementPoints -= value;
        }
        private static void AddMp(WorldClient client, short value)
        {
            client.Character.StatsRecord.MovementPoints += value;
        }
        private static void Ap(WorldClient client, short value)
        {
            client.Character.StatsRecord.ActionPoints  += value;
        }
        private static void Chance(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextChance += value;
        }
        private static void Followed(WorldClient client, short value)
        {
            //if (value > 0)
            //    client.Character.HumanOptions.Add(new HumanOptionFollowers(new List<IndexedEntityLook> { new IndexedEntityLook(EntityLookHelper.CreateBonesLook(460), 0) }));
            //else
            //{
            //    var option = client.Character.HumanOptions.Find(x => x.TypeId == 410);
            //    client.Character.HumanOptions.Remove(option);
            //}
            //client.Character.RefreshMapInstance();
        }

        private static void Wisdom(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextWisdom += value;
        }
        private static void Agility(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextAgility += value;
        }
        private static void Strenght(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextStrength += value;
        }
        private static void Vitality(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextVitality += value;
            client.Character.CurrentStats.LifePoints += (uint)value;
            client.Character.StatsRecord.LifePoints += value;
        }
        private static void Intelligence(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextIntelligence += value;
        }

        private static void SubIntelligence(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextIntelligence -= value;
        }
        private static void SubChance(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextChance -= value;
        }
        private static void SubVitality(WorldClient client,short value)
        {
            client.Character.StatsRecord.ContextVitality -= value;
            client.Character.CurrentStats.LifePoints -= (uint)value;
            client.Character.StatsRecord.LifePoints -= value;
        }
        private static void SubAgility(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextAgility -= value;
        }
        private static void SubStrength(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextStrength -= value;
        }
        private static void APReduction(WorldClient client, short value)
        {
            client.Character.StatsRecord.ContextAPReduction += value;
        }
    }
}
