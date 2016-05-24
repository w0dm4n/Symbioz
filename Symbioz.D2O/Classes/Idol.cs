// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Idol : ID2OClass
    {
        [Cache]
        public static List<Idol> Idols = new List<Idol>();
        public Int32 id;
        public String description;
        public Int32 categoryId;
        public Int32 itemId;
        public Boolean groupOnly;
        public Int32 score;
        public Int32 experienceBonus;
        public Int32 dropBonus;
        public Int32 spellPairId;
        public Int32[] synergyIdolsIds;
        public Double[] synergyIdolsCoeff;
        public Idol(Int32 id, String description, Int32 categoryId, Int32 itemId, Boolean groupOnly, Int32 score, Int32 experienceBonus, Int32 dropBonus, Int32 spellPairId, Int32[] synergyIdolsIds, Double[] synergyIdolsCoeff)
        {
            this.id = id;
            this.description = description;
            this.categoryId = categoryId;
            this.itemId = itemId;
            this.groupOnly = groupOnly;
            this.score = score;
            this.experienceBonus = experienceBonus;
            this.dropBonus = dropBonus;
            this.spellPairId = spellPairId;
            this.synergyIdolsIds = synergyIdolsIds;
            this.synergyIdolsCoeff = synergyIdolsCoeff;
        }
    }
}
