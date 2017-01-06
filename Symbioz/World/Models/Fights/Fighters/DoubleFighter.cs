using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Providers.ActorIA;
using Symbioz.Providers.ActorIA.Actions;
using Symbioz.World.PathProvider;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public class DoubleFighter : Fighter
    {
        public Fighter Master { get; set; }


        public DoubleFighter(FightTeam team,Fighter master,int castcellid) :base(team)
        {
            ContextualId = master.Fight.PopNextNonPlayerId();
            this.Master = master;
            this.CellId = (short)castcellid;
            this.FighterLook = Master.RealFighterLook.CloneContextActorLook();
            this.FighterStats = master.FighterStats;
            FighterInformations = new GameFightCharacterInformations(ContextualId, FighterLook.ToEntityLook(),
               new EntityDispositionInformations((short)castcellid, this.Master.Direction), (sbyte)Team.TeamColor,
               0, true, FighterStats.GetMinimalStats(), new ushort[0], master.GetName(), new PlayerStatus(0),
               200,new ActorAlignmentInformations(),1,
               true);
            this.CellId = (short)castcellid;


        }
        public override FightTeamMemberInformations GetFightMemberInformations()
        {
            return new FightTeamMemberCharacterInformations(ContextualId,this.GetName(),200);
        }

        public override Records.Spells.SpellLevelRecord GetSpellLevel(ushort spellid)
        {
            return SpellLevelRecord.GetLevel(spellid);

        }

        public override int GetInitiative()
        {
            return Master.GetInitiative();
        }
        public override void StartTurn()
        {
            base.StartTurn();
            this.Execute(this);
            base.EndTurn();

        }
        public override void RefreshStats()
        {
            this.FighterInformations = new GameFightCharacterInformations(ContextualId, FighterLook,
               new EntityDispositionInformations(CellId, Direction), (sbyte)Team.TeamColor,
               0, !Dead, FighterStats.GetMinimalStats(), new ushort[0], this.GetName(), new PlayerStatus(0),
               200, new ActorAlignmentInformations(), 1,
               true);
        }
        public override string GetName()
        {
            return this.Master.GetName();
        }

        public void Execute(Fighter fighter)
        {
            if (fighter.FighterStats.Stats.MovementPoints <= 0)
                return;
            Logger.Log("MOVETOLOWERENEMY");
            // search fighter lower by pm
            Fighter lower = fighter.GetOposedTeam().LowerProchFighter(fighter, fighter.FighterStats.Stats.MovementPoints);

            //si c'est un summon que que c'est notre fin de tour ont regarde plus loin un fighter non summon
          /*  if (fighter.FighterStats.Stats.ActionPoints <= 2 && lower is MonsterFighter && (lower as MonsterFighter).isSummon)
                lower = fighter.GetOposedTeam().LowerProchFighter(fighter, 6);*/

            var path = new Pathfinder(fighter.Fight.Map, fighter.CellId);

            path.PutEntities(fighter.Fight.GetAllFighters());
            var cells = path.FindPathProche(lower.CellId, fighter.FighterStats.Stats.MovementPoints, fighter.CellId);
            if (cells == null || cells.Count() <= 1)
                return;
            cells.Remove(cells.Last());
            cells.Insert(0, fighter.CellId);
            cells = cells.Take(fighter.FighterStats.Stats.MovementPoints + 1).ToList();
            sbyte direction = PathParser.GetDirection(cells.Last());

            fighter.Move(cells, cells.Last(), direction);
        }
    }
}
