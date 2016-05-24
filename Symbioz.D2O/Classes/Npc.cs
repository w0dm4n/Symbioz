// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Npc : ID2OClass
    {
        [Cache]
        public static List<Npc> Npcs = new List<Npc>();
        public Int32 id;
        public Int32 nameId;
        public ArrayList[] dialogMessages;
        public ArrayList dialogReplies;
        public UInt32[] actions;
        public Int32 gender;
        public String look;
        public ArrayList animFunList;
        public Boolean fastAnimsFun;
        public Npc(Int32 id, Int32 nameId, ArrayList[] dialogMessages, ArrayList dialogReplies, UInt32[] actions, Int32 gender, String look, ArrayList animFunList, Boolean fastAnimsFun)
        {
            this.id = id;
            this.nameId = nameId;
            this.dialogMessages = dialogMessages;
            this.dialogReplies = dialogReplies;
            this.actions = actions;
            this.gender = gender;
            this.look = look;
            this.animFunList = animFunList;
            this.fastAnimsFun = fastAnimsFun;
        }
    }
}
