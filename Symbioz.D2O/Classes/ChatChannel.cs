// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class ChatChannel : ID2OClass
    {
        [Cache]
        public static List<ChatChannel> ChatChannels = new List<ChatChannel>();
        public Int32 id;
        public Int32 nameId;
        public Int32 descriptionId;
        public String shortcut;
        public String shortcutKey;
        public Boolean isPrivate;
        public Boolean allowObjects;
        public ChatChannel(Int32 id, Int32 nameId, Int32 descriptionId, String shortcut, String shortcutKey, Boolean isPrivate, Boolean allowObjects)
        {
            this.id = id;
            this.nameId = nameId;
            this.descriptionId = descriptionId;
            this.shortcut = shortcut;
            this.shortcutKey = shortcutKey;
            this.isPrivate = isPrivate;
            this.allowObjects = allowObjects;
        }
    }
}
