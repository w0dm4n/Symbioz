using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class FrictionBuff : Buff
    {
        public override FighterEventType EventType
        {
            get
            {
                return FighterEventType.BEFORE_ATTACKED;
            }
        }
        public FrictionBuff(uint uid, short delta, short duration, int sourceid, short sourcespellid, int delay)
            : base(uid, delta, duration, sourceid, sourcespellid, delay)
        {

        }
        public override void SetBuff()
        {
            Fighter.Fight.Send(new GameActionFightDispellableEffectMessage((ushort)EffectsEnum.Eff_792, SourceId, new FightTemporaryBoostEffect((uint)Fighter.BuffIdProvider.Pop(), Fighter.ContextualId, Duration, 1, (ushort)SourceSpellId, (uint)EffectsEnum.Eff_792, 0, Delta)));
        }

        public override void RemoveBuff()
        {

        }
        public override bool OnEventCalled(object arg1, object arg2, object arg3)
        {
           
            Fighter source = Fighter.Fight.GetFighter(SourceId);

            if (!Fighter.IsAligned(source))
                return false;

            DirectionsEnum dir = ShapesProvider.GetDirectionFromTwoCells(Fighter.CellId, source.CellId);
            List<short> cells = Fighter.Fight.BreakAtFirstObstacles(ShapesProvider.GetLineFromDirection(Fighter.CellId, 1, dir));

            if (cells.Count > 0) 
                Fighter.Slide(SourceId, new List<short>() { Fighter.CellId, cells[0] });

            return false;
        }
    }
}
