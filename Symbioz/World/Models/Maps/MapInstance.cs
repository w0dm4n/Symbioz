using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Providers;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records;
using Symbioz.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Enums;

namespace Symbioz.World.Models
{
    public class MapInstance
    {
        public const sbyte MAX_MONSTERGROUP_PER_MAP = 4;
        public const sbyte MAX_MONSTER_PER_GROUP = 6;

        public List<FightCommonInformations> Fights = new List<FightCommonInformations>();
        public List<InteractiveRecord> Interactives = new List<InteractiveRecord>();
        public List<NpcSpawnRecord> Npcs = new List<NpcSpawnRecord>();

        internal List<WorldClient> Clients = new List<WorldClient>();
        internal List<MonsterGroup> MonstersGroups = new List<MonsterGroup>();
        public bool Muted = false;
        public MapRecord Record { get; set; }

        public MapInstance(MapRecord record)
        {
            this.Record = record;
            this.Npcs = NpcSpawnRecord.GetMapNpcs(Record.Id);
            this.Interactives = InteractiveRecord.GetInteractivesOnMap(Record.Id);
        }
        public void AddClient(WorldClient client)
        {
            this.Send(new GameRolePlayShowActorMessage(client.Character.GetRolePlayActorInformations()));

            if (!Clients.Contains(client))
                Clients.Add(client);
        }
        public void RemoveClient(WorldClient client) // Leaving Map
        {
            client.Character.Map = null;
            Clients.Remove(client);
            RemoveEntity(client.Character.Id);
        }
        void RemoveEntity(int id)
        {
            this.Send(new GameContextRemoveElementMessage(id));
        }
        public void RemoveMonsterGroup(int groupid)
        {
            MonstersGroups.Remove(MonstersGroups.Find(x => x.MonsterGroupId == groupid));
            RemoveEntity(groupid);
        }
        public void Send(Message message)
        {
            for (int i = 0; i < Clients.Count(); i++)
            {
                Clients[i].Send(message);
            }
        }
        List<GameRolePlayCharacterInformations> GetPlayers()
        {
            return Clients.ConvertAll<GameRolePlayCharacterInformations>(x => x.Character.GetRolePlayActorInformations());
        }
        public List<InteractiveElement> GetInteractiveElements()
        {
            return Interactives.ConvertAll<InteractiveElement>(x => x.GetInteractiveElement());
        }
        List<GameRolePlayNpcInformations> GetNpcsInformations()
        {
            return Npcs.ConvertAll<GameRolePlayNpcInformations>(x => x.GetGameRolePlayNpcInformations());
        }
        public List<GameRolePlayActorInformations> GetActors()
        {
            List<GameRolePlayActorInformations> actors = new List<GameRolePlayActorInformations>();
            actors.AddRange(GetMonsters());
            actors.AddRange(GetNpcsInformations());
            actors.AddRange(GetPlayers());
            return actors;
        }

        public void SyncMonsters()
        {
            lock (this)
            {
                if (MonstersGroups.Count > 0 || Record.HaveZaap || !MapNoSpawnRecord.CanSpawn(Record.Id))
                    return;
                AsyncRandom random = new AsyncRandom();
                if (this.Record.DugeonMap)
                {
                    if (this.MonstersGroups.Count == 0)
                    {
                        List<MonsterSpawnMapRecord> monsters = DungeonRecord.GetDungeonMapMonsters(this.Record.Id).ConvertAll<MonsterSpawnMapRecord>(x => new MonsterSpawnMapRecord(-1, x, this.Record.Id, 100));
                        monsters.ForEach(x => x.ActualGrade = (sbyte)random.Next(1, 6));
                        MonstersGroups.Add(new MonsterGroup(MonsterGroup.START_ID, monsters, (ushort)Record.RandomWalkableCell()));
                    }
                    return;
                }
                var spawns = MonsterSpawnSubRecord.GetSpawns(Record.Id);
                if (spawns.Count() == 0)
                    return;
                for (sbyte groupId = 0; groupId < random.Next(1, MAX_MONSTER_PER_GROUP + 1); groupId++)
                {
                    List<MonsterSpawnMapRecord> monsters = new List<MonsterSpawnMapRecord>();
                    for (int w = 0; w < random.Next(1, MAX_MONSTER_PER_GROUP + 1); w++)
                    {
                        int max = spawns.Sum((MonsterSpawnMapRecord entry) => entry.Probability);
                        int num = random.Next(0, max);
                        int num2 = 0;
                        foreach (var monster in spawns)
                        {
                            num2 += monster.Probability;
                            if (num <= num2)
                            {
                                monster.ActualGrade = (sbyte)random.Next(1, 6);
                                monsters.Add(monster);
                                break;
                            }
                        }

                    }
                    MonstersGroups.Add(new MonsterGroup(groupId + MonsterGroup.START_ID, monsters, (ushort)Record.RandomWalkableCell()));
                }
            }
        }
        public void AddFightSword(FightCommonInformations commonInfos)
        {
            Fights.Add(commonInfos);
            Send(new MapFightCountMessage((ushort)Fights.Count()));
            Send(new GameRolePlayShowChallengeMessage(commonInfos));
        }
        public void OnFighterAdded(int fightid, int teamid, FightTeamMemberInformations addedmember)
        {
            var fight = Fights.Find(x => x.fightId == fightid);
            var team = fight.fightTeams.FirstOrDefault(x => x.teamId == teamid);
            team.teamMembers.Add(addedmember);
            Send(new GameRolePlayRemoveChallengeMessage(fightid));
            Send(new GameRolePlayShowChallengeMessage(fight));
        }
        public void OnFighterRemoved(int fightid, int teamid, int fighterid)
        {
            var fight = Fights.Find(x => x.fightId == fightid);
            if (fight != null)
            {
                var team = fight.fightTeams.FirstOrDefault(x => x.teamId == teamid);
                team.teamMembers.RemoveAll(x => x.id == fighterid);
                Send(new GameRolePlayRemoveChallengeMessage(fightid));
                Send(new GameRolePlayShowChallengeMessage(fight));
            }
        }
        public void ShowFightsCount(WorldClient client)
        {
            client.Send(new MapFightCountMessage((ushort)FightProvider.Instance.m_worldFights.FindAll(x => x.Map.Id == client.Character.Map.Id).Count()));
        }
        public void RemoveFightSword(int fightid)
        {
            Fights.Remove(Fights.Find(x => x.fightId == fightid));
            Send(new MapFightCountMessage((ushort)Fights.Count()));
            Send(new GameRolePlayRemoveChallengeMessage(fightid));
        }
        public MonsterGroup GetMapMonsterGroup(int id)
        {
            return MonstersGroups.Find(x => x.MonsterGroupId == id);
        }
        List<GameRolePlayGroupMonsterInformations> GetMonsters()
        {
            return MonsterGroup.GetActorsInformations(Record, MonstersGroups);
        }
    }
}
