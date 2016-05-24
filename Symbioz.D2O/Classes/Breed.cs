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
    public class Breed : ID2OClass
    {
        [Cache]
        public static List<Breed> Breeds = new List<Breed>();
        public Int32 id;
        public Int32 shortNameId;
        public Int32 longNameId;
        public Int32 descriptionId;
        public Int32 gameplayDescriptionId;
        public String maleLook;
        public String femaleLook;
        public Int32 creatureBonesId;
        public Int32 maleArtwork;
        public Int32 femaleArtwork;
        public ArrayList[] statsPointsForStrength;
        public ArrayList[] statsPointsForIntelligence;
        public ArrayList[] statsPointsForChance;
        public ArrayList[] statsPointsForAgility;
        public ArrayList[] statsPointsForVitality;
        public ArrayList[] statsPointsForWisdom;
        public UInt32[] breedSpellsId;
        public BreedRoleByBreed[] breedRoles;
        public UInt32[] maleColors;
        public UInt32[] femaleColors;
        public Int32 spawnMap;
        public Breed(Int32 id, Int32 shortNameId, Int32 longNameId, Int32 descriptionId, Int32 gameplayDescriptionId, String maleLook, String femaleLook, Int32 creatureBonesId, Int32 maleArtwork, Int32 femaleArtwork, ArrayList[] statsPointsForStrength, ArrayList[] statsPointsForIntelligence, ArrayList[] statsPointsForChance, ArrayList[] statsPointsForAgility, ArrayList[] statsPointsForVitality, ArrayList[] statsPointsForWisdom, UInt32[] breedSpellsId, object[] breedRoles, UInt32[] maleColors, UInt32[] femaleColors, Int32 spawnMap)
        {
            this.id = id;
            this.shortNameId = shortNameId;
            this.longNameId = longNameId;
            this.descriptionId = descriptionId;
            this.gameplayDescriptionId = gameplayDescriptionId;
            this.maleLook = maleLook;
            this.femaleLook = femaleLook;
            this.creatureBonesId = creatureBonesId;
            this.maleArtwork = maleArtwork;
            this.femaleArtwork = femaleArtwork;
            this.statsPointsForStrength = statsPointsForStrength;
            this.statsPointsForIntelligence = statsPointsForIntelligence;
            this.statsPointsForChance = statsPointsForChance;
            this.statsPointsForAgility = statsPointsForAgility;
            this.statsPointsForVitality = statsPointsForVitality;
            this.statsPointsForWisdom = statsPointsForWisdom;
            this.breedSpellsId = breedSpellsId;
            this.breedRoles = breedRoles.Cast<BreedRoleByBreed>().ToArray();
            this.maleColors = maleColors;
            this.femaleColors = femaleColors;
            this.spawnMap = spawnMap;
        }
    }
}
