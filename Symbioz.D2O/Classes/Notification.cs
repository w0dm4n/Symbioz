// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Notification : ID2OClass
    {
        [Cache]
        public static List<Notification> Notifications = new List<Notification>();
        public Int32 id;
        public Int32 titleId;
        public Int32 messageId;
        public Int32 iconId;
        public Int32 typeId;
        public String trigger;
        public Notification(Int32 id, Int32 titleId, Int32 messageId, Int32 iconId, Int32 typeId, String trigger)
        {
            this.id = id;
            this.titleId = titleId;
            this.messageId = messageId;
            this.iconId = iconId;
            this.typeId = typeId;
            this.trigger = trigger;
        }
    }
}
