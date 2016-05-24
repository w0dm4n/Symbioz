using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public class BombFighter : Fighter
    {
        public Fighter Master { get; set; }

        public MonsterRecord MonsterTemplate { get; set; }

        public sbyte Grade { get; set; }

        public BombWallRecord BombWallRecord { get; set; }

        public List<Wall> Walls = new List<Wall>();

        public BombFighter(Fighter master,FightTeam team,MonsterRecord monsterTemplate,short cellid,sbyte grade) : base(team) 
        {
            this.MonsterTemplate = monsterTemplate;
            this.ReadyToFight = true;
            this.Team = team;
            this.Team.AddBomb(this);
            this.Master = master;
            this.Grade = grade;
            this.InitializeBomb(master.ContextualId, cellid);
            this.BombWallRecord = BombWallRecord.GetWallRecord(monsterTemplate.Id);
        }
        public override FightTeamMemberInformations GetFightMemberInformations()
        {
            return null;
        }
        public override void OnMoved(List<short> cells)
        {
            CheckWalls();
        }
        public override void BeforeMove(List<short> cells)
        {
           
        }
        public override SpellLevelRecord GetSpellLevel(ushort spellid)
        {
            return SpellLevelRecord.GetLevel(spellid, Grade);
        }
        public void InitializeBomb(int summonerid, short cellid)
        {
            this.Direction = 3;
            this.CellId = cellid;
            this.ContextualId = Fight.PopNextNonPlayerId();
            this.FighterLook = MonsterTemplate.RealLook.CloneContextActorLook();
            this.RealFighterLook = FighterLook.CloneContextActorLook();
            this.FighterStats = MonsterTemplate.GetFighterStats(Grade, true, summonerid);
            this.FighterInformations = new GameFightMonsterInformations(ContextualId, FighterLook,
              new EntityDispositionInformations(CellId, Direction),
              (sbyte)Team.TeamColor, 0, true, FighterStats.GetMinimalStats(), new ushort[0], MonsterTemplate.Id,Grade);

        }
        public override int GetInitiative()
        {
            return 0;
        }
        public override void StartTurn()
        {
            base.StartTurn();
        }
        public override void Die()
        {
            Walls.ForEach(x => Fight.RemoveMarkTrigger(Master, x));
            base.Die();
        }
        public override void RefreshStats()
        {
            this.FighterInformations = new GameFightMonsterInformations(ContextualId, FighterLook,
               new EntityDispositionInformations(CellId, Direction),
               (sbyte)Team.TeamColor, 0, true, FighterStats.GetMinimalStats(), new ushort[0], MonsterTemplate.Id, Grade);
        }
        public override string GetName()
        {
            return MonsterTemplate.Name;
        }
        public void CheckWalls()
        {
           
            var createdWalls = MarksHelper.Instance.GetPotentialWalls(Fight, this);

            foreach (var wall in createdWalls)
            {
                Walls.Add(wall);
                Fight.AddMarkTrigger(this.Master, wall);
            }

            for (int i = 0; i < Walls.Count; i++)
            {
                Wall wall = Walls[i];
                if (!MarksHelper.Instance.IsValid(wall))
                {
                    Fight.RemoveMarkTrigger(wall.Caster, wall);
                    Walls.Remove(wall);
                }
               
            }

        }
        public void Increment()
        {
            //if (RealFighterLook.scales.Count == 0)
            //    RealFighterLook.scales.Add(100);
            //if (RealFighterLook.scales[0] >= 200)
            //    return;
            //RealFighterLook.SetScale((short)(RealFighterLook.scales[0]+30));
            //FighterLook = RealFighterLook;
            //this.Fight.TryStartSequence(this.ContextualId, 1);
            //this.Fight.Send(new GameActionFightChangeLookMessage(0, this.ContextualId, this.ContextualId, FighterLook.ToEntityLook()));
            //this.Fight.TryEndSequence(1, 0);
        }
        public void Detonate()
        {
            
            MarksHelper.Instance.CastDetonation(Master,this, BombWallRecord.DetonationSpellId, BombWallRecord.BombMonsterId,5);
            Die();
           
        }
       
    }
}
