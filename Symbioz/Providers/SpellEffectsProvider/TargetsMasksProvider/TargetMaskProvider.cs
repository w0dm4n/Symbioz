using Symbioz.Core.Startup;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.TargetsMasksProvider
{
    class TargetMaskProvider
    {
        private static Dictionary<string, TargetMaskParserDel> Handlers = new Dictionary<string, TargetMaskParserDel>();

        public delegate List<Fighter> TargetMaskParserDel(Fighter fighter, string value);

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            foreach (var method in typeof(TargetMaskProvider).GetMethods())
            {
                var attribute = method.GetCustomAttributes(typeof(TargetMask), false);
                if (attribute.Count() > 0)
                {
                    var atr = (TargetMask)attribute[0];
                    Handlers.Add(atr.Identifier, (TargetMaskParserDel)Delegate.CreateDelegate(typeof(TargetMaskParserDel), method));
                }
            }
        }
        public static List<Fighter> Handle(Fighter fighter, string identifier)
        {
            var handler = Handlers.FirstOrDefault(x => identifier.StartsWith(x.Key));
            if (handler.Value != null)
            {
                string value = identifier.Remove(0,handler.Key.Length);
                return handler.Value(fighter, value);
            }
            else
            {
                if (!TargetMaskValidator.HandlerExist(identifier)) 
                Logger.Log("Unknown TargetMask Identifier " + identifier);
                return new List<Fighter>();
            }
        }

       
        [TargetMask("A")]
        public static List<Fighter> AllEnemies(Fighter fighter, string value)
        {
            return fighter.GetOposedTeam().GetFighters();
        }

        [TargetMask("a")]
        public static List<Fighter> AllAllies(Fighter fighter, string value)
        {
            return fighter.Team.GetFighters();
        }

        [TargetMask("j")]
        public static List<Fighter> AllSummons(Fighter fighter, string value)
        {
            return fighter.Fight.GetAllSummons();
        }

        [TargetMask("M")]
        public static List<Fighter> Monsters(Fighter fighter, string value)
        {
            return AllEnemies(fighter, value).FindAll(x=>x is MonsterFighter);
        }

        [TargetMask("C")] 
        public static List<Fighter> Self(Fighter fighter, string value)
        {
            return new List<Fighter>() { fighter };
        }

        [TargetMask("c")]
        public static List<Fighter> OnlySelf(Fighter fighter,string value)
        {
            return new List<Fighter>() { fighter };
        }

        [TargetMask("g")]
        public static List<Fighter> Allies(Fighter fighter, string value)
        {
            var fighters = fighter.Team.GetFighters();
            fighters.Remove(fighter);
            return fighters;
        }

        [TargetMask("i")]
        public static List<Fighter> Invocations(Fighter fighter,string value)
        {
            return fighter.Fight.GetAllSummons();
        }
    }
}
