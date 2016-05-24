using Symbioz.Core;
using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Monsters
{
    [Table("Monsters")]
    public class MonsterRecord : ITable
    {
        public static List<MonsterRecord> Monsters = new List<MonsterRecord>();

        public List<MonsterDropRecord> Drops { get { return MonsterDropRecord.GetMonsterDrops(Id); } }
        public List<MonsterGradeRecord> Grades { get { return MonsterGradeRecord.GetMonsterGrades(Id); } }
        public ushort Id;
        public int NameId;
        [Ignore]
        public string Name;
        public int Race;
        public string Look;
        [Ignore]
        public ContextActorLook RealLook;
        public bool UseSummonSlot;
        public bool UseBombSlot;
        public bool IsBoss;
        public List<ushort> Spells;
        public List<int> IAActions;
        public int MinKamas;
        public int MaxKamas;


        public MonsterGradeRecord GetGrade(sbyte id)
        {
            return Grades.Find(x => x.GradeId == id);
        }

        public MonsterRecord(ushort id,int nameid,int race,string look,bool usesummondslot,bool usebombslot,bool isboss,List<ushort> spells,List<int> iaactions,int minkamas,int maxkamas)
        {
            this.Id = id;
            this.NameId = nameid;
            this.Name = LangManager.GetText(NameId);
            this.Race = race;
            this.Look = look;
            if (Look != string.Empty)
            this.RealLook = ContextActorLook.Parse(Look);
            this.UseSummonSlot = usesummondslot;
            this.UseBombSlot = usebombslot;
            this.IsBoss = isboss;
            this.Spells = spells;
            this.IAActions = iaactions;
            this.MinKamas = minkamas;
            this.MaxKamas = maxkamas;
        }
        public FighterStats GetFighterStats(sbyte gradeId,bool summon = false,int summonerid = 0)
        {
            var grade = GetGrade(gradeId);
            return new FighterStats(new StatsRecord(-1, (short)grade.LifePoints, 0, grade.Level, 0,  (short)grade.ActionPoints, grade.MovementPoints, grade.Power, 0,(short) grade.Wisdom, grade.Power, grade.Power, grade.Power, 0, 0,
                (short)grade.DamageReflect, 0, 0, 0, 0 ,0,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, grade.PaDodge, grade.PmDodge,grade.NeutralResistance, grade.EarthResistance, grade.WaterResistance,
                grade.AirResistance, grade.FireResistance,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),summon,summonerid);
        }

        public static MonsterRecord GetMonster(ushort id)
        {
            return Monsters.Find(x => x.Id == id);
        }
        
    }
}
