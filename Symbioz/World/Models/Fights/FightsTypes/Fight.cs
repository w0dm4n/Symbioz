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

        /// <summary>
        /// Type de combat
        /// </summary>
        public virtual FightTypeEnum FightType { get; set; }

        public TimeLine TimeLine = new TimeLine();

        public int Id { get; set; }

        public MapRecord Map { get; set; }

        public short FightCellId { get; set; }

        public FightTeam BlueTeam { get; set; }

        public FightTeam RedTeam { get; set; }

        public bool Started = false;

        /// <summary>
        /// Todo
        /// </summary>
        public bool WaitAcknowledgment { get; set; }

        public UIDProvider MarkIdProvider = new UIDProvider();

        public List<MarkTrigger> Marks = new List<MarkTrigger>();

        List<Sequence> m_sequences = new List<Sequence>();

        public List<MarkInteraction> MarkInteractions = new List<MarkInteraction>();


        public Fight(int id, MapRecord map, FightTeam blueteam, FightTeam redteam, short fightcellid)
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
            Map.Instance.RemoveFightSword(Id);
            Started = true;
            Send(new GameFightStartMessage(new Idol[0])); // see
            Send(new GameFightTurnListMessage(TimeLine.GenerateTimeLine(), new int[0]));
            SyncFighters();
            TimeLine.StartFight();
            TimeLine.GetFirstFighter().StartTurn();

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
            if (!IsPlacementCellFree(team.TeamColor))
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
                if (client.Character.PartyMember == null)
                {
                    client.Character.Reply("Ce combat est reserver aux membres du groupe");
                    return false;
                }
                if (mainFighter != null)
                {
                    Party p = WorldServer.Instance.WorldClients.Find(x => x.Character.Id == mainFighter.ContextualId).Character.PartyMember.Party;
                    if (p.Id != client.Character.PartyMember.Party.Id)
                    {
                        client.Character.Reply("Ce combat est reserver aux membres du groupe");
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
        public List<short> BreakAtFirstObstacles(List<short> cells)
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
        public void SpawnBomb(Fighter master,short templateid,sbyte grade,short cellid,FightTeam team)
        {
            BombFighter bomb = new BombFighter(master,team,MonsterRecord.GetMonster((ushort)templateid),cellid,grade);
            Send(new GameActionFightSummonMessage(181,bomb.ContextualId,bomb.FighterInformations));
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
        public void AddMarkTrigger(Fighter fighter, MarkTrigger mark,FightTeam team = null)
        {
            var message=  new GameActionFightMarkCellsMessage(0, fighter.ContextualId, mark.GetMark());
            if (team == null)
                Send(message);
            else
                team.Send(message);
            mark.Intitialize(this, mark.GetType());
            Marks.Add(mark);
        }
        public void RemoveMarkTrigger(Fighter master, MarkTrigger mark)
        {
            TryStartSequence(master.ContextualId,3);
            Marks.Remove(mark);
            MarkInteractions.RemoveAll(x => x.Mark == mark);
            Send(new GameActionFightUnmarkCellsMessage(310, master.ContextualId,mark.Id));
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
            if (Ended)
                return true;
            if (BlueTeam.GetFighters().Alives().Count == 0 || RedTeam.GetFighters().Alives().Count == 0)
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
            var charactersFighting = GetAllCharacterFighters(true).FindAll(x => !x.HasLeft);
            foreach (var fighter in charactersFighting)
            {
                bool winner = GetWinner() == fighter.Team.TeamColor;
                fighter.Client.Character.RejoinMap(SpawnJoin, winner);
            }
            FightProvider.Instance.RemoveFight(this);
            this.BlueTeam.Dispose();
            this.RedTeam.Dispose();
            this.RedTeam = null;
            this.BlueTeam = null;
            this.Synchronizer = null;
        }
        public void NewTurn()
        {
            Fighter newFighter = TimeLine.PopNextFighter();
            newFighter.StartTurn();

        }
        public abstract void ShowFightResults(List<FightResultListEntry> results, WorldClient client);

        public List<FightResultListEntry> GetFightResults(TeamColorEnum winner)
        {
            List<FightResultListEntry> results = new List<FightResultListEntry>();
            var fighters = GetAllFighters(true);
            foreach (Fighter fighter in fighters)
            {
                if (fighter is CharacterFighter)
                {
                    results.Add(new FightResultPlayer(fighter as CharacterFighter, winner).GetEntry());
                }
                if (fighter is MonsterFighter)
                {
                    if (!fighter.FighterStats.Summoned)
                        results.Add(new FightResultMonster(fighter as MonsterFighter, winner).GetEntry());
                }
            }
            return results;
        }


    }
}
