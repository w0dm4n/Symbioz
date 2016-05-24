using Symbioz.Core.Startup;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.TargetsMasksProvider
{
    public class TargetMaskValidator
    {
        private static Dictionary<string, ValidatorParserDel> Handlers = new Dictionary<string, ValidatorParserDel>();

        public delegate List<Fighter> ValidatorParserDel(Fighter fighter, string value,List<Fighter> allfighters);

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
             foreach (var method in typeof(TargetMaskValidator).GetMethods())
            {
                var attribute = method.GetCustomAttributes(typeof(TMValidator), false);
                if (attribute.Count() > 0)
                {
                    var atr = (TMValidator)attribute[0];
                    Handlers.Add(atr.Identifier, (ValidatorParserDel)Delegate.CreateDelegate(typeof(ValidatorParserDel), method));
                }
            }
        }
        public static bool HandlerExist(string identifier)
        {
            var handler = Handlers.FirstOrDefault(x => identifier.StartsWith(x.Key));
            if (handler.Value != null)
                return true;
            else
                return false;
        }
        public static List<Fighter> Valid(Fighter fighter, string identifier, List<Fighter> allfighters)
        {
            var handler = Handlers.FirstOrDefault(x => identifier.StartsWith(x.Key));
            if (handler.Value != null)
            {
                string value = identifier.Remove(0, handler.Key.Length);
                return handler.Value(fighter, value,allfighters);
            }
            else
            {
                return allfighters;
            }
        }
        [TMValidator("*E")]
        public static List<Fighter> WithState(Fighter fighter, string value, List<Fighter> exisiting)
        {
            if (fighter.HaveState(short.Parse(value)))
                return exisiting;
            else
                return new List<Fighter>();
        }
        [TMValidator("*e")]
        public static List<Fighter> WithoutState(Fighter fighter, string value, List<Fighter> exisiting)
        {
            if (!fighter.HaveState(short.Parse(value)))
                return exisiting;
            else
                return new List<Fighter>();
        }
        [TMValidator("P")]
        public static List<Fighter> SelectMonsters(Fighter fighter, string value, List<Fighter> exisiting)
        {
            return exisiting.FindAll(x => x is BombFighter);
        }
        [TMValidator("F")]
        public static List<Fighter> SelectedMonster(Fighter fighter, string value,List<Fighter> existing)
        {
            List<Fighter> results = new List<Fighter>();
            foreach (var target in existing)
            {
                short tempateId = 0;
                if (short.TryParse(value, out tempateId))
                {
                    var mFigther = target as MonsterFighter;
                    if (mFigther != null && mFigther.Template.Id == tempateId)
                        results.Add(mFigther);
                }
            }
            return results;
        }
    }
}
