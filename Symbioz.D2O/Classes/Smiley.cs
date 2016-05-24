// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Smiley : ID2OClass
    {
        [Cache]
        public static List<Smiley> Smileys = new List<Smiley>();
        public Int32 id;
        public Int32 order;
        public String gfxId;
        public Boolean forPlayers;
        public String[] triggers;
        public Smiley(Int32 id, Int32 order, String gfxId, Boolean forPlayers, String[] triggers)
        {
            this.id = id;
            this.order = order;
            this.gfxId = gfxId;
            this.forPlayers = forPlayers;
            this.triggers = triggers;
        }
    }
}
