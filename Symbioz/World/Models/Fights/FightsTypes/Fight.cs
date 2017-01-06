using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbioz;
using System.Threading.Tasks;
using Symbioz.World.Records;
using Symbioz.Providers;
using Symbioz.Providers.FightResults;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.World.Records.Monsters;
using Symbioz.Helper;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.Network.Servers;
using Symbioz.World.Models.Challenges;
using Symbioz.World.Models.Fights.FightsTypes;
using Symbioz.World.Models.Maps;
using Symbioz.World.Models.Alliances;
using System.Drawing;
using Symbioz.World.Records.Alliances.Prisms;
using Symbioz.World.Records.SubAreas;
using Symbioz.World.Models.Alliances.Prisms;
using Symbioz.Core.Startup;
using Symbioz.World.Models.Parties;

namespace Symbioz.World.Models.Fights
{
    public abstract class Fight : IDisposable
    {
        /// <summary>
        /// Permet de synchroniser tout les combants
        /// </summary>
        public ClientsSynchronizer Synchronizer { get; set; }

        /// <summary>
        /// Variable représentant l'état du combat, terminé ou non
        /// </summary>
        public bool Ended = false;

        /// <summary>
        /// Temps de préparation autorisé avant le début du combat
        /// </summary>
        public const short FIGHT_PREPARATION_TIME = 3000;

        /// <summary>
        /// En cas de mort, le combat teleporte t-il au dernier zaap sauvegarder? ou la carte ne change pas?
        /// </summary>
        public virtual bool SpawnJoin { get; set; }

        /// <summary>
        /// S'agit t-il d'un combat PvP (pour les resistances en pvp bouclier etc)
        /// </summary>
        public virtual bool PvP { get; set; }

        /// <summary>
        /// Doit t-on ajouter a la carte l'épée pour permettre au autres joueurs de rejoindre le combat
        /// </summary>
        public abstract bool AddFightSword { get; }

        public bool DeadItemsLoaded = false;

        public List<ItemRecord> ListDeadItems = new List<ItemRecord>();

        public int ListDeadsItemsStartSize = 0;

        /// <summary>
        /// Type de combat
        /// </summary>
        public virtual FightTypeEnum FightType { get; set; }

        public TimeLine TimeLine = new TimeLine();

        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public MapRecord Map { get; set; }

        public DateTime CreationTime { get; set; }

        public short FightCellId { get; set; }

        public FightTeam BlueTeam { get; set; }

        public FightTeam RedTeam { get; set; }

        public ChallengesInstance ChallengesInstance { get; set; }

        public bool Started = false;

        public List<string> PlayersIP = new List<string>();


        /// <summary>
        /// Todo
        /// </summary>
        public bool WaitAcknowledgment { get; set; }

        public int AllianceId { get; set; }

        public virtual short FightPreparationTime
        {
            get
            {
                return 30000;
            }
        }

        public UIDProvider MarkIdProvider = new UIDProvider();

        public List<MarkTrigger> Marks = new List<MarkTrigger>();

        List<Sequence> m_sequences = new List<Sequence>();

        public List<MarkInteraction> MarkInteractions = new List<MarkInteraction>();


        public Fight(int id, MapRecord map, FightTeam blueteam, FightTeam redteam, short fightcellid, bool customFightPreparation = false)
        {
            this.Id = id;
            this.BlueTeam = blueteam;
            this.RedTeam = redteam;
            this.BlueTeam.Fight = this;
            this.RedTeam.Fight = this;
            this.Map = map;
            this.FightCellId = fightcellid;
            this.TimeLine.OnNewRound += TimeLine_OnNewRound;
            this.Synchronizer = new ClientsSynchronizer(this);
        }

        void TimeLine_OnNewRound(uint round)
        {
            Send(new GameFightNewRoundMessage(round));
        }
        public virtual void StartPlacement()
        {
            GetAllFighters().ForEach(x => x.ShowFighter());
            if (AddFightSword)
                Map.Instance.AddFightSword(GetFightCommonInformations());
            ShowPlacementCells();
        }
        public void UpdateTeam(FightTeam team)
        {
            Send(new GameFightUpdateTeamMessage((short)Id, team.GetFightTeamInformations()));
        }


        public IEnumerable<MarkTrigger> GetTriggers()
        {
            return Marks;
        }

        public void SetOption(FightOptionsEnum option, FightTeam team)
        {
            bool setted = false;
            switch (option)
            {
                case FightOptionsEnum.FIGHT_OPTION_SET_SECRET:
                    team.TeamOptions.isSecret = !team.TeamOptions.isSecret;
                    setted = team.TeamOptions.isSecret;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY:
                    team.TeamOptions.isRestrictedToPartyOnly = !team.TeamOptions.isRestrictedToPartyOnly;
                    setted = team.TeamOptions.isRestrictedToPartyOnly;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_SET_CLOSED:
                    team.TeamOptions.isClosed = !team.TeamOptions.isClosed;
                    setted = team.TeamOptions.isClosed;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP:
                    team.TeamOptions.isAskingForHelp = !team.TeamOptions.isAskingForHelp;
                    setted = team.TeamOptions.isAskingForHelp;
                    break;
            }
            GameFightOptionStateUpdateMessage message = new GameFightOptionStateUpdateMessage((short)Id, team.Id, (sbyte)option, setted);
            Map.Instance.Send(message);
            this.Send(message);
        }
        public virtual void StartFight()
        {
            try
            {
                if (this.Started == false)
                {
                    Map.Instance.RemoveFightSword(Id);
                    this.StartTime = DateTime.Now;
                    Started = true;
                    Send(new GameFightStartMessage(new Idol[0]));
                    Send(new GameFightTurnListMessage(TimeLine.GenerateTimeLine(), new int[0]));
                    if (this is FightPvM)
                        ChallengesInstance = new ChallengesInstance(this);
                    SyncFighters();
                    TimeLine.StartFight();
                    TimeLine.GetFirstFighter().StartTurn();
                }
            }
            catch (Exception error)
            {
                Logger.Error(error);
            }
        }

        public void FighterDisconnect(CharacterFighter f)
        {
            if (f.IsPlaying)
            {
                //f.EndTurn();
            }
            f.FirstRoundDisconnected = true;
            f.Disconnected = true;
        }

        public void FighterReconnect(CharacterFighter f)
        {
            f.Disconnected = false;
            var fight = GetFightCommonInformations();
            Send(new GameRolePlayRemoveChallengeMessage(Id));
            Send(new GameRolePlayShowChallengeMessage(fight));
            GetAllFighters().ForEach(x => x.ShowFighter(f.Client));
            String[] name = new string[1];
            name[0] = f.Client.Character.Record.Name;
            Send(new TextInformationMessage(1, 184, name));
        }
        public void Fighterdeleted(CharacterFighter f)
        {
            var fight = GetFightCommonInformations();
            var team = fight.fightTeams.FirstOrDefault(x => x.teamId == f.Team.Id);
            team.teamMembers.RemoveAll(x => x.id == f.ContextualId);
            Send(new GameRolePlayRemoveChallengeMessage(Id));
            Send(new GameRolePlayShowChallengeMessage(fight));
        }

        public void Reply(string message)
        {
            OnCharacterFighters(x => x.Character.Reply(message));
        }
        public void SyncFighters()
        {
            GetAllFighters().ForEach(x => x.RefreshStats());
            Send(new GameFightSynchronizeMessage(GetAllFighters().ConvertAll<GameFightFighterInformations>(x => x.FighterInformations)));
        }

        #region Placement
        public abstract FightCommonInformations GetFightCommonInformations();

        public abstract void TryJoin(WorldClient client, int mainfighterid);

        public bool CanJoin(WorldClient client, FightTeam team, Fighter mainFighter = null)
        {
            if (!IsPlacementCellFree(team.TeamColor) || team.FightersCount >= 8)
            {
                client.Character.Reply("L'équipe est pleine.");
                return false;
            }
            if (team.TeamOptions.isClosed)
            {
                client.Character.Reply("L'équipe est fermé");
                return false;
            }
            if (team.TeamOptions.isRestrictedToPartyOnly)
            {
                if (client.Character.Party == null)
                {
                    client.Character.Reply("Ce combat est reserver aux membres du groupe");
                    return false;
                }
                if (mainFighter != null)
                {
                    Party p = WorldServer.Instance.WorldClients.Find(x => x.Character.Id == mainFighter.ContextualId).Character.Party;
                    if (p.Id != client.Character.Party.Id)
                    {
                        client.Character.Reply("Ce combat est réservé aux membres du groupe");
                        return false;
                    }
                }
            }
            if (this is FightAgression)
            {
                if (!AvAState.CanJoinFight(mainFighter, client))
                {
                    client.Character.Reply("Vous devez être dans la même alliance que ce personnage pour rejoindre ce combat !");
                    return false;
                }
                else
                {
                    if ((this.BlueTeam.GetFighters(true).Count == 1 && this.RedTeam.GetFighters(true).Count >= 5)
                        || (this.RedTeam.GetFighters(true).Count == 1 && this.BlueTeam.GetFighters(true).Count >= 5))
                    {
                        client.Character.Reply("Impossible de rejoindre cette agression ! (5v1 max)");
                        return false;
                    }
                    if (!(this.PlayersIP.Contains(client.SSyncClient.OnlyIp)))
                    {
                        this.PlayersIP.Add(client.SSyncClient.OnlyIp);
                        return true;
                    }
                    else
                    {
                        client.Character.Reply("Impossible de rejoindre une agression avec plus d'un personnage !");
                        return false;
                    }
                }
            }
            return true;
        }
        public void ShowPlacementCells()
        {
            GetAllCharacterFighters().ForEach(x => x.ShowPlacementCells());
        }
        public short GetPlacementCell(TeamColorEnum team)
        {
            List<short> usedplacement;

            switch (team)
            {
                case TeamColorEnum.BLUE_TEAM:
                    usedplacement = BlueTeam.GetFighters().ConvertAll<short>(x => x.CellId);
                    return Map.BlueCells.Find(x => !usedplacement.Contains(x));
                case TeamColorEnum.RED_TEAM:
                    usedplacement = RedTeam.GetFighters().ConvertAll<short>(x => x.CellId);
                    return Map.RedCells.Find(x => !usedplacement.Contains(x));
            }
            return 0;
        }
        public bool IsPlacementCellFree(TeamColorEnum team)
        {
            List<short> usedplacement;

            switch (team)
            {
                case TeamColorEnum.BLUE_TEAM:
                    {
                        usedplacement = BlueTeam.GetFighters().ConvertAll<short>(x => x.CellId);
                        return Map.BlueCells.FindAll(x => !usedplacement.Contains(x)).Count != 0;
                    }
                case TeamColorEnum.RED_TEAM:
                    usedplacement = RedTeam.GetFighters().ConvertAll<short>(x => x.CellId);
                    return Map.RedCells.FindAll(x => !usedplacement.Contains(x)).Count != 0;
            }
            return false;
        }

        public int PopNextNonPlayerId()
        {
            List<int> ids = GetAllFighters().ConvertAll<int>(x => x.ContextualId);
            ids = ids.ConvertAll<int>(x => -Math.Abs(x));
            ids.Sort();
            ids.Reverse();
            if (ids.Count == 0)
                return -1;
            return ids.Last() - 1;

        }
        #endregion
        #region ManageMethods
        /*public List<short> BreakAtFirstObstacles(List<short> cells)
        {
 
            List<short> results = new List<short>();
            if (cells == null)
                return results;
            foreach (var cell in cells)
            {
                if (!IsObstacle(cell))
                {
                    results.Add(cell);
                }
                else
                {
                    // PushDamages
                    break;
                }
            }
            return results;
        }*/
        public List<short> BreakAtFirstObstacles(short endcell, List<short> cells, DirectionsEnum direction)
        {
            try
            {
                List<short> results = new List<short>();
                if (cells == null)
                    return results;
                foreach (var cell in cells)
                {
                    if (!IsObstacle(cell) && !IsOutMap(endcell, cell, direction))
                    {
                        results.Add(cell);
                    }
                    else
                    {
                        break;
                    }
                    endcell = cell;
                }
                return results;
            }
            catch (Exception error)
            {
                Logger.Error(error);
                return null;
            }
        }

        public List<Fighter> GetFighterBreakAtFirstObstacles(short endcell, List<short> cells, DirectionsEnum direction)
        {
            try
            {
                List<Fighter> results = new List<Fighter>();
                if (cells == null)
                    return null;
                bool colled = false;
                foreach (var cell in cells)
                {
                    if (GetAllFighters().ConvertAll<short>(x => x.CellId).Contains(cell) && !IsOutMap(endcell, cell, direction))
                    {
                        colled = true;
                        foreach (var f in GetAllFighters())
                        {
                            if (f.CellId == cell)
                                results.Add(f);
                        }
                    }
                    else
                    {
                        if (colled)
                            break;
                    }
                    endcell = cell;
                }
                return results;
            }
            catch (Exception error)
            {
                Logger.Error(error);
                return null;
            }
        }

        public bool IsOutMap(short endcellid, short cellid, DirectionsEnum dir)
        {
            switch (dir)
            {
                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    if (endcellid >= 0 && endcellid <= 13)//line top
                        return (true);
                    if (IsrightMap(endcellid))//line right
                        return (true);
                    break;
                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    if (endcellid >= 0 && endcellid <= 13)//line top
                        return (true);
                    if (IsleftMap(endcellid))//line left
                        return (true);
                    break;
                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    if (endcellid >= 546 && endcellid <= 559)
                        return (true);
                    if (IsrightMap(endcellid))//line right
                        return (true);
                    break;
                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    if (endcellid >= 546 && endcellid <= 559)
                        return (true);
                    if (IsleftMap(endcellid))//line left
                        return (true);
                    break;
            }
            return (false);
        }

        public bool IsrightMap(short cell)
        {
            switch (cell)
            {
                case 27:
                case 55:
                case 83:
                case 111:
                case 139:
                case 167:
                case 195:
                case 223:
                case 251:
                case 279:
                case 307:
                case 335:
                case 363:
                case 391:
                case 419:
                case 447:
                case 475:
                case 503:
                case 531:
                case 559:
                    return (true);
            }
            return (false);
        }

        public bool IsleftMap(short cell)
        {
            switch (cell)
            {
                case 0:
                case 28:
                case 56:
                case 84:
                case 112:
                case 140:
                case 168:
                case 196:
                case 224:
                case 252:
                case 280:
                case 308:
                case 336:
                case 364:
                case 392:
                case 420:
                case 448:
                case 476:
                case 504:
                case 532:
                    return (true);
            }
            return (false);
        }
        public bool IsObstacle(short cellid)
        {
            if (!Map.WalkableCells.Contains(cellid))
                return true;
            if (GetAllFighters().ConvertAll<short>(x => x.CellId).Contains(cellid))
                return true;
            return false;
        }
        public T GetFighter<T>(Predicate<T> predicate) where T : Fighter
        {
            return GetAllFighters().OfType<T>().ToList().Find(predicate);
        }
        public List<T> GetFighters<T>(bool deads = false) where T : Fighter
        {
            return GetAllFighters(deads).OfType<T>().ToList();
        }
        public bool ContainCharacterFighter(int characterid)
        {
            var eventualFighter = GetAllFighters(true).Find(x => x.ContextualId == characterid);
            if (eventualFighter != null && eventualFighter is CharacterFighter)
                return true;
            else
                return false;
        }
        public List<Fighter> GetAllSummons()
        {
            return GetAllFighters().FindAll(x => x.FighterStats.Summoned);
        }
        public List<Fighter> GetAllFighters(bool deaths = false)
        {

            List<Fighter> result = new List<Fighter>();
            if (BlueTeam != null && RedTeam != null)
            {
                result.AddRange(BlueTeam.GetFighters(deaths));
                result.AddRange(RedTeam.GetFighters(deaths));
            }
            return result;
        }
        public T GetFirstBuff<T>(Predicate<T> predicate) where T : Buff
        {
            List<Buff> results = new List<Buff>();
            GetAllFighters().ForEach(x => results.AddRange(x.Buffs));
            var list = results.OfType<T>().ToList();
            return list.Find(predicate);
        }
        public List<T> GetAllBuffs<T>(Predicate<T> predicate) where T : Buff
        {
            List<Buff> results = new List<Buff>();
            GetAllFighters().ForEach(x => results.AddRange(x.Buffs));
            var list = results.OfType<T>().ToList();
            return list.FindAll(predicate);
        }

        public List<Buff> GetAllBuffs()
        {
            List<Buff> result = new List<Buff>();
            GetAllFighters().ForEach(x => result.AddRange(x.Buffs));
            return result;
        }

        public void RemoveAllBuffs<T>(Predicate<T> predicate) where T : Buff
        {
            foreach (var fighter in GetAllFighters())
            {
                var buffs = fighter.Buffs.OfType<T>().ToList().FindAll(predicate);
                foreach (var buff in buffs)
                {
                    buff.RemoveBuff();
                    fighter.Buffs.Remove(buff);
                }
            }
        }
        public Fighter GetFighter(short cellid)
        {
            return GetAllFighters().Find(x => x.CellId == cellid);
        }
        public Fighter GetFighter(int contextualid)
        {
            return GetAllFighters().Find(x => x.ContextualId == contextualid);
        }
        public List<CharacterFighter> GetAllCharacterFighters(bool withdeaths = false)
        {
            return GetAllFighters(withdeaths).FindAll(x => x is CharacterFighter).ConvertAll<CharacterFighter>(x => (CharacterFighter)x);
        }

        public void Send(Message message)
        {
            GetAllCharacterFighters(true).FindAll(x => !x.HasLeft).ForEach(x => x.Client.Send(message));
        }
        public void OnCharacterFighters(Action<WorldClient> action)
        {
            var f = GetAllCharacterFighters(true);
            f.FindAll(x => !x.HasLeft).ConvertAll<WorldClient>(x => x.Client).ForEach(action);
        }
        public void OnFighters(Action<Fighter> action)
        {
            GetAllFighters().ForEach(action);
        }
        public void SpawnBomb(Fighter master, short templateid, sbyte grade, short cellid, FightTeam team)
        {
            BombFighter bomb = new BombFighter(master, team, MonsterRecord.GetMonster((ushort)templateid), cellid, grade);
            Send(new GameActionFightSummonMessage(181, bomb.ContextualId, bomb.FighterInformations));
            bomb.CheckWalls();
        }
        public MonsterFighter AddSummon(Fighter master, short monsterid, sbyte grade, short cellid, FightTeam team)
        {
            MonsterFighter summoned = new MonsterFighter(MonsterRecord.GetMonster((ushort)monsterid),
              team, grade, cellid, master.ContextualId);
            if (summoned.Template.UseSummonSlot && !summoned.Template.UseBombSlot)
                TimeLine.Insert(summoned, master);
            Send(new GameActionFightSummonMessage(181, summoned.ContextualId, summoned.FighterInformations));
            Send(new GameFightTurnListMessage(TimeLine.GenerateTimeLine(false), new int[0]));
            return summoned;

        }
        public List<T> GetMarks<T>() where T : MarkTrigger
        {
            return Marks.OfType<T>().ToList();
        }
        public List<T> GetMarks<T>(Predicate<T> predicate) where T : MarkTrigger
        {
            return Marks.OfType<T>().ToList().FindAll(predicate);
        }
        public void AddMarkTrigger(Fighter fighter, MarkTrigger mark, FightTeam team = null)
        {
            var message = new GameActionFightMarkCellsMessage(0, fighter.ContextualId, mark.GetMark());
            if (team == null)
                Send(message);
            else
                team.Send(message);
            mark.Intitialize(this, mark.GetType());
            Marks.Add(mark);
        }
        public void RemoveMarkTrigger(Fighter master, MarkTrigger mark)
        {
            TryStartSequence(master.ContextualId, 3);
            Marks.Remove(mark);
            MarkInteractions.RemoveAll(x => x.Mark == mark);
            Send(new GameActionFightUnmarkCellsMessage(310, master.ContextualId, mark.Id));
            TryEndSequence(3, 0);
        }
        public void Acknowledge()
        {
            this.WaitAcknowledgment = false;
        }
        public void EndFight()
        {
            TeamColorEnum winner = GetWinner();
            var results = GetFightResults(winner);
            OnFightEnded(winner);
            foreach (var fighter in GetAllCharacterFighters(true).FindAll(x => !x.HasLeft))
            {
                if (Started)
                    ShowFightResults(results, fighter.Client);
            }
            Dispose();
        }

        public virtual void OnFightEnded(TeamColorEnum winner)
        {

        }

        public bool CheckFightEnd()
        {
            try
            {
                if (Ended)
                    return true;
                if (BlueTeam.GetFighters().Alives().Count <= 0 || RedTeam.GetFighters().Alives().Count <= 0 || this.TimeLine.m_fighters.Count <= 0)
                {
                    if (Started)
                        Synchronizer.Start(EndFight);
                    else
                        EndFight();
                    Ended = true;
                    return true;
                }
                return false;
            }
            catch (Exception error)
            {
                Logger.Error(error);
                return false;
            }

        }
        public void TryEndSequence(sbyte sequenceType, ushort actionId)
        {
            var sequence = m_sequences.Find(x => x.SequenceType == sequenceType);
            if (sequence != null)
            {
                sequence.End(this, actionId);
                this.m_sequences.Remove(sequence);
                this.WaitAcknowledgment = true;
            }
        }
        public void TryStartSequence(int sourceid, sbyte sequencetype)
        {
            var sequence = m_sequences.Find(x => x.SequenceType == sequencetype);
            if (sequence == null)
            {
                var newSequence = new Sequence(this, sourceid, sequencetype);
                newSequence.Start(this);
                m_sequences.Add(newSequence);
            }
        }
        public TeamColorEnum GetWinner()
        {
            List<CharacterFighter> bluePlayers = BlueTeam.GetCharacterFighters(true);
            if (bluePlayers.Count > 0 && bluePlayers.All(x => x.HasLeft))
                return TeamColorEnum.RED_TEAM;

            List<CharacterFighter> redPlayers = RedTeam.GetCharacterFighters(true);
            if (redPlayers.Count > 0 && redPlayers.All(x => x.HasLeft))
                return TeamColorEnum.BLUE_TEAM;

            if (BlueTeam.GetFighters().Alives().Count == 0)
            {
                return TeamColorEnum.RED_TEAM;
            }
            else if (RedTeam.GetFighters().Alives().Count == 0)
            {
                return TeamColorEnum.BLUE_TEAM;
            }
            Logger.Error("Try to define winner but fight is running...");
            return 0;

        }
        #endregion
        public void Dispose()
        {
            Singleton<Startup>.Instance.IOTask.AddMessage(new Action(() => {
                try
                {
                    var charactersFighting = GetAllCharacterFighters(true).FindAll(x => !x.HasLeft);
                    foreach (var fighter in charactersFighting)
                    {
                        bool winner = GetWinner() == fighter.Team.TeamColor;
                        if (fighter.Fight.FightType == FightTypeEnum.FIGHT_TYPE_PvM || fighter.Fight.FightType == FightTypeEnum.FIGHT_TYPE_AGRESSION)
                        {
                            if (fighter.Client.Character.GetLifePoints == true)
                            {
                                fighter.Client.Character.CurrentStats.LifePoints = (uint)fighter.Client.Character.CharacterStatsRecord.LifePoints;
                                fighter.Client.Character.Record.CurrentLifePoint = fighter.Client.Character.CurrentStats.LifePoints;
                                fighter.Client.Character.GetLifePoints = false;
                            }
                            else
                            {
                                fighter.Client.Character.CurrentStats.LifePoints = (uint)fighter.FighterStats.Stats.LifePoints;
                                fighter.Client.Character.Record.CurrentLifePoint = fighter.Client.Character.CurrentStats.LifePoints;
                            }
                        }
                        fighter.Client.Character.RejoinMap(SpawnJoin, winner);
                    }
                    FightProvider.Instance.RemoveFight(this);
                    this.BlueTeam.Dispose();
                    this.RedTeam.Dispose();
                    this.RedTeam = null;
                    this.BlueTeam = null;
                    this.Synchronizer = null;
                    this.ChallengesInstance = null;
                }
                catch (Exception error)
                {
                    Logger.Error(error);
                }
            }));
        }
        public void NewTurn()
        {
            if (!this.CheckFightEnd())
            {
                Fighter newFighter = TimeLine.PopNextFighter();
                newFighter.StartTurn();
            }
        }
        public abstract void ShowFightResults(List<FightResultListEntry> results, WorldClient client);


        public void GetRewards(DungeonsRewardRecord Rewards, int CharacterId, int mapId)
        {
            int KamasWon = 0;
            WorldClient client = null;
            List<string> itemsWon = new List<string>();
            if (Rewards != null)
            {
                KamasWon = Rewards.gain_kamas;
                client = WorldServer.Instance.GetOnlineClient(CharacterId);
                if (client != null)
                {
                    var itemsList = DungeonsRewardRecord.itemToAdd(mapId);
                    foreach (var item in itemsList)
                    {
                        client.Character.Inventory.Add(ushort.Parse(item.Id.ToString()), 1, false);
                        itemsWon.Add(item.Name);
                    }
                }
            }
            string toSend = "Pour vous récompenser de votre bravoure, nous vous offrons : <br/>";
            foreach (var itemWon in itemsWon)
                toSend += "- " + itemWon + "<br/>";
            toSend += "<b>" + KamasWon + "</b> Kamas sur votre personnage";
            if (client != null)
            {
                client.Character.ShowNotification(toSend);
                if (KamasWon > 0)
                    client.Character.AddKamas(KamasWon);
            }
        }

        public List<FightResultListEntry> GetFightResults(TeamColorEnum winner)
        {
            if (this is FightAvAPrism)
            {
                if (winner == TeamColorEnum.RED_TEAM)
                {
                    var AlliancePlayers = AllianceProvider.GetClients(this.AllianceId);
                    foreach (var tmp in AlliancePlayers)
                        tmp.Character.Reply("Votre prisme en  (<b>" + this.Map.WorldX + "," + this.Map.WorldY + "</b>, " + SubAreaRecord.GetSubAreaName(this.Map.SubAreaId) + "), à survécu !", Color.Orange);
                    var prism = PrismRecord.GetPrismByMapId(this.Map.Id);
                    if (prism != null)
                        prism.ParsedState = PrismStateEnum.PRISM_STATE_NORMAL;
                }
                else
                {
                    var AlliancePlayers = AllianceProvider.GetClients(this.AllianceId);
                    foreach (var tmp in AlliancePlayers)
                        tmp.Character.Reply("Votre prisme en  (<b>" + this.Map.WorldX + "," + this.Map.WorldY + "</b>, " + SubAreaRecord.GetSubAreaName(this.Map.SubAreaId) + "), n'a pas survécu !", Color.Orange);
                    var prism = PrismRecord.GetPrismByMapId(this.Map.Id);
                    if (prism != null)
                        this.Map.Instance.DeletePrism(prism);
                }
            }
            List<FightResultListEntry> results = new List<FightResultListEntry>();
            ulong xpwinnable = 0;

            var fighters = GetAllFighters(true);

            foreach (Fighter fighter in fighters)
            {
                try
                {
                    if (fighter.Team.TeamColor != winner && fighter is CharacterFighter && fighter.Dead)
                        xpwinnable += ((fighter as CharacterFighter).Client.Character.Record.Exp / 4);
                }
                catch (Exception error)
                {
                    Logger.Error(error);
                }
            }

            if (xpwinnable > 0)
                xpwinnable = xpwinnable / (ulong)(winner == TeamColorEnum.RED_TEAM ? this.RedTeam.FightersCount : this.BlueTeam.FightersCount);

            foreach (Fighter fighter in fighters)
            {
                try
                {
                    if (fighter is CharacterFighter)
                    {
                        if (fighter.Team.TeamColor == winner)
                            this.GetRewards(DungeonsRewardRecord.RewardOnCurrentMap(fighter.Fight.Map.Id), fighter.FighterStats.RealStats.CharacterId, fighter.Fight.Map.Id);
                        results.Add(new FightResultPlayer(fighter as CharacterFighter, winner, (xpwinnable == 0 ? 0 : (xpwinnable / (ulong)fighter.Team.FightersCount))).GetEntry());
                    }
                    if (fighter is MonsterFighter)
                    {
                        if (!fighter.FighterStats.Summoned)
                            results.Add(new FightResultMonster(fighter as MonsterFighter, winner).GetEntry());
                    }
                    if (fighter is PrismFighter)
                    {
                        FightOutcomeEnum OutCome = (fighter.Team.TeamColor == winner) ? FightOutcomeEnum.RESULT_VICTORY : FightOutcomeEnum.RESULT_LOST;
                        if (!fighter.FighterStats.Summoned)
                            results.Add(new FightResultFighterListEntry((ushort)OutCome, 0, new FightLoot(new List<ushort>(), 0), fighter.ContextualId, !fighter.Dead));
                    }
                }
                catch (Exception error)
                {
                    Logger.Error(error);
                }
            }
            return results;
        }

        public List<FightResultListEntry> GetFightResultsForLeaver(TeamColorEnum winner)
        {
            List<FightResultListEntry> results = new List<FightResultListEntry>();
            var fighters = GetAllFighters(true);

            ulong xpwinnable = 0;

            foreach (Fighter fighter in fighters)
            {
                try
                {
                    if (fighter.Team.TeamColor != winner && fighter is CharacterFighter && fighter.Dead)
                        xpwinnable += ((fighter as CharacterFighter).Client.Character.Record.Exp / 4);
                }
                catch (Exception error)
                {
                    Logger.Error(error);
                }
            }

            if (xpwinnable > 0)
                xpwinnable = xpwinnable / (ulong)(winner == TeamColorEnum.RED_TEAM ? this.RedTeam.FightersCount : this.BlueTeam.FightersCount);

            foreach (Fighter fighter in fighters)
            {
                try
                {
                    if (fighter is CharacterFighter)
                    {
                        results.Add(new FightResultPlayer(fighter as CharacterFighter, winner, (xpwinnable == 0 ? 0 : (xpwinnable / (ulong)fighter.Team.FightersCount)), true).GetEntryForLeaver());
                    }
                    if (fighter is MonsterFighter)
                    {
                        if (!fighter.FighterStats.Summoned)
                            results.Add(new FightResultMonster(fighter as MonsterFighter, winner).GetEntry());
                    }
                    if (fighter is PrismFighter)
                    {
                        FightOutcomeEnum OutCome = (fighter.Team.TeamColor == winner) ? FightOutcomeEnum.RESULT_VICTORY : FightOutcomeEnum.RESULT_LOST;
                        if (!fighter.FighterStats.Summoned)
                            results.Add(new FightResultFighterListEntry((ushort)OutCome, 0, new FightLoot(new List<ushort>(), 0), fighter.ContextualId, !fighter.Dead));
                    }
                }
                catch (Exception error)
                {
                    Logger.Error(error);
                }
            }
            return results;
        }

        public int GetFightDuration()
        {
            return (!this.Started) ? 0 : ((int)(DateTime.Now - this.StartTime).TotalMilliseconds);
        }

        public virtual double GetPlacementTimeLeft(Fighter fighter = null)
        {
            double timeLeft = this.FightPreparationTime - (DateTime.Now - this.CreationTime).TotalMilliseconds;

            if (timeLeft < 0)
                timeLeft = 0;

            return timeLeft / 100;
        }

        #region FightDeadCharacterItemsLoot

        public List<CharacterItemRecord> GetAllItems(int characterId)
        {
            List<CharacterItemRecord> allItems = new List<CharacterItemRecord>();
            foreach (var record in CharacterItemRecord.CharactersItems)
            {
                if (record.CharacterId == characterId)
                    allItems.Add(record);
            }
            return allItems;
        }

        public void LoadDeadItems(List<CharacterFighter> losers)
        {
            foreach (var loser in losers)
            {
                var character = CharacterRecord.GetCharacterRecordById(loser.FighterStats.Stats.CharacterId);
                var items = this.GetAllItems(character.Id);
                foreach (var item in items)
                {
                    ItemRecord template = ItemRecord.GetItem(item.GID);
                    this.ListDeadItems.Add(template);
                }
            }
            this.DeadItemsLoaded = true;
            this.ListDeadsItemsStartSize = this.ListDeadItems.Count;
        }

        public List<ItemRecord> GetItemRecordDropped(int[] droppeds)
        {
            List<ItemRecord> listDropped = new List<ItemRecord>();
            var index = 0;
            foreach (var dropped in droppeds)
            {
                index = 0;
                foreach (var item in this.ListDeadItems)
                {
                    if (index == dropped)
                    {
                        listDropped.Add(item);
                        break;
                    }
                    index++;
                }
            }
            return listDropped;
        }

        public void DeleteFromDeadItems(int itemId)
        {
            foreach (var item in this.ListDeadItems)
            {
                if (item != null)
                {
                    if (item.Id == itemId)
                    {
                        this.ListDeadItems.Remove(item);
                        break;
                    }
                }
            }
        }

        #endregion
    }
}