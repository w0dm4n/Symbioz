using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Breeds", true)]
    public class BreedRecord : ITable
    {
        public static List<BreedRecord> Breeds = new List<BreedRecord>();

        public int Id;
        public string MaleLook;
        public string FemaleLook;
        public List<int> MaleColors;
        public List<int> FemaleColors;
        public short StartLifePoints;
        public short StartProspecting;
        public BreedStatField SPForIntelligence;
        public BreedStatField SPForAgility;
        public BreedStatField SPForStrenght;
        public BreedStatField SPForVitality;
        public BreedStatField SPForWisdom;
        public BreedStatField SPForChance;


        public BreedRecord(int id, string malelook, string femalelook, List<int> malecolors, List<int> femalecolors, short startlifepoints, short startprospecting,
            BreedStatField spforintelligence,BreedStatField spforagility,BreedStatField spforstrenght,BreedStatField spforvitality,BreedStatField spforwisdom,BreedStatField spforchance)
        {
            this.Id = id;
            this.MaleLook = malelook;
            this.FemaleLook = femalelook;
            this.MaleColors = malecolors;
            this.FemaleColors = femalecolors;
            this.StartLifePoints = startlifepoints;
            this.StartProspecting = startprospecting;
            this.SPForIntelligence = spforintelligence;
            this.SPForAgility = spforagility;
            this.SPForStrenght = spforstrenght;
            this.SPForVitality = spforvitality;
            this.SPForWisdom = spforwisdom;
            this.SPForChance = spforchance;
        }
        public uint[][] GetThresholds(StatsBoostTypeEnum statsid)
        {
            uint[][] result = null;
            switch (statsid)
            {
                case StatsBoostTypeEnum.Strength:
                    result = this.SPForStrenght.Values;
                    break;
                case StatsBoostTypeEnum.Vitality:
                    result = this.SPForVitality.Values;
                    break;
                case StatsBoostTypeEnum.Wisdom:
                    result = this.SPForWisdom.Values;
                    break;
                case StatsBoostTypeEnum.Chance:
                    result = this.SPForChance.Values;
                    break;
                case StatsBoostTypeEnum.Agility:
                    result = this.SPForAgility.Values;
                    break;
                case StatsBoostTypeEnum.Intelligence:
                    result = this.SPForIntelligence.Values;
                    break;
            }
            return result;
        }
        public uint[] GetThreshold(short actualpoints, StatsBoostTypeEnum statsid)
        {
            uint[][] thresholds = this.GetThresholds(statsid);
            return thresholds[this.GetThresholdIndex((int)actualpoints, thresholds)];
        }
        public int GetThresholdIndex(int actualpoints, uint[][] thresholds)
        {
            int result;
            for (int i = 0; i < thresholds.Length - 1; i++)
            {
                if ((ulong)thresholds[i][0] <= (ulong)((long)actualpoints) && (ulong)thresholds[i + 1][0] > (ulong)((long)actualpoints))
                {
                    result = i;
                    return result;
                }
            }
            result = thresholds.Length - 1;
            return result;
        }
        public static ushort AvailableBreedsFlags
        {
            get
            {
                return (ushort)AvailableBreeds.Aggregate(0, (int current, PlayableBreedEnum breedEnum) => current | 1 << breedEnum - PlayableBreedEnum.Feca);
            }
        }
        public static readonly List<PlayableBreedEnum> AvailableBreeds = new List<PlayableBreedEnum>
		{
			PlayableBreedEnum.Feca,
			PlayableBreedEnum.Osamodas,
			PlayableBreedEnum.Enutrof,
			PlayableBreedEnum.Ecaflip,
			PlayableBreedEnum.Eniripsa,
			PlayableBreedEnum.Iop,
			PlayableBreedEnum.Cra,
			PlayableBreedEnum.Sacrieur,
			PlayableBreedEnum.Zobal,
            PlayableBreedEnum.Eliotrope,
            PlayableBreedEnum.Sadida,
            PlayableBreedEnum.Sram,
            PlayableBreedEnum.Pandawa,
            PlayableBreedEnum.Steamer,
            PlayableBreedEnum.Xelor,
            PlayableBreedEnum.Pandawa
      
		};
        static int GetDefaultBreedColor(int breedid, bool sex, int colorindex)
        {
            var breed = Breeds.Find(x => x.Id == breedid);
            if (sex)
            {
                var colors = breed.FemaleColors;
                return colors[colorindex - 1];
            }
            else
            {
                var colors = breed.MaleColors;
                return colors[colorindex - 1];
            }

        }
        public static BreedRecord GetBreed(int id)
        {
            return Breeds.Find(x => x.Id == id);
        }
        public static ContextActorLook GetBreedEntityLook(int breedid, bool sex, int cosmeticid, IEnumerable<int> colors)
        {
            var breed = Breeds.Find(x => x.Id == breedid);
            ContextActorLook result;
            if (!sex)
            {
                result = ContextActorLook.Parse(breed.MaleLook);
            }
            else
            {
                result = ContextActorLook.Parse(breed.FemaleLook);
            }
            List<int> futureColors = new List<int>();
            int index = 1;
            foreach (var color in colors)
            {
                if (color == -1)
                {
                    futureColors.Add(GetDefaultBreedColor(breed.Id, sex, index));
                }
                else
                {
                    futureColors.Add(color);
                }
                index++;
            }
            result.indexedColors = ContextActorLook.GetDofusColors(futureColors);
            result.AddSkin((ushort)HeadRecord.GetSkinFromCosmeticId(cosmeticid));
            return result;

        }
        public class BreedStatField
        {
            public uint[][] Values = new uint[0][];
            public BreedStatField(uint[][] values)
            {
                this.Values = values;
            }
            public static BreedStatField Deserialize(string str)
            {
                List<uint[]> list = new List<uint[]>();
                foreach (var value in str.Split('|'))
                {
                    List<uint> subList = new List<uint>();
                    foreach (var value2 in value.Split(','))
                    {
                        subList.Add(uint.Parse(value2));
                    }
                    list.Add(subList.ToArray());
                }
                
                return new BreedStatField(list.ToArray());
            }
        }


    }
}
