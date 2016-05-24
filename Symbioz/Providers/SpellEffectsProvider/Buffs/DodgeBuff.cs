using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Providers.SpellEffectsProvider.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class DodgeBuff : Buff
    {
        public override FighterEventType EventType
        {
            get
            {
                return FighterEventType.BEFORE_ATTACKED;
            }
        }
        public DodgeBuff(uint uid,short delta,short duration,int sourceid,short sourcespellid,int delay) : base(uid, delta, duration, sourceid, sourcespellid,delay) {  }

        public override void SetBuff()
        {
            Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)EffectsEnum.Eff_Dodge,
                SourceId, new FightTriggeredEffect(UID, Fighter.ContextualId, Duration, 1, (ushort)SourceSpellId, (uint)EffectsEnum.Eff_Dodge, 0, 100, Delta, 0, 0)));
        }

        public override void RemoveBuff()
        {
            
        }
        public void OverrideDamages(Fighter source)
        {
            var direction = ShapesProvider.GetDirectionFromTwoCells(Fighter.CellId,source.CellId);
            List<short> line = ShapesProvider.GetLineFromOposedDirection(Fighter.CellId,Delta, direction);
            List<short> cells = Fighter.Fight.BreakAtFirstObstacles(line);
            if (cells.Count > 0)
            {
                Fighter.Fight.TryStartSequence(Fighter.ContextualId, 5);
                Fighter.Fight.Send(new GameActionFightSlideMessage(0, Fighter.ContextualId,Fighter.ContextualId, Fighter.CellId, cells.Last()));
                Fighter.CellId = cells.Last();
                Fighter.Fight.TryEndSequence(5, 2);
            }
        }
        public override bool OnEventCalled(object arg1,object arg2,object arg3)
        {
            var source = Fighter.Fight.GetFighter((int)arg1);
            if (ShapesProvider.GetSquare(Fighter.CellId, false).Contains(source.CellId))
            {
                OverrideDamages(source);
                return true;
            }
            else
                return false;
        }
    }
}
