// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Companion : ID2OClass
    {
        [Cache]
        public static List<Companion> Companions = new List<Companion>();
        public Int32 id;
        public Int32 nameId;
        public String look;
        public Boolean webDisplay;
        public Int32 descriptionId;
        public Int32 startingSpellLevelId;
        public Int32 assetId;
        public UInt32[] characteristics;
        public UInt32[] spells;
        public Int32 creatureBoneId;
        public Companion(Int32 id, Int32 nameId, String look, Boolean webDisplay, Int32 descriptionId, Int32 startingSpellLevelId, Int32 assetId, UInt32[] characteristics, UInt32[] spells, Int32 creatureBoneId)
        {
            this.id = id;
            this.nameId = nameId;
            this.look = look;
            this.webDisplay = webDisplay;
            this.descriptionId = descriptionId;
            this.startingSpellLevelId = startingSpellLevelId;
            this.assetId = assetId;
            this.characteristics = characteristics;
            this.spells = spells;
            this.creatureBoneId = creatureBoneId;
        }
    }
}
