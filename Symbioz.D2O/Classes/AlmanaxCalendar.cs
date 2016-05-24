// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AlmanaxCalendar : ID2OClass
    {
        [Cache]
        public static List<AlmanaxCalendar> AlmanaxCalendars = new List<AlmanaxCalendar>();

        public Int32 id;
        public Int32 nameId;
        public Int32 descId;
        public Int32 npcId;
        public AlmanaxCalendar(Int32 id, Int32 nameId, Int32 descId, Int32 npcId)
        {
            this.id = id;
            this.nameId = nameId;
            this.descId = descId;
            this.npcId = npcId;
        }
    }
}
