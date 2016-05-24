// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class NpcMessage : ID2OClass
    {
        [Cache]
        public static List<NpcMessage> NpcMessages = new List<NpcMessage>();
        public Int32 id;
        public Int32 messageId;
        public NpcMessage(Int32 id, Int32 messageId)
        {
            this.id = id;
            this.messageId = messageId;
        }
    }
}
