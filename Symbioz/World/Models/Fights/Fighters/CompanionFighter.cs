using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Records.Companions;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    /// <summary>
    /// Roublabot, Roublard sur le même principe
    /// </summary>
    public class CompanionFighter : Fighter
    {
        public CompanionRecord Template { get; set; }
        public CharacterFighter Master { get; set; }
        public List<SpellItem> Spells { get; set; }
        public List<ShortcutSpell> Shortcuts { get; set; }
        public SpellLevelRecord AutoCastSpell { get; set; }

        public CompanionFighter(CompanionRecord template, CharacterFighter master, FightTeam team)
            : base(team)
        {
            this.Template = template;
            this.Master = master;
            this.Spells = Template.GetSpellItems(Master.Client.Character);
            this.Shortcuts = GenerateShortcuts();
            if (this.Template.StartingSpellLevelId != 0)
            this.AutoCastSpell = SpellLevelRecord.GetLevel(Template.StartingSpellLevelId);
            this.ReadyToFight = true;

        }
        public override void StartTurn()
        {
            RefreshFighter();
            base.StartTurn();
        }
        public override void EndTurn()
        {
            if (AutoCastSpell != null)
                this.CastSpellOnCell(AutoCastSpell.SpellId, this.CellId);
            base.EndTurn();
        }
        public override void Initialize()
        {
            base.Initialize();
            ContextualId = Fight.PopNextNonPlayerId();
            FighterLook = Template.RealLook.CloneContextActorLook();

            FighterStats = Template.GetFighterStats(Master.Client.Character);
            FighterInformations = new GameFightCompanionInformations(ContextualId, Template.RealLook, new EntityDispositionInformations(CellId, Direction),
                (sbyte)Team.TeamColor, 0, true, FighterStats.GetMinimalStats(), new ushort[0], Template.Id, Master.Client.Character.Record.Level, Master.Client.Character.Id);
        }
        public void SwitchContext()
        {
            Master.Client.Send(new SlaveSwitchContextMessage(Master.ContextualId, this.ContextualId, Spells, Master.FighterStats.GetCharacterCharacteristics(Master.Client.Character), Shortcuts));
        }
        public override FightTeamMemberInformations GetFightMemberInformations()
        {
            return new FightTeamMemberCompanionInformations(ContextualId, Template.Id, Master.Client.Character.Record.Level, Master.Client.Character.Id);
        }
        public override void SwapPosition(Fighter fighter)
        {
            short fighterCellId = fighter.CellId;
            fighter.CellId = this.CellId;
            this.CellId = fighterCellId;
            IdentifiedEntityDispositionInformations[] identifieds = new IdentifiedEntityDispositionInformations[2];
            identifieds[0] = (fighter.GetIdentifiedEntityDisposition());
            identifieds[1] = (this.GetIdentifiedEntityDisposition());
            RefreshStats();
            Fight.Send(new GameFightPlacementSwapPositionsMessage(identifieds));
        }
        public override SpellLevelRecord GetSpellLevel(ushort spellid)
        {
            if (this.AutoCastSpell == null)
                return SpellLevelRecord.GetLevel(spellid, CompanionRecord.GetSpellGrade(Master.Client.Character));
            else
                return this.AutoCastSpell.SpellId == spellid ? this.AutoCastSpell : SpellLevelRecord.GetLevel(spellid, CompanionRecord.GetSpellGrade(Master.Client.Character));
        }
  
        public override int GetInitiative()
        {
            return Master.GetInitiative() - 10;
        }


        public override void RefreshStats()
        {
            this.FighterInformations = new GameFightCompanionInformations(ContextualId, FighterLook, new EntityDispositionInformations(CellId, Direction), (sbyte)Team.TeamColor,
                0, !Dead, FighterStats.GetMinimalStats(), new ushort[0], Template.Id, Master.Client.Character.Record.Level, Master.ContextualId);
        }
        public List<ShortcutSpell> GenerateShortcuts()
        {
            List<ShortcutSpell> shortcuts = new List<ShortcutSpell>();
            for (sbyte i = 0; i < Spells.Count; i++)
            {
                shortcuts.Add(new ShortcutSpell(i, (ushort)Spells[i].spellId));
            }
            return shortcuts;
        }
        public override string GetName()
        {
            return Template.Name;
        }
    }
}
