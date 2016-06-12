using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Alliances.Prisms.Modules
{
    [Table("PrismsModules")]
    public class PrismModuleRecord : ITable
    {
        public static List<PrismModuleRecord> PrismsModules = new List<PrismModuleRecord>();

        [Primary]
        public int UID;
        public int GID;
        public int PrismId;

        public PrismModuleRecord(int uid, int gid, int prismId)
        {
            this.UID = uid;
            this.GID = gid;
            this.PrismId = prismId;
        }

        public List<ObjectEffect> GetEffects()
        {
            return new List<ObjectEffect>();
        }

        public ObjectItem GetObjectItem()
        {
            return new ObjectItem(63, (ushort)this.GID, this.GetEffects(), (uint)this.UID, 1);
        }

        public static List<PrismModuleRecord> GetPrismModules(int prismId)
        {
            return PrismsModules.FindAll(x => x.PrismId == prismId);
        }

    }
}
