using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Maps
{
    [Table("MapsTriggers")]
    public class MapTriggerRecord : ITable
    {
        public static List<MapTriggerRecord> MapsTriggers = new List<MapTriggerRecord>();

        public int Id;
        public int BaseMapId;
        public int BaseCellId;
        public int NextMapId;
        public int NextCellId;

        public MapTriggerRecord(int id, int baseMapId, int baseCellId, int nextMapId, int nextCellId)
        {
            this.Id = id;
            this.BaseMapId = baseMapId;
            this.BaseCellId = baseCellId;
            this.NextMapId = nextMapId;
            this.NextCellId = nextCellId;
        }

        public MapTriggerRecord GetMapTrigger(int baseMapId)
        {
            return MapsTriggers.FirstOrDefault(x => x.BaseMapId == baseMapId);
        }
    }
}
