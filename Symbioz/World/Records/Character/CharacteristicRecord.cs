using Symbioz.ORM;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Characteristics")]
    public class CharacteristicRecord : ITable
    {
        public static List<CharacteristicRecord> Characteristics = new List<CharacteristicRecord>();

        public int Id;
        public string Keyword;
        public string Name;
        public sbyte Order;
        public sbyte CategoryId;

        public CharacteristicRecord(int id, string keyword, string name, sbyte order, sbyte categoryid)
        {
            this.Id = id;
            this.Keyword = keyword;
            this.Name = name;
            this.Order = order;
            this.CategoryId = categoryid;
        }
        public static CharacteristicRecord GetCharacteristic(string keyword)
        {
            return Characteristics.Find(x => x.Keyword == keyword);
        }
        public static CharacteristicRecord GetCharacteristic(int id)
        {
            return Characteristics.Find(x => x.Id == id);
        }
       
        public static UInt16ReflectedStat GetReflectedStat(StatsRecord host,int characteristicid)
        {
            CharacteristicRecord record = Characteristics.Find(x => x.Id == characteristicid);
            return new UInt16ReflectedStat(StatsRecord.GetFieldInfo(record.Keyword), host);
        }
    }
}
