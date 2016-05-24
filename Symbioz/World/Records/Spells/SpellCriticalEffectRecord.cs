using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Spells
{
    [Table("SpellsEffectsCritical")]
    public class SpellCriticalEffectRecord :  AbstractSpellEffectRecord,ITable
    {
        public static List<SpellCriticalEffectRecord> SpellsEffectsCritical = new List<SpellCriticalEffectRecord>();

        
        public SpellCriticalEffectRecord(int effectuid,int spelllevelid,short effectid, string rawzone, string targetmask, short dicenum, short diceside,
            short duration,int value,int delay,int random)
        {
            this.SpellLevelId = spelllevelid;
            this.EffectId = effectid;
            this.RawZone = rawzone;
            this.TargetMask = targetmask;
            this.DiceNum = dicenum;
            this.DiceSide = diceside;
            this.Duration = duration;
            this.EffectUID = effectuid;
            this.Value = value;
            this.Delay = delay;
            this.Random = random;
        }
        public static List<ExtendedSpellEffect> GetSpellLevelEffects(int levelid)
        {
            return SpellsEffectsCritical.FindAll(x => x.SpellLevelId == levelid).ConvertAll<ExtendedSpellEffect>(w => new ExtendedSpellEffect(w));
        }
    }
}