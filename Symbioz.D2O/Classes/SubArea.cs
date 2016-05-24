// automatic generation Symbioz.Sync 2015

using Symbioz.D2O.InternalClasses;
using Symbioz.DofusProtocol.D2O;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SubArea : ID2OClass
    {
        [Cache]
        public static List<SubArea> SubAreas = new List<SubArea>();
        public Int32 id;
        public Int32 nameId;
        public Int32 areaId;
        public AmbientSound[] ambientSounds;
        public ArrayList[] playlists;
        public UInt32[] mapIds;
        public Rectangle bounds;
        public Int32[] shape;
        public ArrayList customWorldMap;
        public UInt32 packId;
        public UInt32 level;
        public Boolean isConquestVillage;
        public Boolean basicAccountAllowed;
        public Boolean displayOnWorldMap;
        public UInt32[] monsters;
        public ArrayList entranceMapIds;
        public ArrayList exitMapIds;
        public Boolean capturable;
        public SubArea(Int32 id, Int32 nameId, Int32 areaId, object[] ambientSounds, ArrayList[] playlists, UInt32[] mapIds, Rectangle bounds, Int32[] shape, ArrayList customWorldMap, UInt32 packId, UInt32 level, Boolean isConquestVillage, Boolean basicAccountAllowed, Boolean displayOnWorldMap, UInt32[] monsters, ArrayList entranceMapIds, ArrayList exitMapIds, Boolean capturable)
        {
            this.id = id;
            this.nameId = nameId;
            this.areaId = areaId;
            this.ambientSounds = ambientSounds.Cast<AmbientSound>().ToArray();
            this.playlists = playlists;
            this.mapIds = mapIds;
            this.bounds = bounds;
            this.shape = shape;
            this.customWorldMap = customWorldMap;
            this.packId = packId;
            this.level = level;
            this.isConquestVillage = isConquestVillage;
            this.basicAccountAllowed = basicAccountAllowed;
            this.displayOnWorldMap = displayOnWorldMap;
            this.monsters = monsters;
            this.entranceMapIds = entranceMapIds;
            this.exitMapIds = exitMapIds;
            this.capturable = capturable;
        }
    }
}
