using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Succes
{
    [Table("Succes", true)]
    class Succes : ITable
    {
        [Primary]
        public int Id;
        public short SuccesId;
        public int SubAreaDiscoveredId;
        public int KamasRatio;
        public int XPRatio;
        public int MonsterId;
    }
}
