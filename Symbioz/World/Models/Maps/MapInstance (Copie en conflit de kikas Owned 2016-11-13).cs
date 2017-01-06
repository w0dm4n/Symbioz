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
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.PathProvider;
using Symbioz.World.Records.SubAreas;
using Shader.Helper;
using Symbioz.Core;
using System.Threading;

namespace Symbioz.World.Models
{
    public class MapInstance
    {
        public const sbyte MAX_MONSTERGROUP_PER_MAP = 4;
        public const sbyte MAX_MONSTER_PER_GROUP = 6;

        public List<FightCommonInformations> Fights = new List<FightCommonInformations>();
        public List<InteractiveRecord> Interactives = new List<InteractiveRecord>();
        public List<NpcSpawnRecord> Npcs = new List<NpcSpawnRecord>();
        public PrismRecord Prism = null;

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
            Monitor.Enter(this.Clients);
            try
            {
                if (!Clients.Contains(client))
                    Clients.Add(client);
            }finally
            {
                Monitor.Exit(this.Clients);
            }
        }

        public void RemoveClient(WorldClient client) // OnLeavingMap
        {
            Monitor.Enter(this.Clients);
            try
            {
                client.Character.Map = null;
                Clients.Remove(client);
                RemoveEntity(client.Character.Id);

            }finally
            {
                Monitor.Exit(this.Clients);
            }


        }
        public void RemoveEntity(int id)
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
                if (message is GameRolePlayShowActorMessage)
                    AvAState.GetState(Clients[i], Clients[i].Character.Map.Instance.GetPlayers());
            }
        }

        public void SendMerchantAds(ChatServerMessage message)
        {
            for (int i = 0; i < Clients.Count(); i++)
            {
                if (Clients[i].Character.Restrictions.MarketMessage == false)
                {
                    Clients[i].Send(message);
                }
             }
        }

        public List<GameRolePlayCharacterInformations> GetPlayers()
        {
            List<GameRolePlayCharacterInformations> ListPlayers = new List<GameRolePlayCharacterInformations>();
            foreach (var client in Clients)
            {
                if (client != null && client.Character != null)
                {
                    if (client.Character.Record.MerchantMode == 0)
                        ListPlayers.Add(client.Character.GetRolePlayActorInformations());
                }
            }
            return ListPlayers;
        }

        List<GameRolePlayMerchantInformations> GetPlayersMerchant()
        {
            List<GameRolePlayMerchantInformations> ListPlayersMerchant = new List<GameRolePlayMerchantInformations>();

            Parallel.ForEach(Clients, client =>
            {
                if (client != null && client.Character != null && client.Character.Record.MerchantMode == 1)
                {
                    if (CharactersMerchantsRecord.GetItemsFromCharacterId((uint)client.Character.Id) != null)
                        ListPlayersMerchant.Add(client.Character.GetRolePlayMerchantInformations());
                }
            });
            return ListPlayersMerchant;
        }

        public List<InteractiveElement> GetInteractiveElements()
        {
            return Interactives.ConvertAll<InteractiveElement>(x => x.GetInteractiveElement());
        }

        public List<GameRolePlayNpcInformations> GetNpcsInformations()
        {
            return Npcs.ConvertAll<GameRolePlayNpcInformations>(x => x.GetGameRolePlayNpcInformations());
        }

        public List<GameRolePlayActorInformations> GetActors(WorldClient client)
        {
            List<GameRolePlayActorInformations> actors = new List<GameRolePlayActorInformations>();
            if (this.Record.Id != 115609089) // SHOP MAP
                actors.AddRange(GetMonsters());
            actors.AddRange(GetNpcsInformations());
            if(this.Prism != null)
            {
                actors.Add(this.Prism.GetGameRolePlayPrismInformations(client));
            }
            actors.AddRange(GetPlayers());
            actors.AddRange(GetPlayersMerchant());
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
                        MonstersGroups.Add(new MonsterGroup(MonsterGroup.START_ID, monsters, (ushort)Record.RandomWalkableCell(), Record.SubAreaId));
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
                                MonsterRecord currentMonsterRecord = MonsterRecord.GetMonster(monster.MonsterId);
                                if (currentMonsterRecord != null && currentMonsterRecord.Race == 78)
                                {
                                    var subArea = SubAreaRecord.GetSubArea(this.Record.SubAreaId);
                                    if (SubAreaRecord.RefreshArchMonsterTime(this.Record.SubAreaId) || subArea.ArchMonsterCount < ConfigurationManager.Instance.ArchMonsterBySubArea)
                                    {
                                        subArea.ArchMonsterCount++;
                                        SubAreaRecord.UpdateLastArchMonsterOnSubArea(this.Record.SubAreaId);
                                        monster.ActualGrade = (sbyte)random.Next(1, 6);
                                        monsters.Add(monster);
                                        break;
                                    }
                                    else
                                    {
                                        w--;
                                        break;
                                    }
                                }
                                else
                                {
                                    monster.ActualGrade = (sbyte)random.Next(1, 6);
                                    monsters.Add(monster);
                                    break;
                                }
                            }
                        }
                    }
                    MonstersGroups.Add(new MonsterGroup(groupId + MonsterGroup.START_ID, monsters, (ushort)Record.RandomWalkableCell(), Record.SubAreaId));
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

        #region AlliancePrims

        public void AddPrism(PrismRecord prismRecord, short playerSenderCellId = -1)
        {
            if (this.Prism == null)
            {
                this.Prism = prismRecord;
                this.RandomizePrismPosition(this.Prism, playerSenderCellId);
            }
            else
            {
                //throw new Exception("Un prisme est déjà présent sur cette carte !");
            }
        }

        public void DeletePrism(PrismRecord prism)
        {
            PrismRecord.RemovePrismFromMapid(this.Record.Id);
            this.Send(new GameContextRemoveElementMessage(-10000));
            //todo: find how to free the subArea from the alliance
            this.Prism = null;
        }

        private void RandomizePrismPosition(PrismRecord prismRecord, short playerSenderCellId = -1, short moveCellsCount = 2)
        {
            var random = new AsyncRandom();
            var map = prismRecord.Map.Record;

            short cellId = -1;
            if(playerSenderCellId != -1)
            {
                cellId = playerSenderCellId;
            }
            else
            {
                cellId = this.Record.RandomWalkableCell();
            }

            if (playerSenderCellId != -1)
            {
                List<short> cells = Pathfinding.GetCircleCells(cellId, moveCellsCount);
                cells.Remove((short)cellId);
                cells.RemoveAll(x => !map.WalkableCells.Contains(x));
                if (cells.Count != 0)
                {
                    var newCell = cells[random.Next(0, cells.Count())];
                    prismRecord.CellId = (ushort)newCell;
                }
                else
                {
                    Logger.Error("Impossible de trouver une cellule disponible pour placer le prisme '" + prismRecord.Id + "' (carte : " + this.Record.Id + ")");
                    prismRecord.CellId = cellId;
                }
            }
            else
            {
                prismRecord.CellId = cellId;
            }

            this.Clients.ForEach(x => x.Send(new GameRolePlayShowActorMessage(prismRecord.GetGameRolePlayPrismInformations(x))));
        }

        #endregion
    }
}
