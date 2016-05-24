// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Recipe : ID2OClass
    {
        [Cache]
        public static List<Recipe> Recipes = new List<Recipe>();
        public Int32 resultId;
        public Int32 resultNameId;
        public UInt32 resultTypeId;
        public Int32 resultLevel;
        public Int32[] ingredientIds;
        public UInt32[] quantities;
        public Int32 jobId;
        public Int32 skillId;
        public Recipe(Int32 resultId, Int32 resultNameId, UInt32 resultTypeId, Int32 resultLevel, Int32[] ingredientIds, UInt32[] quantities, Int32 jobId, Int32 skillId)
        {
            this.resultId = resultId;
            this.resultNameId = resultNameId;
            this.resultTypeId = resultTypeId;
            this.resultLevel = resultLevel;
            this.ingredientIds = ingredientIds;
            this.quantities = quantities;
            this.jobId = jobId;
            this.skillId = skillId;
        }
    }
}
