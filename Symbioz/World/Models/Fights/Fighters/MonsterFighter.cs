using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Providers.ActorIA;
using Symbioz.World.Records;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public class MonsterFighter : Fighter
    {
        public MonsterBrain Brain { get; set; }
        public MonsterRecord Template { get; set; }
        public MonsterSpawnMapRecord SpawnRecord { get; set; }
        public List<GameFightSpellCooldown> Cooldowns = new List<GameFightSpellCooldown>();
        public MonsterFighter(MonsterSpawnMapRecord spawn, FightTeam team)
            : base(team)
        {
            this.SpawnRecord = spawn;
            this.Template = MonsterRecord.GetMonster(SpawnRecord.MonsterId);
            this.ReadyToFight = true;
            this.Brain = new MonsterBrain(this, Template.IAActions);
        }
        /// <summary>
        /// For summons
        /// </summary>
        /// <param name="template"></param>
        /// <param name="team"></param>
        public MonsterFighter(MonsterRecord template, FightTeam team, sbyte grade, short cellid, int summonerid)
            : base(team)
        {
            this.Template = template;
            this.ReadyToFight = true;
            this.Brain = new MonsterBrain(this, Template.IAActions);
            this.SpawnRecord = new MonsterSpawnMapRecord(0, template.Id, -1, 0);
            this.SpawnRecord.ActualGrade = grade;
            this.Team = team;
            this.Team.AddSummon(this);
            InitializeSummon(summonerid, cellid);
        }
        public void InitializeSummon(int summonerid, short cellid)
        {
            this.Direction = 3;
            this.CellId = cellid;
            this.ContextualId = Fight.PopNextNonPlayerId();
            this.FighterLook = Template.RealLook.CloneContextActorLook();
            this.RealFighterLook = FighterLook.CloneContextActorLook();
            this.FighterStats = Template.GetFighterStats(SpawnRecord.ActualGrade, true, summonerid);
            this.FighterInformations = new GameFightMonsterInformations(ContextualId, FighterLook,
              new EntityDispositionInformations(CellId, Direction),
              (sbyte)Team.TeamColor, 0, true, FighterStats.GetMinimalStats(), new ushort[0], SpawnRecord.MonsterId, SpawnRecord.ActualGrade);

            foreach (var spell in Template.Spells)
            {
                var level = GetSpellLevel(spell);
                if (level.InitialCooldown > 0)
                    AddCooldownOnSpell(level.SpellId, (sbyte)level.InitialCooldown);
            }
        }
        public void AddCooldownOnSpell(int spellid, sbyte cooldown)
        {
            Cooldowns.Add(new GameFightSpellCooldown(spellid, cooldown));
        }
        public bool HaveCooldown(short spellid)
        {
            return Cooldowns.Find(x => x.spellId == spellid) != null;
        }
        public override void OnSpellCasted(SpellLevelRecord spelllevel)
        {
            AddCooldownOnSpell(spelllevel.SpellId, (sbyte)spelllevel.GlobalCooldown);
        }
        public void DecrementCooldowns()
        {
            foreach (var spellcd in Cooldowns)
            {
                spellcd.cooldown--;
            }
            Cooldowns.RemoveAll(x => x.cooldown <= 0);
        }
        public override void Initialize()
        {
            base.Initialize();
            ContextualId = Fight.PopNextNonPlayerId();
            FighterLook = Template.RealLook.CloneContextActorLook();
            RealFighterLook = FighterLook.CloneContextActorLook();
            FighterStats = Template.GetFighterStats(SpawnRecord.ActualGrade);
            FighterInformations = new GameFightMonsterInformations(ContextualId, FighterLook,
                new EntityDispositionInformations(CellId, Direction),
                (sbyte)Team.TeamColor, 0, true, FighterStats.GetMinimalStats(), new ushort[0], SpawnRecord.MonsterId, SpawnRecord.ActualGrade);
        }
        public override void RefreshStats() // Previous pros?
        {
            this.FighterInformations = new GameFightMonsterInformations(ContextualId, FighterLook,
                new EntityDispositionInformations(CellId, Direction), (sbyte)Team.TeamColor,
                0, !Dead, FighterStats.GetMinimalStats(), new ushort[0], Template.Id, SpawnRecord.ActualGrade);
        }
        public override FightTeamMemberInformations GetFightMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(ContextualId, Template.Id, SpawnRecord.ActualGrade);
        }
        public override int GetInitiative()
        {
            return Template.GetGrade(SpawnRecord.ActualGrade).Level;
        }
        public override void StartTurn()
        {
            this.DecrementCooldowns();
            base.StartTurn();
            Brain.StartPlay();
            base.EndTurn();


        }
        public List<SpellRecord> GetSpellsByCategory(SpellCategoryEnum category)
        {
            return Template.Spells.ConvertAll<SpellRecord>(x => SpellRecord.GetSpell(x)).FindAll(x => x.Category == category);
        }
        public override SpellLevelRecord GetSpellLevel(ushort spellid)
        {
            return SpellLevelRecord.GetLevel(spellid, SpawnRecord.ActualGrade);
        }
        public override string GetName()
        {
            return Template.Name;
        }

    }
}