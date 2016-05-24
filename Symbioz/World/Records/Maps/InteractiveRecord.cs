using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.ORM;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Maps
{
    [Table("Interactives",true)]
    public class InteractiveRecord : ITable
    {
        public static List< InteractiveRecord> Interactive = new List<InteractiveRecord>();
        [Primary]
        public int Id;
        public int MapId;
        public int SkillId;
        public int ElementId;
        public int ElementTypeId;
        public string ActionType;
        public string OptionalValue1;
        public string OptionalValue2;

        public InteractiveRecord(int id,int mapid,int skillid, int elementid, int elementtype, string actiontype, string optionalvalue1, string optionalvalue2)
        {
            this.Id = id;
            this.MapId = mapid;
            this.SkillId = skillid;
            this.ElementId = elementid;
            this.ElementTypeId = elementtype;
            this.ActionType = actiontype;
            this.OptionalValue1 = optionalvalue1;
            this.OptionalValue2 = optionalvalue2;
        }
        public InteractiveElement GetInteractiveElement()
        {
            return new InteractiveElement(ElementId, ElementTypeId, new List<InteractiveElementSkill>() { new InteractiveElementSkill((uint)SkillId, 1) }, new List<InteractiveElementSkill>());
        }
        public static InteractiveRecord GetInteractive(uint elemid)
        {
            return Interactive.Find(x => x.ElementId == elemid);
        }
        public static List<InteractiveRecord> GetInteractivesByActionType(string type)
        {
            return Interactive.FindAll(x => x.ActionType.ToLower() == type.ToLower());
        }
        public static List<InteractiveRecord> GetInteractivesOnMap(int mapid)
        {
            return Interactive.FindAll(x => x.MapId == mapid);
        }
        public static ushort GetTeleporterCellId(int mapid,TeleporterTypeEnum tptype)
        {
            var map = MapRecord.GetMap(mapid);
            string actionType = string.Empty;
            switch (tptype)
            {
                case TeleporterTypeEnum.TELEPORTER_ZAAP:
                    actionType = "Zaap";
                    break;
                case TeleporterTypeEnum.TELEPORTER_SUBWAY:
                    actionType = "Zaapi";
                    break;
                case TeleporterTypeEnum.TELEPORTER_PRISM:
                    actionType = "Prism";
                    break;
                default:
                    break;
            }
            var interactive = GetInteractivesByActionType(actionType).Find(x => x.MapId == mapid);
            if (interactive != null)
            {
                var mapElements = MapElementRecord.GetMapElementByMap(interactive.MapId);
                var ele = mapElements.Find(x => x.ElementId == interactive.ElementId);
                return (ushort)map.CloseCell((short)ele.CellId);
            }
            return (ushort)map.RandomWalkableCell();
          
        }
    }
}
