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

namespace Symbioz.Providers
{
    class SpellEffectsHandler
    {
        public delegate void SpellHandlerDelegate(Fighter source, SpellLevelRecord level, ExtendedSpellEffect effect,List<Fighter> affecteds, short castcellid);

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
        public static void Handle(Fighter fighter, SpellLevelRecord record, ExtendedSpellEffect effect,List<Fighter> affecteds, short castcellid)
        {
            var handler = Handlers.FirstOrDefault(x => x.Key == effect.BaseEffect.EffectType);
            if (handler.Value != null)
            {
                handler.Value(fighter, record, effect,affecteds, castcellid);
            }
            else
            {
                fighter.Fight.Reply(effect.BaseEffect.EffectType + " is not handled...");
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
