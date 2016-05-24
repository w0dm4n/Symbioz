using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class LookBuff : Buff
    {
        public ContextActorLook AppliedLook { get; set; }
        public LookBuff(uint uid,short delta, short duration, int sourceid, short sourcespellid,int delay)
            : base(uid,delta, duration, sourceid, sourcespellid,delay)
        {

        }
        public override void SetBuff()
        {
            ContextActorLook look = FightLookProvider.GetLook(Fighter, (ushort)SourceSpellId);
            if (look != null)
            {
                Fighter.Fight.Send(new GameActionFightChangeLookMessage((ushort)ActionsEnum.ACTION_CHARACTER_CHANGE_LOOK, Fighter.ContextualId,
                     Fighter.ContextualId, look.ToEntityLook()));
                Fighter.FighterLook = look;
                this.AppliedLook = look;
            }
            else
            {
                Fighter.Fight.Reply("No look associated to this spell...");
            }
        }

        public override void RemoveBuff()
        {
            Fighter.FighterLook = Fighter.RealFighterLook;
            Fighter.Fight.Send(new GameActionFightChangeLookMessage((ushort)ActionsEnum.ACTION_CHARACTER_CHANGE_LOOK,SourceId, Fighter.ContextualId, Fighter.FighterLook.ToEntityLook()));
            var buffs = Fighter.GetAllBuffs<LookBuff>();
            buffs.Remove(this);
            buffs.ForEach(x=>x.SetBuff()); // l'idéal serait de ne pas envoyer de gameactionfightchange et juste de refresh a la fin
            
        }
    }
}
