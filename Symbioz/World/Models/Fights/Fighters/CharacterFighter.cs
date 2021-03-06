﻿using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Providers;
using Symbioz.World.Handlers;
using Symbioz.World.Models.Fights.CastSpellCheck;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.StartTurn;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World.Models.Fights.Fighters
{
    public class CharacterFighter : Fighter
    {
        public const SpellIdEnum PUNCH_SPELL = SpellIdEnum.Punch;
        public bool Available
        {
            get
            {
                return !HasLeft;

            }
        }

        public bool HasLeft = false;
        public bool ReadyToSee = false;
        public bool Disconnected = false;
        public bool FirstRoundDisconnected = false;
        public short TurnDisconnect = (short)ConfigurationManager.Instance.TurnBeforeFightDisconnection;
        public WorldClient Client { get; set; }
        public CharacterFighter(WorldClient client, FightTeam team)
            : base(team)
        {
            this.Client = client;
        }

        public override void Initialize()
        {
            base.Initialize();
            ContextualId = Client.Character.Id;
            FighterLook = Client.Character.Look.CloneContextActorLook();
            RealFighterLook = FighterLook.CloneContextActorLook();
            FighterStats = new FighterStats(Client.Character.CharacterStatsRecord, false, 0, Client.Character.CurrentStats);

            FighterInformations = new GameFightCharacterInformations(ContextualId, FighterLook.ToEntityLook(),
                new EntityDispositionInformations(CellId, Direction), (sbyte)Team.TeamColor,
                0, true, FighterStats.GetMinimalStats(), new ushort[0], Client.Character.Record.Name, new PlayerStatus(0),
                Client.Character.Record.Level, Client.Character.GetActorAlignement(), Client.Character.Record.Breed,
                Client.Character.Record.Sex);
        }

        public override void OnAdded()
        {
            Client.Character.StopRegenLife();
            Client.Send(new GameFightJoinMessage(false, true, false, Fight.FIGHT_PREPARATION_TIME, (sbyte)Fight.FightType));
            Fight.ShowPlacementCells();
            base.OnAdded();
        }

        public void ToogleReadyToSee()
        {
            this.ReadyToSee = true;
            if (Fight.GetAllCharacterFighters().All(x => x.ReadyToSee))
            {
                Fight.OnCharacterFighters(x => x.Character.FighterInstance.ReadyToSee = false);
                Fight.NewTurn();
            }
        }

        public override void Move(List<short> keys, short cellid, sbyte direction)
        {
            try
            {
                CompanionFighter companion = GetCompanion();
                if (companion != null && companion.IsPlaying)
                    companion.Move(keys, cellid, direction);
                else
                    base.Move(keys, cellid, direction);
            } catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public override void EndTurn()
        {
            try
            {
                CompanionFighter companion = GetCompanion();
                if (companion != null && companion.IsPlaying)
                {
                    companion.EndTurn();
                }
                else if (IsPlaying)
                {
                    if (this.Fight.ChallengesInstance != null)
                        this.Fight.ChallengesInstance.HandleEndTurn(this);
                    base.EndTurn();
                    if (companion != null && !companion.Dead)
                        companion.SwitchContext();
                }
            } catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public override bool CastSpellOnTarget(ushort spellid, int targetid, bool checkRange = true)
        {
            try
            {
                CompanionFighter companion = GetCompanion();
                Fighter target = this.GetOposedTeam().getFighterById(targetid);

                if (companion != null && companion.IsPlaying)
                {
                    if (target == null)
                        return companion.CastSpellOnTarget(spellid, targetid);
                    else
                        return companion.CastSpellOnCell(spellid, target.CellId);
                }
                else
                {
                    if (checkRange == false)
                    {
                        if (target == null)
                            return base.CastSpellOnTarget(spellid, targetid);
                        else
                            return base.CastSpellOnCell(spellid, target.CellId, 0, false);
                    }
                    if (target == null)
                        return base.CastSpellOnTarget(spellid, targetid);
                    else
                        return base.CastSpellOnCell(spellid, target.CellId);
                }
            }
            catch (Exception error)
            {
                Logger.Error(error);
                return false;
            }
        }

        public override bool CastSpellOnCell(ushort spellid, short cellid, int targetId = 0, bool checkRange = true)
        {
            try
            {
                CompanionFighter companion = GetCompanion();
                if (companion != null && companion.IsPlaying)
                {
                    return companion.CastSpellOnCell(spellid, cellid, 0, checkRange);
                }
                else
                {
                    return base.CastSpellOnCell(spellid, cellid, 0, checkRange);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public CompanionFighter GetCompanion()
        {
            if (Fight != null)
            {
                if (Fight is FightPvM || Fight is FightDual)
                    return Fight.GetFighter<CompanionFighter>(x => x.Master == this);
                else
                    return null;
            }
            else
                return null;
        }

        public void RemoveCompanion()
        {
            CompanionFighter fighter = GetCompanion();
            if (fighter == null)
                return;
            if (!Fight.Started)
            {
                Fight.Send(new GameFightRemoveTeamMemberMessage((short)Fight.Id, Team.Id, fighter.ContextualId));
                if (Fight.FightType != FightTypeEnum.FIGHT_TYPE_PVP_ARENA)
                    Fight.Map.Instance.OnFighterRemoved(Fight.Id, Team.Id, fighter.ContextualId);
                Team.RemoveFighter(fighter);
            }
            else
            {
                if (!fighter.Dead)
                {
                    fighter.Die();
                    fighter.Dead = true;
                }
            }
        }

        public void Leave()
        {
            try
            {
                if (!Team.GetCharacterFighters(true).Contains(this))
                    return;
                if (!Fight.Started && Fight.FightType != FightTypeEnum.FIGHT_TYPE_PvM && Fight.FightType != FightTypeEnum.FIGHT_TYPE_AGRESSION)
                {
                    RemoveCompanion();
                    Fight.Send(new GameFightRemoveTeamMemberMessage((short)Fight.Id, Team.Id, ContextualId));
                    if (Fight.FightType != FightTypeEnum.FIGHT_TYPE_PVP_ARENA)
                        Fight.Map.Instance.OnFighterRemoved(Fight.Id, Team.Id, ContextualId);
                    Team.RemoveFighter(this);
                    Fight.CheckFightEnd();
                    Client.Character.RejoinMap(Fight.SpawnJoin, false);
                }
                else
                {
                    if (Fight.FightType == FightTypeEnum.FIGHT_TYPE_PvM && Fight.FightType == FightTypeEnum.FIGHT_TYPE_AGRESSION)
                    {
                        Client.Character.Record.DeathCount++;
                        if (Client.Character.Record.DeathMaxLevel < Client.Character.Record.Level)
                            Client.Character.Record.DeathMaxLevel = Client.Character.Record.Level;
                        Client.Character.Record.Energy = 0;
                        Client.Character.Look = ContextActorLook.Parse("{24}");
                        Client.Character.Restrictions.cantMove = true;
                        Client.Character.Restrictions.cantSpeakToNPC = true;
                        Client.Character.Restrictions.cantExchange = true;
                        Client.Character.Restrictions.cantAttackMonster = true;
                        Client.Character.Restrictions.isDead = true;
                        String[] Data = new string[1];
                        Data[0] = Client.Character.Record.Name;
                        Client.Send(new TextInformationMessage(1, 190, Data));
                        Client.Character.CurrentStats.LifePoints = 0;
                    }
                    Team.RemoveFighter(this);
                    RemoveCompanion();
                    if (!Dead)
                        Die();
                    Fight.CheckFightEnd();
                    HasLeft = true;
                    this.AknowlegeAndLeave();
                }
            } catch(Exception e)
            {
                Logger.Error(e);
            }
        }

        public void AknowlegeAndLeave()
        {
            try // if two players leave at the same time, because of non acknowlege action
            {
                if (IsPlaying)
                    EndTurn();
                Fight.CheckFightEnd();

                Fight.Send(new GameFightLeaveMessage(ContextualId));
                if (Fight.FightType == FightTypeEnum.FIGHT_TYPE_PvM || Fight.FightType == FightTypeEnum.FIGHT_TYPE_AGRESSION)//Heroique
                {
                    if (Core.ConfigurationManager.Instance.ServerId == 22)
                    {
                        Client.Character.Record.DeathCount++;
                        if (Client.Character.Record.DeathMaxLevel < Client.Character.Record.Level)
                            Client.Character.Record.DeathMaxLevel = Client.Character.Record.Level;
                        Client.Character.Record.Energy = 0;
                        Client.Character.Look = ContextActorLook.Parse("{24}");
                        Client.Character.Restrictions.cantMove = true;
                        Client.Character.Restrictions.cantSpeakToNPC = true;
                        Client.Character.Restrictions.cantExchange = true;
                        Client.Character.Restrictions.cantAttackMonster = true;
                        Client.Character.Restrictions.isDead = true;
                        String[] Data = new string[1];
                        Data[0] = Client.Character.Record.Name;
                        Client.Send(new TextInformationMessage(1, 190, Data));
                    }
                }
                if (Fight.Started && Fight.FightType != FightTypeEnum.FIGHT_TYPE_PVP_ARENA)
                    Fight.ShowFightResults(Fight.GetFightResultsForLeaver(GetOposedTeam().TeamColor), Client);
                if (Fight.FightType == FightTypeEnum.FIGHT_TYPE_PVP_ARENA)
                {
                    Client.Send(new GameRolePlayArenaRegistrationStatusMessage(false, (sbyte)PvpArenaStepEnum.ARENA_STEP_UNREGISTER, ArenaProvider.FIGHTERS_PER_TEAM));
                }
                Client.Character.RejoinMap(Fight.SpawnJoin, false);
            }
            catch (Exception error)
            {
                Logger.Error(error);   
            }

        }

        public override void Die()
        {
            try
            {
                CompanionFighter companion = GetCompanion();
                if (companion != null && !companion.Dead)
                    companion.SwitchContext();
                base.Die();
            } catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public void OnDisconnect()
        {
            /*if (Fight == null)
                return;
            if (this.TurnDisconnect > 0)
            {
                String[] Data = new string[2];
                Data[0] = Client.Character.Record.Name;
                Data[1] = TurnDisconnect.ToString();
                if (this.FirstRoundDisconnected == false)
                    this.TurnDisconnect--;
                else
                    this.FirstRoundDisconnected = false;
                if (TurnDisconnect != 0)
                    Fight.Send(new TextInformationMessage(1, 182, Data));
                return;
            }
            if (!Fight.Started)
                Leave();
            else
            {*/
            try
            {
                Die();
                Leave();
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
                //AknowlegeAndLeave();
                //Fight.OnCharacterFighters(x => x.Character.NotificationError(GetName() + " s'est déconnecté !"));
            //}
        }

        public override void RefreshStats()
        {

            this.FighterInformations = new GameFightCharacterInformations(ContextualId, FighterLook,
                new EntityDispositionInformations(CellId, Direction), (sbyte)Team.TeamColor,
                0, !Dead, FighterStats.GetMinimalStats(), new ushort[0], Client.Character.Record.Name, new PlayerStatus(0),
                Client.Character.Record.Level, Client.Character.GetActorAlignement(), Client.Character.Record.Breed,
                Client.Character.Record.Sex);
            Client.Send(new FighterStatsListMessage(FighterStats.GetCharacterCharacteristics(Client.Character)));

        }

        public override SpellLevelRecord GetSpellLevel(ushort spellid)
        {
            var spell = Client.Character.Spells.Find(x => x.spellId == spellid);
            return SpellLevelRecord.GetLevel(spellid, spell.spellLevel);
        }

        public override FightTeamMemberInformations GetFightMemberInformations()
        {
            return new FightTeamMemberCharacterInformations(ContextualId, Client.Character.Record.Name, Client.Character.Record.Level);
        }

        public void ToogleReady(bool ready)
        {
            ReadyToFight = ready;
            Fight.Send(new GameFightHumanReadyStateMessage((uint)ContextualId, ready));
            if (Fight.GetAllFighters().All(x => x.ReadyToFight))
            {
                Fight.StartFight();
            }
        }

        public override void StartTurn()
        {
            try
            {
                if (this.Disconnected)
                    OnDisconnect();
                base.StartTurn();
                if (this.Client.Character.Record.Breed == 10)
                    StartTurnSadida.EffectAllTreeSadida(this);
                RefreshStats();
                Client.Send(new GameFightTurnStartPlayingMessage());
                if (this.Disconnected)
                    base.EndTurn();
            }
            catch (Exception error)
            {
                Logger.Error(error);
            }
        }

        public void ShowPlacementCells()
        {
            Client.Send(new GameFightPlacementPossiblePositionsMessage(Fight.BlueTeam.PlacementCells.ListCast<short, ushort>(), Fight.RedTeam.PlacementCells.ListCast<short, ushort>(), Team.Id));
        }

        public override void OnSpellCastFailed(CastFailedReason reason, SpellLevelRecord spelllevel)
        {
            switch (reason)
            {
                case CastFailedReason.RANGE:
                    Client.Character.ReplyError("Vous n'avez pas la portée pour lancer ce sort.");
                    break;
                case CastFailedReason.FIGHT_ENDED:
                    Client.Character.ReplyError("Le combat est terminé.");
                    break;
                case CastFailedReason.FORBIDDEN_STATE:
                    Client.Character.ReplyError("Impossible dans votre état actuel.");
                    break;
                case CastFailedReason.AP_COST:
                    Client.Character.ReplyError("Vous n'avez pas assez de PA pour lancer ce sort.");
                    break;
                case CastFailedReason.CAST_LIMIT:
                    RefreshStats();
                    break;
                case CastFailedReason.NOT_PLAYING:
                    Client.Character.ReplyError("Vous n'etes pas entrain de jouer.");
                    break;
            }
            Fight.Send(new GameActionFightNoSpellCastMessage((uint)spelllevel.Grade));
        }

        public override string GetName()
        {
            return Client.Character.Record.Name;
        }

        public override int GetInitiative()
        {
            return Client.Character.Initiative;
        }

        public void UsePunch(short cellid)
        {
            try
            {
                Fight.TryStartSequence(this.ContextualId, 2);
                Fighter target = Fight.GetFighter(cellid);
                SpellLevelRecord spell = GetSpellLevel((ushort)PUNCH_SPELL);
                if (target != null)
                {
                    Fight.Send(new GameActionFightCloseCombatMessage(0, this.ContextualId, target.ContextualId, cellid, 0, false, 0));
                    var jet = CalculateJet(spell.Effects[0], FighterStats.Stats.Strength);
                    target.TakeDamages(new TakenDamages(jet, ElementType.Earth), this.ContextualId);
                }
                else
                {
                    Fight.Send(new GameActionFightCloseCombatMessage(0, this.ContextualId, 0, cellid, 0, false, 0));
                }
                GameActionFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, (short)-spell.ApCost);
                FighterStats.Stats.ActionPoints -= spell.ApCost;
                RefreshStats();
                Fight.TryEndSequence(2, 0);
                Fight.CheckFightEnd();
            }
            catch (Exception error)
            {
                Logger.Error(error);
            }
        }

        public override bool UseWeapon(short cellid)
        {
            try
            {
                var target = Fight.GetFighter(cellid);
                if (this.Fight.ChallengesInstance != null)
                    this.Fight.ChallengesInstance.HandleWeaponUse(this);

                if (Client.Character.isGod == true)
                {
                    if (target != null)
                    {
                        target.TakeDamages(new TakenDamages(25000, ElementType.Earth), this.ContextualId);
                        Fight.CheckFightEnd();
                        return true;
                    }
                }
                CharacterItemRecord weapon = Client.Character.Inventory.GetEquipedWeapon();
                if (weapon == null)
                {
                    UsePunch(cellid);
                    return true;
                }
                WeaponRecord template = WeaponRecord.GetWeapon(weapon.GID);

                FightSpellCastCriticalEnum critical = RollCriticalDice(template);
                Fight.TryStartSequence(this.ContextualId, 2);
                int targetId = target != null ? target.ContextualId : 0;
                Fight.Send(new GameActionFightCloseCombatMessage(0, this.ContextualId, targetId, cellid, (sbyte)critical, false, weapon.GID));
                var effects = template.GetWeaponEffects(critical);
                this.HandleWeaponEffect(cellid, effects, critical, template, Fight.Map, this.CellId);
                this.FighterStats.Stats.ActionPoints -= template.ApCost;
                this.GameActionFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, (short)(-template.ApCost));
                Fight.TryEndSequence(2, 0);
                return true;
            }
            catch (Exception error)
            {
                Logger.Error(error);
                return false;
            }
        }

        private void CheckFightEnd()
        {
            Fight.CheckFightEnd();
        }

        public void HandleWeaponEffect(short cellid, List<ExtendedSpellEffect> handledEffects, FightSpellCastCriticalEnum critical,
            WeaponRecord template, MapRecord CurrentMap, short CasterCellId)
        {
            foreach (var effect in handledEffects)
            {
                try
                {
                    Fight.TryStartSequence(ContextualId, 1);
                    short[] cells = Pathfinding.getCiblesByZoneByWeapon(CurrentMap, template, cellid, CasterCellId);
                    var actors = GetAffectedActors(cells, effect.Targets);
                    CharacterStates.CharacterSpellStatesChecker(this, actors, null, effect);
                    MonsterStates.MonsterSpellStatesChecker(this, actors, null, effect);
                    SpellEffectsHandler.Handle(this, null, effect, actors, cellid);
                    Fight.TryEndSequence(1, 0);
                }
                catch (Exception error)
                {
                    Logger.Error(error);
                }
            }
            this.CheckFightEnd();
        }
    }
}
