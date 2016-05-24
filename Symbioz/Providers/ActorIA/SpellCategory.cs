using Symbioz.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Symbioz.Providers.ActorIA
{
    [YAXComment("Représente une catégorie de sorts")]
    public class SpellCategory
    {
        public static string FILES_DIRECTORY = Environment.CurrentDirectory + "/SpellCategories/";
        public static YAXSerializer Serializer = new YAXSerializer(typeof(SpellCategory));
    
        public SpellCategoryEnum Category { get; set; }
        public EffectsEnum[] Effects { get; set; }

        public SpellCategory(SpellCategoryEnum category,EffectsEnum[] effects)
        {
            this.Category = category;
            this.Effects = effects ;
        }
        public SpellCategory() { }
        public void Serialize()
        {
            File.WriteAllText(GetFileName(Category), Serializer.Serialize(this));
        }
        public void Deserialize(SpellCategoryEnum category)
        {
            var obj = FromCategory(category);
            this.Category = obj.Category;
            this.Effects = obj.Effects;
        }
        public static SpellCategory FromCategory(SpellCategoryEnum category)
        {
            return (SpellCategory)Serializer.Deserialize(File.ReadAllText(GetFileName(category)));
        }
        public static string GetFileName(SpellCategoryEnum category)
        {
            return FILES_DIRECTORY + category + ".xml";
        }
    }
}
