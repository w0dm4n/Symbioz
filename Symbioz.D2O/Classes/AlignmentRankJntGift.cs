// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlignmentRankJntGift : ID2OClass
    {
        [Cache]
        public static List<AlignmentRankJntGift> AlignmentRankJntGifts = new List<AlignmentRankJntGift>();
        public Int32 id;
        public Int32[] gifts;
        public Int32[] parameters;
        public Int32[] levels;
        public AlignmentRankJntGift(Int32 id, Int32[] gifts, Int32[] parameters, Int32[] levels)
        {
            this.id = id;
            this.gifts = gifts;
            this.parameters = parameters;
            this.levels = levels;
        }
    }
}
