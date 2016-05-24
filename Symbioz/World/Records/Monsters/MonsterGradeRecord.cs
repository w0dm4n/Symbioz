using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Monsters
{
    [Table("MonstersGrades")]
    public class MonsterGradeRecord : ITable
    {
        public static List<MonsterGradeRecord> MonstersGrades = new List<MonsterGradeRecord>();

        public int GradeId;
        public ushort MonsterId;
        public short Level;
        public int LifePoints;
        public ushort ActionPoints;
        public short MovementPoints;
        public short PaDodge;
        public short PmDodge;
        public ushort Wisdom;
        public short EarthResistance;
        public short AirResistance;
        public short FireResistance;
        public short WaterResistance;
        public short NeutralResistance;
        public ulong GradeXp;
        public ushort DamageReflect;
        public short Power;

        public MonsterGradeRecord(int gradeid,ushort monsterid,short level,int lifepoints,ushort actionpoints,short movementpoints,
            short padodge,short pmdodge,ushort wisdom,short earthresist,short airresist,short fireresist,short waterresist,
            short neutralresist,ulong gradexp,ushort damagereflect,short power)
        {
            this.GradeId = gradeid;
            this.MonsterId = monsterid;
            this.Level = level;
            this.LifePoints = lifepoints;
            this.ActionPoints = actionpoints;
            this.MovementPoints = movementpoints;
            this.PaDodge = padodge;
            this.PmDodge = pmdodge;
            this.Wisdom = wisdom;
            this.EarthResistance = earthresist;
            this.AirResistance = airresist;
            this.FireResistance = fireresist;
            this.WaterResistance = waterresist;
            this.NeutralResistance = neutralresist;
            this.GradeXp = gradexp;
            this.DamageReflect = damagereflect;
            this.Power = power;
        }

        public static List<MonsterGradeRecord> GetMonsterGrades(ushort monsterid)
        {
            return MonstersGrades.FindAll(x => x.MonsterId == monsterid);
        }
    }
}
