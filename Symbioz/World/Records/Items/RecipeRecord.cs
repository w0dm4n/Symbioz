using Symbioz.Core;
using Symbioz.ORM;
using Symbioz.World.Models;
using Symbioz.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Symbioz.World.Records
{
    [Table("Recipes",true)]
    public class RecipeRecord : ITable
    {
        public static List<RecipeRecord> Recipes = new List<RecipeRecord>();
        [Primary]
        public ushort ResultId;
        public int ResultNameId;
        [Ignore]
        public string ResultName { get; set; }
        public short ResultTypeId;
        public byte ResultLevel;
        public List<ushort> Ingredients;
        public List<uint> Quantities;
        public sbyte JobId;
        public ushort SkillId;
        [Ignore]
        public Dictionary<ushort, uint> IngredientsWithQuantities { get; set; }

        public RecipeRecord(ushort resultid,int resultnameid,short resulttypeid,byte resultlevel,List<ushort> ingredients,List<uint> quantitites,sbyte jobid,ushort skillid)
        {
            this.ResultId = resultid;
            this.ResultNameId = resultnameid;
            this.ResultName = LangManager.GetText(ResultNameId);
            this.ResultTypeId = resulttypeid;
            this.ResultLevel = resultlevel;
            this.Ingredients = ingredients;
            this.Quantities = quantitites;
            this.JobId = jobid;
            this.SkillId = skillid;
            this.IngredientsWithQuantities = new Dictionary<ushort, uint>();
            for (int i = 0; i < Ingredients.Count(); i++)
            {
                IngredientsWithQuantities.Add(Ingredients[i], Quantities[i]);
            }
        }
        public static RecipeRecord GetRecipe(ushort resultid)
        {
           return Recipes.Find(x => x.ResultId == resultid);
        }
        public static RecipeRecord GetRecipe(List<CharacterItemRecord> crafteditems,ushort skillid)
        {
            Dictionary<ushort,uint> ingredients = new Dictionary<ushort,uint>();
            foreach (var item in crafteditems)
            {
                if (!ingredients.ContainsKey(item.GID))
                ingredients.Add(item.GID,item.Quantity);
            }
            var template = RecipeRecord.Recipes.Find(x => x.IngredientsWithQuantities.ScramEqualDictionary<ushort, uint>(ingredients) && x.SkillId == skillid);
            return template;
        }
    }
}
