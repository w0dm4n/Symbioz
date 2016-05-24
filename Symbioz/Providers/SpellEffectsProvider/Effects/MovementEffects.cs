using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Servers;
using Symbioz.PathProvider;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Models;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Effects
{
    class MovementEffects
    {
        [EffectHandler(EffectsEnum.Eff_RepelsTo)] // Peur du sram
        public static void RepelsTo(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            var dir = ShapesProvider.GetDirectionFromTwoCells(fighter.CellId, castcellid);
            var movedCellAmount = (short)(PathHelper.GetDistanceBetween(fighter.CellId, castcellid));
            fighter.Fight.Reply(movedCellAmount.ToString());
            var line = ShapesProvider.GetLineFromDirection(fighter.CellId, movedCellAmount, dir);
            if (line.Count > 0)
            {
                var target = fighter.Fight.GetFighter(line.First());
                if (target != null)
                {
                    line.Remove(target.CellId);
                    List<short> cells = fighter.Fight.BreakAtFirstObstacles(line);
                    if (cells.Count > 0)
                        target.Slide(fighter.ContextualId, cells);
                }
            }
        }
        [EffectHandler(EffectsEnum.Eff_1099)] // Rembobinage
        public static void LastTurnPositionMove(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (Fighter affected in affecteds)
            {
                if (affected.LastTurnPosition == 0)
                    continue;
                if (fighter.Fight.GetFighter(affected.LastTurnPosition) != null)
                {
                    Fighter other = fighter.Fight.GetFighter(affected.LastTurnPosition);
                    SwitchPosition(other, null, effect, new List<Fighter> { affected }, castcellid);
                }
                else
                {
                    affected.Teleport(affected.LastTurnPosition, false);
                }
            }
        }
        [EffectHandler(EffectsEnum.Eff_1100)] // Gelure/Fuite
        public static void LastPositionMove(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (Fighter affected in affecteds)
            {
                if (affected.LastPosition.Count <= 0)
                    continue;
                if (fighter.Fight.GetFighter(affected.LastPosition.Last()) != null)
                {
                    Fighter other = fighter.Fight.GetFighter(affected.LastPosition.Last());
                    SwitchPosition(other, null, effect, new List<Fighter> { affected }, castcellid);
                }
                else
                {
                    affected.Teleport(affected.LastPosition.Last(),false);
                }
                affected.LastPosition.Remove(affected.LastPosition.Last());
            }
        }
        [EffectHandler(EffectsEnum.Eff_1104)] // Xelor
        public static void SymetryToTargetMove(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            var symetrySource = fighter.Fight.GetFighter(castcellid);
            if (symetrySource == null)
                return;
            var target = fighter.Fight.GetFighter(castcellid);
            var direction = ShapesProvider.GetDirectionFromTwoCells(target.CellId, fighter.CellId);
            short destinationCell = (short)((target.CellId * 2) - fighter.CellId);
            if (ShapesProvider.IsDiagonalDirection(direction))
            {
                if (PathHelper.GetDistanceBetween(fighter.CellId, target.CellId) % 2 != 0) {
                    destinationCell = (short)((target.CellId * 2) - fighter.CellId - 1);
                }
                else
                    destinationCell = (short)((target.CellId * 2) - fighter.CellId);
            }
            else if (PathHelper.GetDistanceBetween(fighter.CellId, castcellid) % 2 != 0)
            {
                destinationCell = (short)((target.CellId * 2) - fighter.CellId -1);
            }
            if (fighter.Fight.IsObstacle(destinationCell) && fighter.Fight.GetFighter(destinationCell) == null)
                return;
            if (fighter.Fight.GetFighter(destinationCell) != null)
            {
                SwitchPosition(fighter, null, null, new List<Fighter>() { fighter.Fight.GetFighter(destinationCell) }, destinationCell);
            }
            else
            {
                fighter.Teleport(destinationCell);
            }
        }
        [EffectHandler(EffectsEnum.Eff_1105)]
        public static void SymetryToLauncherMove(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (Fighter affected in affecteds)
            {
                var direction = ShapesProvider.GetDirectionFromTwoCells(castcellid, fighter.CellId);
                short destinationCell = (short)((fighter.CellId * 2) - castcellid);
                if (ShapesProvider.IsDiagonalDirection(direction))
                {
                    if (PathHelper.GetDistanceBetween(fighter.CellId, affected.CellId)%2 != 0)
                        destinationCell = (short)((fighter.CellId * 2) - castcellid + 1);
                    else
                        destinationCell = (short)((fighter.CellId * 2) - castcellid);
                }
                else if(PathHelper.GetDistanceBetween(fighter.CellId, affected.CellId) % 2 != 0)
                {
                    destinationCell = (short)((fighter.CellId * 2) - castcellid + 1);
                }
                if (fighter.Fight.IsObstacle(destinationCell) && fighter.Fight.GetFighter(destinationCell) == null)
                    return;
                if (fighter.Fight.GetFighter(destinationCell) != null)
                {
                    SwitchPosition(affected, null, null, new List<Fighter>() { fighter.Fight.GetFighter(destinationCell) }, destinationCell);
                }
                else
                {
                    affected.Teleport(destinationCell);
                }
            }
        }
        [EffectHandler(EffectsEnum.Eff_1106)] // symétrie esprit 
        public static void SymetryMove(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            var direction = ShapesProvider.GetDirectionFromTwoCells(fighter.CellId, castcellid);
            var line = ShapesProvider.GetLineFromDirection(fighter.CellId, 2, direction);
            short destinationCell = line.Last();
            if (fighter.Fight.IsObstacle(destinationCell))
            {
                var target = fighter.Fight.GetFighter(destinationCell);
                if (target != null)
                {
                    SwitchPosition(fighter, null, null, new List<Fighter>() { target }, destinationCell);
                }
            }
            else
            {
                fighter.Teleport(destinationCell);
            }
        }
        [EffectHandler(EffectsEnum.Eff_SwitchPosition)]
        public static void SwitchPosition(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            bool save = true;
            if(effect != null && effect.BaseEffect.EffectType == EffectsEnum.Eff_1100)
            {
                save = false;
            }
            var target = affecteds.Find(x => x.CellId == castcellid);
            if (target != null)
            {
                target.Teleport(fighter.CellId,save);
                fighter.Teleport(castcellid,save);
            }
        }
        [EffectHandler(EffectsEnum.Eff_Teleport)]  //Bond de Iop
        public static void Teleport(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            if (fighter.Fight.IsObstacle(castcellid))
                return;
            fighter.Fight.Send(new SequenceStartMessage(5, fighter.ContextualId));
            fighter.Fight.Send(new GameActionFightTeleportOnSameMapMessage(4, fighter.ContextualId, fighter.ContextualId, castcellid));
            fighter.Fight.Send(new SequenceEndMessage(2, fighter.ContextualId, 5));
            fighter.ApplyFighterEvent(FighterEventType.ON_TELEPORTED, castcellid);
            fighter.CellId = castcellid;
        }
        [EffectHandler(EffectsEnum.Eff_1041)]  //Appui du zobal 
        public static void PushCaster(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                List<short> line = ShapesProvider.GetLineFromOposedDirection(target.CellId, (byte)(effect.BaseEffect.DiceNum + 1), ShapesProvider.GetDirectionFromTwoCells(fighter.CellId, target.CellId));

                List<short> cells = new List<short>();
                foreach (var cell in line)
                {
                    if (!fighter.Fight.IsObstacle(cell) || cell == fighter.CellId)
                        cells.Add(cell);
                    else
                        break;
                }
                if (cells.Count > 0)
                {
                    fighter.Slide(fighter.ContextualId, cells);
                }
            }

        }
        [EffectHandler(EffectsEnum.Eff_BePulled)]
        public static void BePulled(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                var direction = ShapesProvider.GetDirectionFromTwoCells(target.CellId, fighter.CellId);
                List<short> line = ShapesProvider.GetLineFromDirection(target.CellId, (byte)effect.BaseEffect.DiceNum, direction);
                List<short> cells = fighter.Fight.BreakAtFirstObstacles(line);
                cells.Reverse();

                if (cells.Count > 0)
                {
                    fighter.Slide(fighter.ContextualId, cells);
                }
            }
        }
        [EffectHandler(EffectsEnum.Eff_PushBack_1103)]
        public static void PushBack1103(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            affecteds.Remove(fighter);
            PushBack(fighter, level, effect, affecteds, castcellid);
        }
        [EffectHandler(EffectsEnum.Eff_PullForward)]
        public static void PullForward(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                DirectionsEnum direction;
                if (target.CellId != castcellid)
                {
                    direction = ShapesProvider.GetDirectionFromTwoCells(target.CellId, castcellid);
                }
                else
                {
                    direction = ShapesProvider.GetDirectionFromTwoCells(target.CellId, fighter.CellId);
                }
                List<short> line = ShapesProvider.GetLineFromDirection(target.CellId, effect.BaseEffect.DiceNum, direction);
                List<short> cells = fighter.Fight.BreakAtFirstObstacles(line);
                if (cells.Count > 0)
                {
                    target.Slide(fighter.ContextualId, cells);
                }
            }
        }
        [EffectHandler(EffectsEnum.Eff_PushBack)]
        public static void PushBack(Fighter fighter, SpellLevelRecord level, ExtendedSpellEffect effect, List<Fighter> affecteds, short castcellid)
        {
            foreach (var target in affecteds)
            {
                DirectionsEnum direction = 0;

                if (castcellid == target.CellId)
                {
                    direction = ShapesProvider.GetDirectionFromTwoCells(fighter.CellId, target.CellId);
                }
                else
                {
                    direction = ShapesProvider.GetDirectionFromTwoCells(castcellid, target.CellId);
                }
                List<short> line = ShapesProvider.GetLineFromDirection(target.CellId, (byte)effect.BaseEffect.DiceNum, direction);
                List<short> cells = fighter.Fight.BreakAtFirstObstacles(line);
                if (cells.Count > 0)
                {
                    target.Slide(fighter.ContextualId, cells);
                }
            }

        }
    }
}
