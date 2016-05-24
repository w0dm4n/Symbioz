using Symbioz.Enums;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("NpcsActions",true)]
   public class NpcActionsRecord : ITable
    {
        public static List<NpcActionsRecord> NpcsActions = new List<NpcActionsRecord>();
        [Primary]
        public int Id;
        public int NpcId;
        public sbyte ActionId;
        [Ignore]
        public NpcActionTypeEnum ActionType { get { return (NpcActionTypeEnum)ActionId; } }
        public string OptionalValue1;
        public string OptionalValue2;

        public NpcActionsRecord(int id,int npcid,sbyte actionid,string optionalvalue1,string optionalvalue2)
        {
            this.Id = id;
            this.NpcId = npcid;
            this.ActionId = actionid;
            this.OptionalValue1 = optionalvalue1;
            this.OptionalValue2 = optionalvalue2;
        }
        public static NpcActionsRecord GetNpcAction(int npcid,NpcActionTypeEnum type)
        {
            return NpcsActions.Find(x => x.NpcId == npcid && x.ActionType == type);
        }
    }
}
