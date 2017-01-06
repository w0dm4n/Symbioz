using Symbioz.DofusProtocol.Types;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public class PrismFighter : Fighter
    {
        private const int PrismCreatureId = 3451;

        public PrismRecord Record { get; set; }
        public short Level { get; set; }

        public PrismFighter(PrismRecord prismRecord, FightTeam team, int allianceId) : base(team)
        {
            this.Record = prismRecord;
            this.Level = 50;
            this.ReadyToFight = true;
            this.Team = team;
            this.Fight = this.Team.Fight;
            this.AllianceId = allianceId;
        }

        public override void Initialize()
        {
            this.Direction = 3;
            this.CellId = (short)this.Record.CellId;
            this.ContextualId = Fight.PopNextNonPlayerId();
            this.FighterLook = this.Record.GetContextActorLook();

            short lifePoints = 5000;
            short actionPoints = 0;
            short movementPoints = 0;
            short strenght = 0;
            short wisdom = 0;

            this.FighterStats = new FighterStats(new CharacterStatsRecord(-1, lifePoints, 0, this.Level, 0, actionPoints, movementPoints, strenght, 0, wisdom, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 15, 15,
                15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), false, 0);
            this.FighterInformations = new GameFightMonsterInformations(this.ContextualId, this.FighterLook,
                new EntityDispositionInformations(this.CellId, this.Direction), (sbyte)this.Team.TeamColor, 0, true, this.FighterStats.GetMinimalStats(),
                new ushort[0], PrismCreatureId, (sbyte)this.Level);

            //TODO: LoadSpells
            base.Initialize();
        }

        public override void RefreshStats()
        {
            this.FighterInformations = new GameFightMonsterInformations(this.ContextualId, this.FighterLook,
                new EntityDispositionInformations(this.CellId, this.Direction), (sbyte)this.Team.TeamColor,
                0, !this.Dead, this.FighterStats.GetMinimalStats(), new ushort[0], PrismCreatureId, (sbyte)this.Level);
        }

        public override FightTeamMemberInformations GetFightMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(this.ContextualId, PrismCreatureId, (sbyte)this.Level);
        }

        public override int GetInitiative()
        {
            return this.Level;
        }

        public override SpellLevelRecord GetSpellLevel(ushort spellid)
        {
            return null;
        }

        public override string GetName()
        {
            return this.Record.Alliance.Name;
        }
    }
}
