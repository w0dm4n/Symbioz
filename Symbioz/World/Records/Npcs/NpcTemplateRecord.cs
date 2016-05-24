using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("NpcsTemplates",true)]
    class NpcTemplateRecord : ITable
    {
        public static List<NpcTemplateRecord> NpcsTemplates = new List<NpcTemplateRecord>();

        [Primary]
        public int Id;
        public string Look;
        [Ignore]
        public ContextActorLook RealLook;

        public NpcTemplateRecord(int id,string look)
        {
            this.Id = id;
            this.Look = look;
            this.RealLook = ContextActorLook.Parse(Look);
        }

        public static EntityLook GetNpcLook(int id)
        {
            return NpcsTemplates.Find(x => x.Id == id).RealLook.ToEntityLook();
        }
    }
}
