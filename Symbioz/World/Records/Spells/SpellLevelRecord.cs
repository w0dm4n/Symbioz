using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Spells
{
    [Table("SpellsLevels")]
    public class SpellLevelRecord : ITable
    {
        public static List<SpellLevelRecord> SpellsLevels = new List<SpellLevelRecord>();
        [Primary]
        public int Id;
        public ushort SpellId;
        public sbyte Grade;
        public short ApCost;
        public short MinRange;
        public short MaxRange;
        public bool CastInLine;
        public bool CastInDiagonal;
        public bool CastTestLos;
        public sbyte CriticalHitProbability;
        public bool NeedFreeCell;
        public bool NeedTakenCell;
        public bool NeedFreeTrapCell;
        public bool RangeCanBeBoosted;
        public short MaxStack;
        public short MaxCastPerTurn;
        public short MaxCastPerTarget;
        public short MinCastInterval;
        public short InitialCooldown;
        public short GlobalCooldown;
        public short MinPlayerLevel;
        public List<short> StatesRequired;
        public List<short> StatesForbidden;

        [Ignore]
        public List<ExtendedSpellEffect> Effects { get { return SpellEffectRecord.GetSpellLevelEffects(Id); } }
        [Ignore]
        public List<ExtendedSpellEffect> CriticalEffects { get { return SpellCriticalEffectRecord.GetSpellLevelEffects(Id); } }

        public SpellLevelRecord(int id, ushort spellid, sbyte grade, short apcost, short minrange, short maxrange, bool castinline,
            bool castindiagonal, bool casttestlos, sbyte criticalhitprobability, bool needfreecell, bool needtakencell, bool needfreetrapcell,
            bool rangecanbeboosted, short maxstack, short maxcastperturn, short maxcastpertarget, short mincastinterval,
            short initialcooldown, short globalcooldown, short minplayerlevel, List<short> statesrequired, List<short> statesforbidden)
        {
            this.Id = id;
            this.SpellId = spellid;
            this.Grade = grade;
            this.ApCost = apcost;
            this.MinRange = minrange;
            this.MaxRange = maxrange;
            this.CastInLine = castinline;
            this.CastInDiagonal = castindiagonal;
            this.CastTestLos = casttestlos;
            this.CriticalHitProbability = criticalhitprobability;
            this.NeedFreeCell = needfreecell;
            this.NeedTakenCell = needtakencell;
            this.NeedFreeTrapCell = needfreetrapcell;
            this.RangeCanBeBoosted = rangecanbeboosted;
            this.MaxStack = maxstack;
            this.MaxCastPerTurn = maxcastperturn;
            this.MaxCastPerTarget = maxcastpertarget;
            this.MinCastInterval = mincastinterval;
            this.InitialCooldown = initialcooldown;
            this.GlobalCooldown = globalcooldown;
            this.MinPlayerLevel = minplayerlevel;
            this.StatesRequired = statesrequired;
            this.StatesForbidden = statesforbidden;
            
        }
        public static SpellLevelRecord GetLevel(int levelid)
        {
            return SpellsLevels.Find(x => x.Id == levelid);
        }
        public static SpellLevelRecord GetLevel(ushort spellid,sbyte grade)
        {
            var spell = SpellsLevels.Find(x => x.SpellId == spellid && x.Grade == grade);
            if (spell != null)
                return spell;
            else
            {
                var spells = SpellsLevels.FindAll(x => x.SpellId == spellid);
                return spells.OrderByDescending(x => x.Grade).Last();
            }
        }
    }
}
