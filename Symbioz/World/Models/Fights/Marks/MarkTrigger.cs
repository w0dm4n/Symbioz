using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.PathProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights
{
    public abstract class MarkTrigger
    {
        public delegate void MarkInteractionDelegate(Fighter source, object arg1, object arg2, object arg3);

        public void Intitialize(Fight fight, Type marktype)
        {
            var interactionDefinitions = InteractionsManager.GetEventsMethods(marktype);
            foreach (var data in interactionDefinitions)
            {
                fight.MarkInteractions.Add(new MarkInteraction(this, data.Value, data.Key));
            }

            OnCasted(GetAffecteds(fight));

        }
        public IEnumerable<Fighter> GetAffecteds(Fight fight)
        {
            foreach (var fighter in fight.GetAllFighters())
            {
                if (Cells.Contains(fighter.CellId))
                    yield return fighter;
            }
        }
        public virtual GameActionMarkTypeEnum MarkType { get; set; }

        public short Id { get; set; }

        public char ShapeType { get; set; }

        public List<short> Cells { get; set; }

        public List<GameActionMarkedCell> MarkedCells { get; set; }

        public short CenterCell { get; set; }

        public Fighter Caster { get; set; }

        public short AssociatedSpellId { get; set; }

        public sbyte AssociatedSpellGrade { get; set; }

        public int MarkColor { get; set; }

        public short Radius { get; set; }

        public MarkTrigger(Fighter caster, short centercell, short entitycell, char shapetype, short radius, short associatedspellid, sbyte spellgrade, int markcolor)
        {
            this.Id = (short)caster.Fight.MarkIdProvider.Pop();
            this.CenterCell = centercell;
            this.Radius = radius;
            this.Caster = caster;
            this.ShapeType = shapetype;
            this.Cells = ShapesProvider.Handle(shapetype, centercell, entitycell, radius);
            this.AssociatedSpellId = associatedspellid;
            this.AssociatedSpellGrade = spellgrade;
            this.MarkColor = markcolor;
            this.MarkedCells = GetMarkedCells();

        }
        public List<GameActionMarkedCell> GetMarkedCells()
        {
            List<GameActionMarkedCell> results = new List<GameActionMarkedCell>();
            GameActionMarkCellsTypeEnum cellsType;
            if (ShapeType == 'C')
            {
                cellsType = GameActionMarkCellsTypeEnum.CELLS_CIRCLE;
                results.Add(new GameActionMarkedCell((ushort)CenterCell, (sbyte)Radius, MarkColor, (sbyte)cellsType));
            }
            else if (ShapeType == 'X')
            {
                cellsType = GameActionMarkCellsTypeEnum.CELLS_CROSS;
                results.Add(new GameActionMarkedCell((ushort)CenterCell, (sbyte)Radius, MarkColor, (sbyte)cellsType));
            }
            else if (ShapeType == 'P')
            {
                cellsType = 0;
                Radius = 0;
                results.Add(new GameActionMarkedCell((ushort)CenterCell, (sbyte)Radius, MarkColor, (sbyte)cellsType));
            }
            else
            {
                cellsType = GameActionMarkCellsTypeEnum.CELLS_SQUARE;
                Radius = 0;
                foreach (var cell in Cells)
                {
                    results.Add(new GameActionMarkedCell((ushort)cell, (sbyte)Radius, MarkColor, (sbyte)cellsType));
                }
            }
            return results;
        }
        public virtual GameActionMark GetMark()
        {
            return new GameActionMark(Caster.ContextualId, (sbyte)Caster.Team.TeamColor, AssociatedSpellId, AssociatedSpellGrade, Id, (sbyte)MarkType, CenterCell, MarkedCells, true);
        }
        public virtual void OnCasted(IEnumerable<Fighter> affecteds) { }


    }
}
