using Symbioz.Core;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Messages;
using Symbioz.World.Records.Spells;
using Symbioz.Providers.Conditions;

namespace Symbioz.Providers
{
    class ItemUseEffectsProvider
    {
        private delegate bool HandlerDelegate(WorldClient client, ObjectEffect effect,uint objectuid);
        private static Dictionary<EffectsEnum, HandlerDelegate> Functions = new Dictionary<EffectsEnum,HandlerDelegate>();
        [StartupInvoke(StartupInvokeType.Others)]
        public static void LoadFunctions()
        {
            Functions.Add(EffectsEnum.Eff_AddHealth, AddHealth);
            Functions.Add(EffectsEnum.Eff_999, Teleport);
            Functions.Add(EffectsEnum.Eff_Teleport_SavePoint, SavePoint);
            Functions.Add(EffectsEnum.Eff_10, Emote);
            Functions.Add(EffectsEnum.Eff_620, Book);
            Functions.Add(EffectsEnum.Eff_AddRessources, AddRessources);
            Functions.Add(EffectsEnum.Eff_LearnSpell, LearnSpell);
            Functions.Add(EffectsEnum.Eff_AddPermanentStrength, AddStrength);
            Functions.Add(EffectsEnum.Eff_AddPermanentAgility ,AddAgility);
            Functions.Add(EffectsEnum.Eff_AddPermanentChance, AddChance);
            Functions.Add(EffectsEnum.Eff_AddPermanentIntelligence, AddIntelligence);
            Functions.Add(EffectsEnum.Eff_AddPermanentWisdom, AddWisdom);
            Functions.Add(EffectsEnum.Eff_AddPermanentVitality, AddVitality);
            Functions.Add(EffectsEnum.Eff_AddSpellPoints, AddSpellPoints);
            Functions.Add(EffectsEnum.Eff_GiveKamas, GiveKamas);

        }
        public static bool HandleEffects(WorldClient client, CharacterItemRecord item)
        {
            
            bool remove = false;
            if (!ConditionProvider.ParseAndEvaluate(client,item.GetTemplate().Criteria))
            {
                client.Character.Reply("Vous ne possédez pas les critères nécessaires pour utiliser cet objet.");
                return remove;
            }
            var effects = item.GetEffects();
            foreach (var effect in effects)
            {
                var function = Functions.ToList().Find(x => x.Key == (EffectsEnum)effect.actionId);
                if (function.Value != null)
                {
                    try
                    {
                        if (function.Value(client, effect, item.UID))
                            remove = true;
                    }
                    catch (Exception ex)
                    {
                        client.Character.NotificationError(ex.Message);
                    }
                }
                else
                {
                    client.Character.NotificationError((EffectsEnum)effect.actionId + " is not handled");
                }
            }
            return remove;
        }
        static bool AddVitality(WorldClient client, ObjectEffect effect, uint id)
        {
            short value = (short)(effect as ObjectEffectInteger).value;
            client.Character.StatsRecord.PermanentVitality += value;
            client.Character.Reply("Vous avez obtenu " + value + " en vitalité.");
            client.Character.RefreshStats();
            return true;
        }
        static bool AddWisdom(WorldClient client, ObjectEffect effect, uint id)
        {
            short value = (short)(effect as ObjectEffectInteger).value;
            client.Character.StatsRecord.PermanentWisdom += value;
            client.Character.Reply("Vous avez obtenu " + value + " en sagesse.");
            client.Character.RefreshStats();
            return true;
        }
        static bool GiveKamas(WorldClient client, ObjectEffect effect, uint id)
        {
            short value = (short)(effect as ObjectEffectInteger).value;
            if (value > 0)
                client.Character.AddKamas(value,true);
            else
                client.Character.RemoveKamas(-value,true);
            return true;
        }
        static bool AddIntelligence(WorldClient client, ObjectEffect effect, uint id)
        {
            short value = (short)(effect as ObjectEffectInteger).value;
            client.Character.StatsRecord.PermanentIntelligence += value;
            client.Character.Reply("Vous avez obtenu " + value + " en intelligence.");
            client.Character.RefreshStats();
            return true;
        }
        static bool AddChance(WorldClient client, ObjectEffect effect, uint id)
        {
            short value = (short)(effect as ObjectEffectInteger).value;
            client.Character.StatsRecord.PermanentChance += value;
            client.Character.Reply("Vous avez obtenu "+value+" en chance.");
            client.Character.RefreshStats();
            return true;
        }
        static bool AddAgility(WorldClient client, ObjectEffect effect, uint id)
        {
            short value = (short)(effect as ObjectEffectInteger).value;
            client.Character.StatsRecord.PermanentAgility += value;
            client.Character.Reply("Vous avez obtenu "+value+" en agilité.");
            client.Character.RefreshStats();
            return true;
        }
        static bool AddStrength(WorldClient client,ObjectEffect effect,uint id)
        {
            short value = (short)(effect as ObjectEffectInteger).value;
            client.Character.StatsRecord.PermanentStrenght += value;
            client.Character.Reply("Vous avez obtenu "+value+" en force.");
            client.Character.RefreshStats();
            return true;
        }
        static bool AddSpellPoints(WorldClient client,ObjectEffect effect,uint id)
        {
            client.Character.Record.SpellPoints += (effect as ObjectEffectInteger).value;
            client.Character.RefreshStats();
            return true;
        }
        static bool LearnSpell(WorldClient client,ObjectEffect effect,uint uid)
        {
            var level = SpellLevelRecord.GetLevel((int)(effect as ObjectEffectInteger).value);
            return client.Character.LearnSpell(level.SpellId);
        }
        static bool AddRessources(WorldClient client, ObjectEffect effect, uint uid)
        {
            client.Character.Inventory.Add((effect as ObjectEffectInteger).value, 10);
            return true;
        }
        static bool Emote(WorldClient client, ObjectEffect effect, uint uid)
        {
            return client.Character.LearnEmote((byte)(effect as ObjectEffectInteger).value);
        }
        static bool Book(WorldClient client, ObjectEffect effect, uint uid)
        {
            ObjectEffectInteger integer = effect as ObjectEffectInteger;
            client.Send(new DocumentReadingBeginMessage(integer.value));
            return false;
        }
        static bool AddHealth(WorldClient client, ObjectEffect effect, uint uid)
        {
            return false;
        }
        static bool Teleport(WorldClient client, ObjectEffect effect, uint uid)
        {
            ObjectEffectInteger integer = effect as ObjectEffectInteger;
            switch (integer.value)
            {
                case 1480:
                    client.Character.Teleport(144419, 231);
                    break;
                case 1436:
                    client.Character.Teleport(147768, 286);
                    break;
            }
            return true;
        }
        static bool SavePoint(WorldClient client, ObjectEffect effect, uint uid)
        {
            client.Character.TeleportToSpawnPoint();
            return true;
        }
    }
}
