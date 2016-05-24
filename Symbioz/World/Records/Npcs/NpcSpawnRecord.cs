using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("NpcsSpawns", true)]
    public class NpcSpawnRecord : ITable
    {
        public static List<NpcSpawnRecord> NpcSpawns = new List<NpcSpawnRecord>();

        [Primary]
        public int Id;
        public ushort TemplateId;
        public int MapId;
        public short CellId;
        public sbyte Direction;
        
        public NpcSpawnRecord(int id,ushort templateid,int mapid,short cellid,sbyte direction)
        {
            this.Id = id;
            this.TemplateId = templateid;
            this.MapId = mapid;
            this.CellId = cellid;
            this.Direction = direction;
        }
        public GameRolePlayNpcInformations GetGameRolePlayNpcInformations()
        {
            return new GameRolePlayNpcInformations(-Id, NpcTemplateRecord.GetNpcLook(TemplateId), new EntityDispositionInformations(CellId, Direction), TemplateId, false, 0);
        }
        public static List<NpcSpawnRecord> GetMapNpcs(int mapid)
        {
            return NpcSpawns.FindAll(x => x.MapId == mapid);
        }
        public static NpcSpawnRecord GetNpcByContextualId(int id)
        {
            return NpcSpawns.Find(x => x.Id == -id);
        }
    }
}
