using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.PathProvider;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.PathProvider;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights
{
    public class MarksHelper : Singleton<MarksHelper>
    {
        /// <summary>
        /// Nombre de cellules par mur de bombe
        /// </summary>
        public const int BOMB_LINE_SIZE = 5;

        public const char WALL_SHAPE = 'B';

        private List<short> GetCheckedCells(short bombCellId)
        {
            List<short> checkedCells = new List<short>();
            checkedCells.AddRange(ShapesProvider.GetLineFromDirection(bombCellId, BOMB_LINE_SIZE, DirectionsEnum.DIRECTION_NORTH_EAST));
            checkedCells.AddRange(ShapesProvider.GetLineFromDirection(bombCellId, BOMB_LINE_SIZE, DirectionsEnum.DIRECTION_NORTH_WEST));
            checkedCells.AddRange(ShapesProvider.GetLineFromDirection(bombCellId, BOMB_LINE_SIZE, DirectionsEnum.DIRECTION_SOUTH_WEST));
            checkedCells.AddRange(ShapesProvider.GetLineFromDirection(bombCellId, BOMB_LINE_SIZE, DirectionsEnum.DIRECTION_SOUTH_EAST));
            return checkedCells;
        }
        public IEnumerable<Wall> GetPotentialWalls(Fight fight, BombFighter bomb)
        {
            List<short> checkedCells = GetCheckedCells(bomb.CellId);

            foreach (var cellId in checkedCells)
            {
                BombFighter fighter = fight.GetFighter<BombFighter>(x => x.CellId == cellId && x.MonsterTemplate.Id == bomb.MonsterTemplate.Id);

                if (fighter != null && fighter != bomb && fighter.Fight.GetMarks<Wall>().Find(x=>x.Cells.Contains(cellId)) ==null)
                {
                    Wall wall = MarksHelper.Instance.CreateWall(bomb, fighter, (short)bomb.BombWallRecord.SpellId, bomb.Grade, bomb.BombWallRecord.Color);
                    yield return wall;
                }
            }
            yield break;
        }
        public bool IsValid(Wall wall)
        {
            List<short> checkedCells = GetCheckedCells(wall.FirstBomb.CellId);

            if (checkedCells.Contains(wall.SecondBomb.CellId))
                return true;
            else
                return false;
        }
        public Wall CreateWall(BombFighter source, BombFighter target, short spellId, sbyte spellGrade, int color)
        {
            int wallRadius = PathHelper.GetDistanceBetween(source.CellId, target.CellId);
            Wall wall = new Wall(source.Master, target.CellId, source.CellId, WALL_SHAPE, (short)wallRadius, spellId, spellGrade, color, source, target);
            return wall;
        }
        public void DirectExplosion(Fighter master,Fighter target,int bombMonsterId,sbyte spellGrade)
        {
            master.Fight.TryEndSequence(1, 0); // During Spell Cast

            BombWallRecord bwall = BombWallRecord.GetWallRecord(bombMonsterId);
            master.Fight.TryStartSequence(master.ContextualId, 1);

            master.Fight.Send(new GameActionFightSpellCastMessage(0, target.ContextualId, target.ContextualId, target.CellId,
                (sbyte)FightSpellCastCriticalEnum.NORMAL
                 , true,bwall.CibleDetonationSpellId, spellGrade, new short[0]));

            SpellLevelRecord elevel = SpellLevelRecord.GetLevel(bwall.CibleDetonationSpellId, spellGrade);
            target.HandleSpellEffects(elevel, target.CellId, FightSpellCastCriticalEnum.NORMAL);
            master.Fight.CheckFightEnd();
            master.Fight.TryEndSequence(1, 0);
        }
        public void CastDetonation(Fighter master,Fighter bomb,ushort explosionSpellId,int bombMonsterId,sbyte spellGrade)
        {
            master.Fight.TryEndSequence(1, 0); // During Spell Cast

            BombWallRecord bwall = BombWallRecord.GetWallRecord(bombMonsterId);
            master.Fight.TryStartSequence(master.ContextualId, 1);

            master.Fight.Send(new GameActionFightSpellCastMessage(0, master.ContextualId, 0, bomb.CellId,
               (sbyte)FightSpellCastCriticalEnum.NORMAL
                , true, bwall.CibleDetonationSpellId, spellGrade, new short[0]));

            SpellLevelRecord elevel = SpellLevelRecord.GetLevel(bwall.CibleDetonationSpellId, spellGrade);
            bomb.HandleSpellEffects(elevel, bomb.CellId, FightSpellCastCriticalEnum.NORMAL);
            master.Fight.CheckFightEnd();
            master.Fight.TryEndSequence(1, 0);
        }
        public static short[] GetPortals(Fight fight,short castCellId)
        {

            var portals = fight.GetMarks<Portal>().OrderByDescending(x => PathHelper.GetDistanceBetween(castCellId, castCellId)).ToList();
            if (portals.Count > 0)
            portals.Remove(portals.Last());
            return portals.ToList().ConvertAll<short>(x => x.CenterCell).ToArray();
        }

    }
}
