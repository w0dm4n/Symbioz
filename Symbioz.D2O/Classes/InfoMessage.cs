// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class InfoMessage : ID2OClass
    {
        [Cache]
        public static List<InfoMessage> InfoMessages = new List<InfoMessage>();
        public Int32 typeId;
        public Int32 messageId;
        public Int32 textId;
        public InfoMessage(Int32 typeId, Int32 messageId, Int32 textId)
        {
            this.typeId = typeId;
            this.messageId = messageId;
            this.textId = textId;
        }
    }
}
