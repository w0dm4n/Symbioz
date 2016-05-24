using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class StateBuff : Buff
    {

        public StateBuff(uint uid,short delta, short duration,int sourceid,short sourcespellid,int delay):base(uid,delta,duration,sourceid,sourcespellid,delay)
        {
             
        }
        public override void SetBuff()
        {
            Fighter.AddState(UID,Delta, SourceId, Duration, SourceSpellId);
        }

        public override void RemoveBuff()
        {
            Fighter.RemoveState(Delta,SourceSpellId);
        }
    }
}
