using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.World.Models.Fights;
using Symbioz.World.Records;
using System;
using Symbioz;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.FightsTypes;

namespace Symbioz.Providers
{
    public class FightProvider : Singleton<FightProvider>
    {
        internal List<Fight> m_worldFights = new List<Fight>();  

        public Fight GetFight(int id)
        {
            return m_worldFights.Find(x => x.Id == id);
        }
        public int PopNextFightId()
        {
            lock (this) 
            {
                return m_worldFights.PopNextId<Fight>(x => x.Id);
            }
        }
        public FightPvM CreatePvMFight(MonsterGroup group,MapRecord map,short cellid)
        {
            FightTeam blueteam = new FightTeam(0,map.BlueCells, TeamColorEnum.BLUE_TEAM,TeamTypeEnum.TEAM_TYPE_PLAYER);
            FightTeam redteam = new FightTeam(1,map.RedCells, TeamColorEnum.RED_TEAM,TeamTypeEnum.TEAM_TYPE_MONSTER);
            var fight = new FightPvM(PopNextFightId(), map, blueteam, redteam,cellid,group);
            m_worldFights.Add(fight);
            return fight;
        }
        public FightArena CreateArenaFight(MapRecord map,ArenaGroup group)
        {
            FightTeam blueTeam = new FightTeam(0, map.BlueCells, TeamColorEnum.BLUE_TEAM, TeamTypeEnum.TEAM_TYPE_PLAYER);
            FightTeam redTeam = new FightTeam(1, map.RedCells, TeamColorEnum.RED_TEAM, TeamTypeEnum.TEAM_TYPE_PLAYER);
            FightArena fight = new FightArena(PopNextFightId(), map, blueTeam, redTeam, 350);
            m_worldFights.Add(fight);
            return fight;
        }
        public FightDual CreateDualFight(MapRecord map,short fightcellid,short secondplayercellid)
        {
            FightTeam blueteam = new FightTeam(0, map.BlueCells, TeamColorEnum.BLUE_TEAM, TeamTypeEnum.TEAM_TYPE_PLAYER);
            FightTeam redteam = new FightTeam(1, map.RedCells, TeamColorEnum.RED_TEAM, TeamTypeEnum.TEAM_TYPE_PLAYER);
            var fight = new FightDual(PopNextFightId(), map, blueteam, redteam, fightcellid,secondplayercellid);
            m_worldFights.Add(fight);
            return fight;
        }
        public void CreateAgressionFight(MapRecord record)
        {

        }
        public void RemoveFight(int id)
        {
            Fight fight = GetFight(id);
            fight.Map.Instance.RemoveFightSword(fight.Id);
            m_worldFights.Remove(fight);
        }
        public void RemoveFight(Fight fight)
        {
            fight.Map.Instance.RemoveFightSword(fight.Id);
            m_worldFights.Remove(fight);
        }
     
        

    }
}
