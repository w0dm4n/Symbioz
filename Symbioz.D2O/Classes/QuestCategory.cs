// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class QuestCategory : ID2OClass
    {
        [Cache]
        public static List<QuestCategory> QuestCategorys = new List<QuestCategory>();
        public Int32 id;
        public Int32 nameId;
        public Int32 order;
        public UInt32[] questIds;
        public QuestCategory(Int32 id, Int32 nameId, Int32 order, UInt32[] questIds)
        {
            this.id = id;
            this.nameId = nameId;
            this.order = order;
            this.questIds = questIds;
        }
    }
}
