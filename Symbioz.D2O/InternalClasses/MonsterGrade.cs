// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class MonsterGrade : ID2OInternalClass
    {
        public Int32 grade;
        public Int32 monsterId;
        public Int32 level;
        public Int32 lifePoints;
        public Int32 actionPoints;
        public Int32 movementPoints;
        public Int32 paDodge;
        public Int32 pmDodge;
        public Int32 wisdom;
        public Int32 earthResistance;
        public Int32 airResistance;
        public Int32 fireResistance;
        public Int32 waterResistance;
        public Int32 neutralResistance;
        public Int32 gradeXp;
        public Int32 damageReflect;
        public MonsterGrade(Int32 grade, Int32 monsterId, Int32 level, Int32 lifePoints, Int32 actionPoints, Int32 movementPoints, Int32 paDodge, Int32 pmDodge, Int32 wisdom, Int32 earthResistance, Int32 airResistance, Int32 fireResistance, Int32 waterResistance, Int32 neutralResistance, Int32 gradeXp, Int32 damageReflect)
        {
            this.grade = grade;
            this.monsterId = monsterId;
            this.level = level;
            this.lifePoints = lifePoints;
            this.actionPoints = actionPoints;
            this.movementPoints = movementPoints;
            this.paDodge = paDodge;
            this.pmDodge = pmDodge;
            this.wisdom = wisdom;
            this.earthResistance = earthResistance;
            this.airResistance = airResistance;
            this.fireResistance = fireResistance;
            this.waterResistance = waterResistance;
            this.neutralResistance = neutralResistance;
            this.gradeXp = gradeXp;
            this.damageReflect = damageReflect;
        }

       
    }
}
