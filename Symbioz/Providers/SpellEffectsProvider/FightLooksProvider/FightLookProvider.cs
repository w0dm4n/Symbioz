using Symbioz.Core.Startup;
using Symbioz.Providers.SpellEffectsProvider.FightLooksProvider;
using Symbioz.World.Models;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider
{
    class FightLookProvider
    {
        public delegate ContextActorLook CustomLookHandlerDel(Fighter fighter);
        // key = spellId
        public static Dictionary<ushort, CustomLookHandlerDel> Handlers = new Dictionary<ushort, CustomLookHandlerDel>();

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            foreach (var method in typeof(FightLookProvider).GetMethods())
            {
                var attribute = method.GetCustomAttribute(typeof(FightLook)) as FightLook;
                if (attribute != null)
                {
                    Handlers.Add(attribute.SpellId, (CustomLookHandlerDel)method.CreateDelegate(typeof(CustomLookHandlerDel)));
                }
            }

        }
        public static ContextActorLook GetLook(Fighter fighter, ushort spellid)
        {
            var handler = Handlers.FirstOrDefault(x => x.Key == spellid);
            if (handler.Value != null)
                return handler.Value(fighter);
            else
                return null;
        }
        [FightLook(2872)]
        public static ContextActorLook ClassMaskLook(Fighter fighter)
        {
            var newLook = fighter.FighterLook.CloneContextActorLook();
            newLook.RemoveSkin(1450);
            newLook.RemoveSkin(1449);
            newLook.RemoveSkin(1443);
            newLook.RemoveSkin(1448);
            if (!newLook.IsRiding)
                newLook.SetBonesId(fighter.RealFighterLook.bonesId);
            return newLook;
        }
        [FightLook(686)]
        public static ContextActorLook DrunkLook(Fighter fighter)
        {
            var newLook = fighter.FighterLook.CloneContextActorLook();
            newLook.SetBonesId(44);
            return newLook;
        }
        [FightLook(701)]
        public static ContextActorLook ZatoïshwanLook(Fighter fighter)
        {
            var newLook = fighter.FighterLook.CloneContextActorLook();

            if (!newLook.IsRiding)
                newLook.SetBonesId(453);
            else
                newLook.SetBonesId(1202);

            if (!newLook.IsRiding)
                newLook.SetScale(80);
            else
                newLook.SetScale(60);
            return newLook;
        }
        [FightLook(2879)]
        public static ContextActorLook CowardLook(Fighter fighter)
        {
            var cfighter = fighter as CharacterFighter;
            if (cfighter == null)
                return null;

            var newLook = fighter.FighterLook.CloneContextActorLook();
            if (!newLook.IsRiding)
                newLook.SetBonesId(1576);
            if (cfighter.Client.Character.Record.Sex)
            {
                newLook.AddSkin(1450);
            }
            else
            {
                newLook.AddSkin(1449);
            }
            return newLook;
        }
        [FightLook(2880)]
        public static ContextActorLook PsycopathMaskLook(Fighter fighter)
        {
            var cfighter = fighter as CharacterFighter;
            if (cfighter == null)
                return null;

            var newLook = fighter.FighterLook.CloneContextActorLook();
            if (!newLook.IsRiding)
                newLook.SetBonesId(1575);

            if (cfighter.Client.Character.Record.Sex)
            {
                newLook.AddSkin(1448);
            }
            else
            {
                newLook.AddSkin(1443);
            }
            return newLook;
        }
    }
}
