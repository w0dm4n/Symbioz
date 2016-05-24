using Symbioz.Core.Startup;
using Symbioz.Enums;
using Symbioz.Providers.SpellEffectsProvider.CustomSpellsEffectsProvider;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.CustomSpellsEffectsProvider
{
    public class CustomSpellEffectsProvider
    {
        public static Dictionary<SpellIdEnum, ISpellHandler> Handlers = new Dictionary<SpellIdEnum, ISpellHandler>();

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            foreach (var type in Assembly.GetAssembly(typeof(CustomSpellEffectsProvider)).GetTypesWithAttribute(typeof(CustomSpell)))
            {
                CustomSpell attribute = type.GetCustomAttribute<CustomSpell>();
                Handlers.Add(attribute.SpellId, (ISpellHandler)Activator.CreateInstance(type));
            }
        }
        public static bool Exist(ushort spellid)
        {
            if (Handlers.ContainsKey((SpellIdEnum)spellid))
                return true;
            else
                return false;
        }
        public static void Handle(ushort spellid,List<ExtendedSpellEffect> effects,Fighter fighter)
        {
            var handler = Handlers.FirstOrDefault(x => x.Key == (SpellIdEnum)spellid);
            handler.Value.Cast(fighter,effects);
        }
    }
}
