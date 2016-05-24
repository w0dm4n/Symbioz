using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Monsters
{
    [Table("MonstersDrops")]
    public class MonsterDropRecord : ITable
    {
        public static List<MonsterDropRecord> MonstersDrops = new List<MonsterDropRecord>();

        public int DropId;
        public ushort MonsterId;
        public ushort ObjectId;
        public double PercentDropForGrade1;
        public double PercentDropForGrade2;
        public double PercentDropForGrade3;
        public double PercentDropForGrade4;
        public double PercentDropForGrade5;
        public int Count;
        public ushort ProspectingLock;
        public MonsterDropRecord(int dropid,ushort monsterid,ushort objectid,double pdf1,double pdf2,double pdf3,
            double pdf4,double pdf5,int count,ushort prospetinglock)
        {
            this.DropId = dropid;
            this.MonsterId = monsterid;
            this.ObjectId = objectid;
            this.PercentDropForGrade1 = pdf1;
            this.PercentDropForGrade2 = pdf2;
            this.PercentDropForGrade3 = pdf3;
            this.PercentDropForGrade4 = pdf4;
            this.PercentDropForGrade5 = pdf5;
            this.Count = count;
            this.ProspectingLock = prospetinglock;
        }
        public double GetDropRate(sbyte grade)
        {
            switch (grade)
            {
                case 1:
                    return PercentDropForGrade1;
                case 2:
                    return PercentDropForGrade2;
                case 3:
                    return PercentDropForGrade3;
                case 4:
                    return PercentDropForGrade4;
                case 5:
                    return PercentDropForGrade5;
            }
            return 0;
        }
        public static List<MonsterDropRecord> GetMonsterDrops(ushort monsterid)
        {
            return MonstersDrops.FindAll(x => x.MonsterId == monsterid);
        }


    }
}
