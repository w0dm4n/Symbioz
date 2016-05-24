using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Companions
{
    [Table("Companions")]
    public class CompanionRecord : ITable
    {
        public static List<CompanionRecord> Companions = new List<CompanionRecord>();

        public sbyte Id;
        public string Name;
        public string Look;
        [Ignore]
        public ContextActorLook RealLook {get{return ContextActorLook.Parse(Look);}}
        public List<ushort> Characteristics;
        public List<int> Spells;
        public int StartingSpellLevelId;
        [Ignore]
        public List<CompanionSpellRecord> RealSpells = new List<CompanionSpellRecord>();
        [Ignore]
        public List<CompanionCharacteristic> RealCharacteristics = new List<CompanionCharacteristic>();

        public CompanionRecord(sbyte id,string name,string look,List<ushort> characteristics,List<int> spells,int startingspelllevelid)
        {
            this.Id = id;
            this.Name = name;
            this.Look = look;
            this.Characteristics = characteristics;
            this.Spells = spells;
            this.StartingSpellLevelId = startingspelllevelid;

            foreach (var spell in Spells)
            {
                RealSpells.Add(CompanionSpellRecord.GetSpell(spell));
            }
            foreach (var carac in Characteristics)
            {
                RealCharacteristics.Add(CompanionCharacteristic.GetCompanionCharacteristics(carac));
            }
        }
        public List<SpellItem> GetSpellItems(Character master)
        {
            return RealSpells.ConvertAll<SpellItem>(x => new SpellItem(63, x.SpellId, GetSpellGrade(master)));
        }
        public static sbyte GetSpellGrade(Character master)
        {
            sbyte grade = 1;
            if (master.Record.Level >= 40)
                grade++;
            if (master.Record.Level >= 80)
                grade++;
            if (master.Record.Level >= 120)
                grade++;
            if (master.Record.Level >= 160)
                grade++;
            if (master.Record.Level == 200)
                grade++;
            return grade;
        }
        public short GetStatData(Character character,string name)
        {
            short delta = 0;
            var characteristic = RealCharacteristics.Find(x => x.CharacteristicTemplate.Keyword == name);
            if (characteristic == null)
                return 0;
            delta += characteristic.InitialValue;
            if (characteristic.LevelPerValue > 0)
            {
                for (int i = 0; i < character.Record.Level; i += characteristic.LevelPerValue)
                {
                    delta += characteristic.ValuePerLevel;
                }
            }
            return delta;
        }
        public FighterStats GetFighterStats(Character master) 
        {
            List<object> constructorDatas = new List<object>();
            foreach (var field in typeof(StatsRecord).GetFields().ToList().FindAll(x=>!x.IsStatic).OrderBy(x=>x.MetadataToken))
            {
                constructorDatas.Add(GetStatData(master, field.Name));
            }
            StatsRecord stats = (StatsRecord)Activator.CreateInstance(typeof(StatsRecord), constructorDatas.ToArray());
            stats.Initiative = (short)(master.Initiative - 1);
            stats.Strength = master.StatsRecord.Strength;
            stats.Agility = master.StatsRecord.Agility;
            stats.Intelligence = master.StatsRecord.Intelligence;
            stats.Chance = master.StatsRecord.Chance;
         
            return new FighterStats(stats);
        }
        public static CompanionRecord GetCompanion(int id)
        {
            return Companions.Find(x => x.Id == id);
        }
    }
}
