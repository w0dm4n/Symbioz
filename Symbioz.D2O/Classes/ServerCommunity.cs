// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class ServerCommunity : ID2OClass
    {
        [Cache]
        public static List<ServerCommunity> ServerCommunitys = new List<ServerCommunity>();
        public Int32 id;
        public Int32 nameId;
        public String[] defaultCountries;
        public String shortId;
        public ServerCommunity(Int32 id, Int32 nameId, String[] defaultCountries, String shortId)
        {
            this.id = id;
            this.nameId = nameId;
            this.defaultCountries = defaultCountries;
            this.shortId = shortId;
        }
    }
}
