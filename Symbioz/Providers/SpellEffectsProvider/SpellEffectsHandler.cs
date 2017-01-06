using Symbioz.Core.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbioz;
using System.Threading.Tasks;
using System.Reflection;
using Symbioz.Providers.SpellEffectsProvider;
using Symbioz.Enums;
using Symbioz.World.Records.Spells;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.Helper;
using Symbioz.Network.Servers;

namespace Symbioz.Providers
{
    class SpellEffectsHandler
    {
        //interface des fonctions pointé vers les EffectsEnums
        public delegate void SpellHandlerDelegate(Fighter source, SpellLevelRecord level, ExtendedSpellEffect effect,List<Fighter> affecteds, short castcellid);

        //Dictionary des Effects a activé l'ors d'un ajout d'une interface avec toutes les informations
        public static Dictionary<EffectsEnum, SpellHandlerDelegate> Handlers = new Dictionary<EffectsEnum, SpellHandlerDelegate>();

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Intialize()
        {
            var assembly = Assembly.GetAssembly(typeof(SpellEffectsHandler));
            foreach (var type in assembly.GetTypes())
            {
                var methods = type.GetMethods().ToList().FindAll(x => x.GetCustomAttribute(typeof(EffectHandler)) != null);
                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttribute(typeof(EffectHandler)) as EffectHandler;
                    Handlers.Add(attribute.Effect, (SpellHandlerDelegate)method.CreateDelegate(typeof(SpellHandlerDelegate)));
                }
            }
        }

        public static int[] GetActorsShieldPoints(List<Fighter> actors, int[] array)
        {
            int i = 0;
            foreach (var actor in actors)
                array[i++] = actor.FighterStats.ShieldPoints;
            return (array);
        }

        public static void Handle(Fighter fighter, SpellLevelRecord record, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            //Ajout des Effets dans le dictionnaire
            try
            {
                var handler = Handlers.FirstOrDefault(x => x.Key == effect.BaseEffect.EffectType);

                Logger.Error("EFFECT " + effect.BaseEffect.EffectType);
                bool exist = false;

                foreach (EffectsEnum e in Handlers.Keys)
                {
                    if (e == effect.BaseEffect.EffectType)
                    {
                        Logger.Error("POINTER OF THIS EFFECT " + e + " EXIST");
                        exist = true;
                    }
                }
                if (!exist)
                    Logger.Error("POINTER OF THIS EFFECT " + effect.BaseEffect.EffectType + " DOESN'T EXIST");

                if (handler.Value != null)
                {
                    int[] actorsShieldPointsBeforeApplyEffect = new int[affecteds.Count()];
                    actorsShieldPointsBeforeApplyEffect = GetActorsShieldPoints(affecteds, actorsShieldPointsBeforeApplyEffect);

                    //Active les Atribut des Effets qui lance c'est information dans la fonction ataché a l'attribut
                    handler.Value(fighter, record, effect, affecteds, castcellid);

                    if (fighter.Fight.ChallengesInstance != null)
                        fighter.Fight.ChallengesInstance.HandleSpellCast(fighter, affecteds, effect, actorsShieldPointsBeforeApplyEffect);
                }
                else
                {
                    if (WorldServer.Instance.GetOnlineClient(fighter.Fight.Id) != null &&
                        WorldServer.Instance.GetOnlineClient(fighter.Fight.Id).Character.isDebugging)
                        fighter.Fight.Reply(effect.BaseEffect.EffectType + " is not handled...");
                }
            } catch (Exception e)
            {
                Logger.Error(e);
            }
        }
        public static short GetRandom(ExtendedSpellEffect record)
        {
            short jet;
            if (record.BaseEffect.DiceSide == 0)
                jet = (short)record.BaseEffect.DiceNum;
            else
                jet = (short)new AsyncRandom().Next(record.BaseEffect.DiceNum, record.BaseEffect.DiceSide + 1);
            return jet;
        }

    }
}
