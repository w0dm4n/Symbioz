// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class ServerPopulation : ID2OClass
    {
        [Cache]
        public static List<ServerPopulation> ServerPopulations = new List<ServerPopulation>();
        public Int32 id;
        public Int32 nameId;
        public Int32 weight;
        public ServerPopulation(Int32 id, Int32 nameId, Int32 weight)
        {
            this.id = id;
            this.nameId = nameId;
            this.weight = weight;
        }
    }
}
