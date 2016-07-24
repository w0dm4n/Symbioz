using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records;
using Symbioz.World.Records.Alliances.Prisms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World.Models.Fights.FightsTypes
{
    public class FightAvAPrism : Fight
    {
        private PrismRecord Prism;
        private short BattleLauncherCellId;

        #region FightType, AddFightSword

        public override FightTypeEnum FightType
        {
            get
            {
                return FightTypeEnum.FIGHT_TYPE_PvT;
            }
        }

        public override bool AddFightSword
        {
            get
            {
                return true;
            }
        }

        public double AttackersPlacementPhaseTime
        {
            get
            {
                return 15000;
            }
        }

        public double DefendersPlacementPhaseTime
        {
            get
            {
                return 120000;
            }
        }

        private double LagCompensationPhaseTime
        {
            get
            {
                return 3.0;
            }
        }

        private bool m_isAttackersPlacementPhase;
        public bool IsAttackersPlacementPhase
        {
            get
            {
                return this.m_isAttackersPlacementPhase;
            }
        }
        private Timer AttackersPlacementTimer = null;
        private DateTime AttackersPlacementTimerNextTick;

        private bool IsDefendersPlacementPhase;
        private Timer DefendersPlacementTimer = null;
        private DateTime DefendersPlacementTimerNextTick;

        private List<Character> DefendersQueue = new List<Character>();
        private object DefendersLockObject = new object();

        #endregion

        public FightAvAPrism(int id, MapRecord map, FightTeam blueTeam, FightTeam redTeam, short fightCellId, short battleLauncherCellId, PrismRecord prism)
            : base(id, map, blueTeam, redTeam, fightCellId, true)
        {
            this.Prism = prism;
            this.BattleLauncherCellId = battleLauncherCellId;
        }

        public override void StartPlacement()
        {
            this.Prism.ParsedState = PrismStateEnum.PRISM_STATE_ATTACKED;
            this.Map.Instance.RemoveEntity(PrismRecord.ConstantContextualId);
            this.m_isAttackersPlacementPhase = true;
            this.StartAttackersPlacement();
            base.StartPlacement();
        }

        #region FightPreparation

        private void StartAttackersPlacement()
        {
            this.AttackersPlacementTimer = new Timer(this.AttackersPlacementPhaseTime);
            this.AttackersPlacementTimer.Elapsed += (sender, e) => this.OnAttackersPlacementEnded();
            this.AttackersPlacementTimerNextTick = (DateTime.Now + TimeSpan.FromMilliseconds(this.AttackersPlacementPhaseTime));
            this.AttackersPlacementTimer.Start();
        }

        private void OnAttackersPlacementEnded()
        {
            this.m_isAttackersPlacementPhase = false;
            this.AttackersPlacementTimer.Dispose();
            base.StartFight();

            //TODO: ThreatDefendersQueue ...
            //this.TreatDefendersQueue();
            //this.StartDefendersPlacement();
        }

        private void StartDefendersPlacement()
        {
            this.DefendersPlacementTimer = new Timer(this.DefendersPlacementPhaseTime);
            this.DefendersPlacementTimer.Elapsed += (sender, e) => this.OnDefendersPlacementEnded();
            this.DefendersPlacementTimerNextTick = (DateTime.Now + TimeSpan.FromMilliseconds(this.DefendersPlacementPhaseTime));
            this.DefendersPlacementTimer.Start();
            this.IsDefendersPlacementPhase = true;
        }

        private void OnDefendersPlacementEnded()
        {
            Console.WriteLine("OnDefendersPlacementEnded");
            this.IsDefendersPlacementPhase = false;
            this.DefendersPlacementTimer.Dispose();

            this.Prism.ParsedState = PrismStateEnum.PRISM_STATE_FIGHTING;
            //TODO: StartFight ...
            base.StartFight();
        }

        #endregion

        #region GetInformations()

        public override FightCommonInformations GetFightCommonInformations()
        {
            List<FightTeamInformations> teams = new List<FightTeamInformations>();
            teams.Add(BlueTeam.GetFightTeamInformations());
            teams.Add(RedTeam.GetFightTeamInformations());
            List<ushort> positions = new List<ushort>();
            positions.Add((ushort)this.BattleLauncherCellId);
            positions.Add((ushort)this.FightCellId);
            return new FightCommonInformations(this.Id, (sbyte)this.FightType, teams, positions, new List<FightOptionsInformations>() { BlueTeam.TeamOptions, RedTeam.TeamOptions });
        }

        public PrismFightersInformation GetPrismFightersInformation()
        {
            List<CharacterMinimalPlusLookInformations> alliesCharacterMinimalPlusLookInformations = new List<CharacterMinimalPlusLookInformations>();
            List<CharacterMinimalPlusLookInformations> enemiesCharacterMinimalPlusLookInformations = new List<CharacterMinimalPlusLookInformations>();
            base.GetAllCharacterFighters().ForEach((x) =>
            {
                if (x.Team.TeamColor == TeamColorEnum.RED_TEAM)
                {
                    alliesCharacterMinimalPlusLookInformations.Add(x.Client.Character.GetCharacterMinimalPlusLookInformations());
                }
                else
                {
                    enemiesCharacterMinimalPlusLookInformations.Add(x.Client.Character.GetCharacterMinimalPlusLookInformations());
                }
            });
            return new PrismFightersInformation((ushort)this.Prism.SubAreaId,
                new ProtectedEntityWaitingForHelpInfo((int)(this.GetAttackersPlacementTimeLeft().TotalMilliseconds / 100.0), (int)(this.GetDefendersWaitTimeForPlacement().TotalMilliseconds / 100.0), (sbyte)this.GetDefendersLeftSlot()),
                alliesCharacterMinimalPlusLookInformations, enemiesCharacterMinimalPlusLookInformations);
        }

        public PrismRecord GetPrism()
        {
            return this.Prism;
        }

        #endregion

        #region BasicFightMethods

        /*public override void ShowFightResults(List<FightResultListEntry> results, WorldClient client)
        {
            throw new NotImplementedException();
        }*/

        public override void ShowFightResults(List<FightResultListEntry> results, WorldClient client)
        {
            client.Send(new GameFightEndMessage(this.GetFightDuration(), 0, 0, results, new NamedPartyTeamWithOutcome[0]));
        }

        public override void TryJoin(WorldClient client, int mainFighterId)
        {
            if (this.IsAttackersPlacementPhase)
            {
                var mainFighter = GetAllFighters().Find(x => x.ContextualId == mainFighterId);
                if (mainFighter != null)
                {
                    if (mainFighter.Team.TeamColor == TeamColorEnum.RED_TEAM)
                    {
                        if (client.Character.HasAlliance)
                        {
                            if (client.Character.AllianceId == mainFighter.AllianceId)
                            {
                                if (this.RedTeam.GetFighters().Count() < 5)
                                {
                                    var newFighter = client.Character.CreateFighter(this.RedTeam);
                                    this.RedTeam.AddFighter(newFighter);
                                    this.GetAllFighters().ForEach(x => x.ShowFighter(client));
                                    this.Map.Instance.OnFighterAdded(Id, this.RedTeam.Id, newFighter.GetFightMemberInformations());
                                }
                                else
                                    client.Character.Reply("Impossible car il y a déjà 5 défenseurs !");
                            }
                        }
                        return;
                    }
                }

                if (base.CanJoin(client, this.BlueTeam, mainFighter))
                {
                    if (mainFighter is CharacterFighter)
                    {
                        if (this.CanJoinPrismAttack(client, (CharacterFighter)mainFighter))
                        {
                            if (this.BlueTeam.GetFighters().Count() < 5)
                            {
                                var newFighter = client.Character.CreateFighter(this.BlueTeam);
                                this.BlueTeam.AddFighter(newFighter);
                                this.GetAllFighters().ForEach(x => x.ShowFighter(client));
                                this.Map.Instance.OnFighterAdded(Id, this.BlueTeam.Id, newFighter.GetFightMemberInformations());
                            }
                            else
                                client.Character.Reply("Impossible car il y a déjà 5 attaquants !");
                        }
                        else
                        {
                            client.Character.Reply("Vous n'appartenez pas à l'alliance qui vient de lancer ce combat.");
                        }
                    }
                }
            }
        }

        #endregion

        #region CanJoin()

        public bool CanJoinPrismAttack(WorldClient client, CharacterFighter mainFighter = null)
        {
            bool canJoin = false;
            if (client.Character.HasAlliance && client.Character.AllianceId == mainFighter.Client.Character.AllianceId)
            {
                canJoin = true;
            }
            return canJoin;
        }

        #endregion

        #region TimeLeft

        public System.TimeSpan GetAttackersPlacementTimeLeft()
        {
            System.TimeSpan result;
            if (this.IsAttackersPlacementPhase)
            {
                result = this.AttackersPlacementTimerNextTick - System.DateTime.Now;
            }
            else
            {
                result = System.TimeSpan.Zero;
            }
            return result.Subtract(TimeSpan.FromSeconds(LagCompensationPhaseTime));
        }

        public System.TimeSpan GetDefendersWaitTimeForPlacement()
        {
            return System.TimeSpan.FromMilliseconds(this.AttackersPlacementPhaseTime);
        }

        public override double GetPlacementTimeLeft(Fighter fighter = null)
        {
            double timeLeft = TimeSpan.Zero.TotalMilliseconds;

            if (!base.Started && fighter.Team == base.BlueTeam)
            {
                timeLeft = this.AttackersPlacementPhaseTime;
            }
            else
            {
                if (fighter.Team == base.RedTeam && this.DefendersPlacementTimer == null)
                {
                    timeLeft = this.DefendersPlacementPhaseTime;
                }
                else
                {
                    if (this.IsAttackersPlacementPhase)
                    {
                        timeLeft = this.AttackersPlacementPhaseTime - (DateTime.Now - base.CreationTime).TotalMilliseconds;
                    }
                    else if (this.IsDefendersPlacementPhase)
                    {
                        timeLeft = this.DefendersPlacementPhaseTime - (DateTime.Now - base.CreationTime).TotalMilliseconds;
                    }
                }
            }

            return timeLeft / 100;
        }

        #endregion

        #region DefendersQueue

        public int GetDefendersLeftSlot()
        {
            return (5 - this.DefendersQueue.Count > 0) ? (5 - this.DefendersQueue.Count) : 0;
        }

        public bool DefendersSlotAvailable()
        {
            lock (this.DefendersLockObject)
            {
                bool defendersSlotAvailable = false;
                if (this.GetDefendersLeftSlot() > 0)
                {
                    defendersSlotAvailable = true;
                }
                return defendersSlotAvailable;
            }
        }

        public bool AddDefenderToQueue(Character defender)
        {
            lock (this.DefendersLockObject)
            {
                bool added = false;
                if (this.DefendersQueue.Count < 5)
                {
                    this.DefendersQueue.Add(defender);
                    added = true;
                }
                return added;
            }
        }

        public bool IsInDefendersQueue(Character character)
        {
            lock (this.DefendersLockObject)
            {
                return this.DefendersQueue.Contains(character);
            }
        }

        public void RemoveDefenderFromQueue(Character defender)
        {
            lock (this.DefendersLockObject)
            {
                this.DefendersQueue.Remove(defender);
            }
        }

        private void TreatDefendersQueue()
        {
            lock (this.DefendersLockObject)
            {
                if (this.DefendersQueue.Count > 0)
                {
                    foreach (Character defender in this.DefendersQueue)
                    {
                        if (defender.Client != null)
                        {
                            this.RedTeam.AddFighter(defender.CreateFighter(this.RedTeam));
                        }
                    }
                }
            }
        }

        #endregion

    }
}
