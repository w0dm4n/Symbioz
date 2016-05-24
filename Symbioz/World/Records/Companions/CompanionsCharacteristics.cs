using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Companions
{
    [Table("CompanionsCharacteristics")]
    public class CompanionCharacteristic : ITable
    {
        public static List<CompanionCharacteristic> CompanionsCharacteristics = new List<CompanionCharacteristic>();

        public int Id;
        public short CaracId;
        public short CompanionId;
        public sbyte Order;
        public short InitialValue;
        public short LevelPerValue;
        public short ValuePerLevel;
        [Ignore]
        public CharacteristicRecord CharacteristicTemplate { get { return CharacteristicRecord.GetCharacteristic(CaracId); } }
        
        public CompanionCharacteristic(int id,short caracid,short companionid,sbyte order,short initialvalue,short levelpervalue,
            short valueperlevel)
        {
            this.Id = id;
            this.CaracId = caracid;
            this.CompanionId = companionid;
            this.Order = order;
            this.InitialValue = initialvalue;
            this.LevelPerValue = levelpervalue;
            this.ValuePerLevel = valueperlevel;
        }

        public static CompanionCharacteristic GetCompanionCharacteristics(int id)
        {
            return CompanionsCharacteristics.Find(x => x.Id == id);
        }
    }
}
