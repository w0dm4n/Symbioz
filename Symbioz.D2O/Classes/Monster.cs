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
    public class Monster : ID2OClass
    {
        [Cache]
        public static List<Monster> Monsters = new List<Monster>();

        public Int32 id;
        public Int32 nameId;
        public Int32 gfxId;
        public Int32 race;
        public MonsterGrade[] grades;
        public String look;
        public Boolean useSummonSlot;
        public Boolean useBombSlot;
        public Boolean canPlay;
        public AnimFunMonsterData[] animFunList;
        public Boolean canTackle;
        public Boolean isBoss;
        public MonsterDrop[] drops;
        public UInt32[] subareas;
        public ArrayList spells;
        public Int32 favoriteSubareaId;
        public Boolean isMiniBoss;
        public Boolean isQuestMonster;
        public Int32 correspondingMiniBossId;
        public Int32 speedAdjust;
        public Int32 creatureBoneId;
        public Boolean canBePushed;
        public Boolean fastAnimsFun;
        public Boolean canSwitchPos;
        public ArrayList incompatibleIdols;
        public Monster(Int32 id, Int32 nameId, Int32 gfxId, Int32 race,object[] grades, String look, Boolean useSummonSlot, Boolean useBombSlot, Boolean canPlay,object[] animFunList, Boolean canTackle, Boolean isBoss,object[] drops, UInt32[] subareas, ArrayList spells, Int32 favoriteSubareaId, Boolean isMiniBoss, Boolean isQuestMonster, Int32 correspondingMiniBossId, Int32 speedAdjust, Int32 creatureBoneId, Boolean canBePushed, Boolean fastAnimsFun, Boolean canSwitchPos, ArrayList incompatibleIdols)
        {
            this.id = id;
            this.nameId = nameId;
            this.gfxId = gfxId;
            this.race = race;
            this.grades = grades.Cast<MonsterGrade>().ToArray();
            this.look = look;
            this.useSummonSlot = useSummonSlot;
            this.useBombSlot = useBombSlot;
            this.canPlay = canPlay;
            this.animFunList = animFunList.Cast<AnimFunMonsterData>().ToArray();
            this.canTackle = canTackle;
            this.isBoss = isBoss;
            this.drops = drops.Cast<MonsterDrop>().ToArray();
            this.subareas = subareas;
            this.spells = spells;
            this.favoriteSubareaId = favoriteSubareaId;
            this.isMiniBoss = isMiniBoss;
            this.isQuestMonster = isQuestMonster;
            this.correspondingMiniBossId = correspondingMiniBossId;
            this.speedAdjust = speedAdjust;
            this.creatureBoneId = creatureBoneId;
            this.canBePushed = canBePushed;
            this.fastAnimsFun = fastAnimsFun;
            this.canSwitchPos = canSwitchPos;
            this.incompatibleIdols = incompatibleIdols;
        }
    }
}
