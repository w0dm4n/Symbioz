using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Spells
{
    public class ExtendedSpellEffect
    {
        public List<string> Targets { get { return BaseEffect.TargetMask.Split(',').ToList(); } }
        public char ZoneShape { get { return BaseEffect.RawZone[0]; } }
        public short ZoneSize { get { return short.Parse(BaseEffect.RawZone[1].ToString()); } }
        public AbstractSpellEffectRecord BaseEffect { get; set; }
        public ExtendedSpellEffect(AbstractSpellEffectRecord effect)
        {
            this.BaseEffect = effect;
        }

    }
}
