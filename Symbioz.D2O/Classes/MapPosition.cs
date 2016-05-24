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
    public class MapPosition : ID2OClass
    {
        [Cache]
        public static List<MapPosition> MapPositions = new List<MapPosition>();
        public Int32 id;
        public Int32 posX;
        public Int32 posY;
        public Boolean outdoor;
        public Int32 capabilities;
        public Int32 nameId;
        public Boolean showNameOnFingerpost;
        public AmbientSound[] sounds;
        public ArrayList[] playlists;
        public Int32 subAreaId;
        public Int32 worldMap;
        public Boolean hasPriorityOnWorldmap;
        public MapPosition(Int32 id, Int32 posX, Int32 posY, Boolean outdoor, Int32 capabilities, Int32 nameId, Boolean showNameOnFingerpost, object[] sounds, ArrayList[] playlists, Int32 subAreaId, Int32 worldMap, Boolean hasPriorityOnWorldmap)
        {
            this.id = id;
            this.posX = posX;
            this.posY = posY;
            this.outdoor = outdoor;
            this.capabilities = capabilities;
            this.nameId = nameId;
            this.showNameOnFingerpost = showNameOnFingerpost;
            this.sounds = sounds.Cast<AmbientSound>().ToArray();
            this.playlists = playlists;
            this.subAreaId = subAreaId;
            this.worldMap = worldMap;
            this.hasPriorityOnWorldmap = hasPriorityOnWorldmap;
        }
    }
}
