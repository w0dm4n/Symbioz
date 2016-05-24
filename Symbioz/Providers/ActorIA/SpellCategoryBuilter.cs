using Symbioz.Core.Startup;
using Symbioz.Enums;
using Symbioz.World.Records;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA
{
    /// <summary>
    /// Effects generated automaticly
    /// </summary>
    class SpellCategoryBuilter
    {

        static List<SpellCategory> Categories = new List<SpellCategory>();

        public const sbyte DEFAULT_SEARCH_GRADE = 1;

        [StartupInvoke("SpellBuilter",StartupInvokeType.Others)]
        public static void Sort()
        {
            foreach (SpellCategoryEnum category in Enum.GetValues(typeof(SpellCategoryEnum)))
            {
                if (File.Exists(SpellCategory.GetFileName(category)))
                    Categories.Add(SpellCategory.FromCategory(category));
            }
            foreach (var spell in SpellRecord.Spells)
            {
                var level = SpellLevelRecord.GetLevel(spell.Id, DEFAULT_SEARCH_GRADE);
                spell.Category = Get(level.Effects.ConvertAll<EffectsEnum>(x => x.BaseEffect.EffectType));
            }
        }
        static SpellCategoryEnum Get(List<EffectsEnum> effects)
        {
            foreach (var effect in effects)
            {
                var category = Categories.Find(x => x.Effects.Contains(effect));
                if (category != null)
                    return category.Category;
            }
            return SpellCategoryEnum.Undefined;

        }
    }
}
