// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Skill : ID2OClass
    {
        [Cache]
        public static List<Skill> Skills = new List<Skill>();
        public Int32 id;
        public Int32 nameId;
        public Int32 parentJobId;
        public Boolean isForgemagus;
        public ArrayList modifiableItemTypeIds;
        public Int32 gatheredRessourceItem;
        public ArrayList craftableItemIds;
        public Int32 interactiveId;
        public String useAnimation;
        public Boolean isRepair;
        public Int32 cursor;
        public Int32 elementActionId;
        public Boolean availableInHouse;
        public Boolean clientDisplay;
        public Int32 levelMin;
        public Skill(Int32 id, Int32 nameId, Int32 parentJobId, Boolean isForgemagus, ArrayList modifiableItemTypeIds, Int32 gatheredRessourceItem, ArrayList craftableItemIds, Int32 interactiveId, String useAnimation, Boolean isRepair, Int32 cursor, Int32 elementActionId, Boolean availableInHouse, Boolean clientDisplay, Int32 levelMin)
        {
            this.id = id;
            this.nameId = nameId;
            this.parentJobId = parentJobId;
            this.isForgemagus = isForgemagus;
            this.modifiableItemTypeIds = modifiableItemTypeIds;
            this.gatheredRessourceItem = gatheredRessourceItem;
            this.craftableItemIds = craftableItemIds;
            this.interactiveId = interactiveId;
            this.useAnimation = useAnimation;
            this.isRepair = isRepair;
            this.cursor = cursor;
            this.elementActionId = elementActionId;
            this.availableInHouse = availableInHouse;
            this.clientDisplay = clientDisplay;
            this.levelMin = levelMin;
        }
    }
}
