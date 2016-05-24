using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Mounts",true)]
    public class MountRecord : ITable
    {
        public static List<MountRecord> Mounts = new List<MountRecord>();
        public uint Id;
        public string Name;
        public string Look;
        public ushort CertificateGID;
        [Ignore]
        public ContextActorLook RealLook { get; set; }
        public MountRecord(uint id,string name,string look,ushort certificategid)
        {
            this.Id = id;
            this.Name = name;
            this.Look = look;
            this.CertificateGID = certificategid;
            this.RealLook = ContextActorLook.Parse(Look);
            this.RealLook.indexedColors = ContextActorLook.GetDofusColors(this.RealLook.indexedColors);
        }
        public static MountRecord GetMountByCertificateGID(ushort gid)
        {
            return Mounts.Find(x => x.CertificateGID == gid);
        }
        public static MountRecord GetMountById(uint id)
        {
            return Mounts.Find(x => x.Id == id);
        }
        public static bool IsMountCertificate(ushort itemgid)
        {
            var mount = Mounts.Find(x => x.CertificateGID == itemgid);
            if (mount.IsNull())
            {
                return false;
            }
            else
                return true;
        }
    }
}
