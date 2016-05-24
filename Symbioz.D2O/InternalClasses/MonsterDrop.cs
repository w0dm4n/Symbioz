// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class MonsterDrop : ID2OInternalClass
    {
       
        public Int32 dropId;
        public Int32 monsterId;
        public Int32 objectId;
        public Double percentDropForGrade1;
        public Double percentDropForGrade2;
        public Double percentDropForGrade3;
        public Double percentDropForGrade4;
        public Double percentDropForGrade5;
        public Int32 count;
        public Boolean hasCriteria;
        public MonsterDrop(Int32 dropId, Int32 monsterId, Int32 objectId, Double percentDropForGrade1, Double percentDropForGrade2, Double percentDropForGrade3, Double percentDropForGrade4, Double percentDropForGrade5, Int32 count, Boolean hasCriteria)
        {
            this.dropId = dropId;
            this.monsterId = monsterId;
            this.objectId = objectId;
            this.percentDropForGrade1 = percentDropForGrade1;
            this.percentDropForGrade2 = percentDropForGrade2;
            this.percentDropForGrade3 = percentDropForGrade3;
            this.percentDropForGrade4 = percentDropForGrade4;
            this.percentDropForGrade5 = percentDropForGrade5;
            this.count = count;
            this.hasCriteria = hasCriteria;
        }

        
    }
}
