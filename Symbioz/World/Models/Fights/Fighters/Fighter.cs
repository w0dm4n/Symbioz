using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.PathProvider;
using Symbioz.Providers;
using Symbioz.Providers.SpellEffectsProvider;
using Symbioz.Providers.SpellEffectsProvider.Buffs;
using Symbioz.Providers.SpellEffectsProvider.CustomSpellsEffectsProvider;
using Symbioz.Providers.SpellEffectsProvider.TargetsMasksProvider;
using Symbioz.World.Models.Fights.Damages;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Fighters
{
    public abstract class Fighter
    {
        public short CellId { get; set; }

        public sbyte Direction { get; set; }

        public int ContextualId { get; set; }

        public GameFightFighterInformations FighterInformations { get; set; }

        public ContextActorLook FighterLook { get; set; }

        public ContextActorLook RealFighterLook { get; set; }

        public bool UsingWeapon = false;

        public Fight Fight { get; set; }

        public FightTeam Team { get; set; }

        public FighterStats FighterStats { get; set; }

        public bool ReadyToFight = false;

        public SpellHistory SpellHistory = new SpellHistory();

        public bool Dead = false;

        public UIDProvider BuffIdProvider = new UIDProvider();

        public List<Buff> Buffs = new List<Buff>();

        private readonly List<short> m_stateIds = new List<short>();

        FightTurnEngine m_turnEngine { get; set; }

        private readonly List<SpellBoost> m_buffedSpells = new List<SpellBoost>();

        public short SummonCount { get { return (short)GetAliveFighterSummons().Count(); } }

        public List<short> LastPosition = new List<short>();

        public short LastTurnPosition = 0;

        public Fighter(FightTeam team)
        {
            this.Team = team;
            this.m_turnEngine = new FightTurnEngine(this);
        }
        /// <summary>
        /// montre le fighter a tout les clients
        /// </summary>
        public void ShowFighter()
        {
            RefreshStats();
            Fight.Send(new GameFightShowFighterMessage(FighterInformations));
        }
        /// <summary>
        /// Montre le fighter au clients
        /// </summary>
        /// <param name="client"></param>
        public void ShowFighter(WorldClient client)
        {
            RefreshStats();
            client.Send(new GameFightShowFighterMessage(FighterInformations));
        }
        public virtual void SwapPosition(Fighter fighter)
        {

        }
        public IdentifiedEntityDispositionInformations GetIdentifiedEntityDisposition()
        {
            return new IdentifiedEntityDispositionInformations(CellId, Direction, ContextualId);
        }
        public void ShowFighterWithRandomStaticPose()
        {
            RefreshFighter();
            Fight.Send(new GameFightShowFighterRandomStaticPoseMessage(FighterInformations));
        }
        public void RefreshFighter()
        {
            RefreshStats();
            Fight.Send(new GameFightRefreshFighterMessage(FighterInformations));
        }
        public virtual void Initialize()
        {
            CellId = Fight.GetPlacementCell(Team.TeamColor);
            Direction = 3;
        }
        public abstract FightTeamMemberInformations GetFightMemberInformations();
        public virtual void OnAdded()
        {
            Fight.TimeLine.Add(this);
            Initialize();
        }
        public void IncrementBombs()
        {
            GetBombs().ForEach(x => x.Increment());
        }
        public virtual void StartTurn()
        {

            if (CheckFighterDie())
            {
                Fight.CheckFightEnd();
                return;
            }
            IncrementBombs();
            LastTurnPosition = CellId;
            UpdateBuffs();
            DecrementGlyphsDuration();
            ApplyFighterEvent(FighterEventType.ON_TURN_STARTED, null);
            Fight.Send(new GameFightTurnStartMessage(ContextualId, FightTurnEngine.TURN_TIMEMOUT / 100));
            IsPlaying = true;
            RefreshStats();
            m_turnEngine.StartTurn();


        }
        public void DecrementGlyphsDuration()
        {
            List<Glyph> glyphs = Fight.GetMarks<Glyph>(x => x.Caster == this);
            glyphs.ForEach(x => x.DecrementDuration());
        }
        public void AddState(uint buffid, short stateid, int sourceid, short duration, short spellid)
        {
            Fight.Send(new GameActionFightDispellableEffectMessage((ushort)EffectsEnum.Eff_AddState, sourceid, new FightTemporaryBoostStateEffect(buffid, ContextualId, duration, 1, (ushort)spellid, 0, 1, 0, stateid)));
            m_stateIds.Add(stateid);
        }
        public void RemoveState(short stateid, short sourcespellid)
        {
            Fight.Send(new GameActionFightDispellSpellMessage(951, ContextualId, ContextualId, (ushort)sourcespellid));
            m_stateIds.Remove(stateid);
        }
        public bool HaveState(short stateid)
        {
            return m_stateIds.Contains(stateid);
        }
        public virtual void EndTurn()
        {
            if (Fight.Ended)
                return;
            ApplyFighterEvent(FighterEventType.ON_TURN_ENDED, null);
          
            Fight.Send(new GameFightTurnEndMessage(ContextualId));
            IsPlaying = false;
            m_turnEngine.EndTurn();
            FighterStats.OnTurnEnded();
            SpellHistory.OnTurnEnded();
            RefreshStats();
            Fight.Synchronizer.Start(Fight.NewTurn);
        }
        public void Teleport(short cellid,bool save = true)
        {
            if (Fight.Ended)
                return;
            this.LastPosition.Add(this.CellId);
            this.Fight.TryStartSequence(ContextualId, 5);
            this.Fight.Send(new GameActionFightTeleportOnSameMapMessage(4, ContextualId, ContextualId, cellid));
            this.CellId = cellid;
            RefreshStats();
            this.Fight.TryEndSequence(5, 5);
        }
        public List<short> CheckMarks(List<short> path)
        {
            List<short> finalPath = new List<short>();
            finalPath.Add(CellId);
            foreach (var cell in path)
            {
                if (cell != CellId)
                {
                    var marks = Fight.MarkInteractions.FindAll(x => x.EventType == FighterEventType.AFTER_MOVE && x.Mark.Cells.Contains(cell));
                    finalPath.Add(cell);
                    foreach (var mark in marks)
                    {
                        return finalPath;
                    }
                }
            }
            return finalPath;
        }
        public void Slide(int sourceid, List<short> cells)
        {
            ApplyFighterEvent(FighterEventType.BEFORE_MOVE, cells);
            LastPosition.Add(CellId);
            cells = CheckMarks(cells);
            Fight.TryStartSequence(ContextualId, 5);
            Fight.Send(new GameActionFightSlideMessage(0, sourceid, ContextualId, CellId, cells.Last()));
            CellId = cells.Last();
            Fight.TryEndSequence(5, 0);
            ApplyFighterEvent(FighterEventType.AFTER_MOVE, 0, cells); // 0 => mp cost
        }
        public virtual void Move(List<short> keys, short cellid, sbyte direction)
        {
            if (Fight.Ended)
                return;

            List<short> path = Pathfinding.FightMove(PathParser.ReturnDispatchedCells(keys)).Keys.ToList();
            path.Insert(0, CellId);
            path = CheckMarks(path);
            ApplyFighterEvent(FighterEventType.BEFORE_MOVE, path);
            this.Fight.TryStartSequence(ContextualId, 5);
            this.Fight.Send(new GameMapMovementMessage(path, ContextualId));
            this.AddToLastPosition(path);
            short mpcost = (short)PathHelper.GetDistanceBetween(CellId, path.Last());
            FighterStats.Stats.MovementPoints -= mpcost;
            this.CellId = path.Last();
            this.Direction = direction;
            RefreshStats();
            GameActionFightPointsVariation(ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE, (short)-mpcost);
            this.Fight.TryEndSequence(5, 5);
            ApplyFighterEvent(FighterEventType.AFTER_MOVE, mpcost, path);

        }
        public void GameActionFightPointsVariation(ActionsEnum action, short delta)
        {
            this.Fight.TryStartSequence(ContextualId, 1);
            this.Fight.Send(new GameActionFightPointsVariationMessage((ushort)action, ContextualId, ContextualId, delta));
            this.Fight.TryEndSequence(1, 100);
        }
        public FightTeam GetOposedTeam()
        {
            if (Team == Fight.BlueTeam)
                return Fight.RedTeam;
            else
                return Fight.BlueTeam;
        }
        public virtual void Die()
        {
            ApplyFighterEvent(FighterEventType.BEFORE_DIED, null);
            GetAliveFighterSummons().ForEach(x => x.Die());
            OnDied();
            Fight.TryEndSequence(1, 100);
            Fight.TryStartSequence(ContextualId, 6);
            Fight.Send(new GameActionFightDeathMessage(103, ContextualId, ContextualId));
            Fight.TryEndSequence(6, 103);
            Fight.TimeLine.RemoveFighter(this);
            Dead = true;
            ApplyFighterEvent(FighterEventType.AFTER_DIED, null);
        }
        /// <summary>
        /// Début de l'animation
        /// </summary>
        public void OnDied()
        {
            Fight.RemoveAllBuffs<Buff>(x => x.SourceId == this.ContextualId);
        }
        public double CalculateCriticRate(double baseRate)
        {
            double num = System.Math.Floor((baseRate - (double)this.FighterStats.Stats.CriticalHit * 2.99011001130495 / System.Math.Log((double)(this.FighterStats.Stats.Agility + 12), 2.7182818284590451)));
            return (num > 2.0) ? num : 2.0;
        }
        public FightSpellCastCriticalEnum RollCriticalDice(SpellLevelRecord spell)
        {
            AsyncRandom asyncRandom = new AsyncRandom();
            FightSpellCastCriticalEnum result = FightSpellCastCriticalEnum.NORMAL;
            if (spell.CriticalHitProbability != 0u && asyncRandom.Next((int)this.CalculateCriticRate(spell.CriticalHitProbability)) == 0)
            {
                result = FightSpellCastCriticalEnum.CRITICAL_HIT;
            }
            return result;
        }
        public FightSpellCastCriticalEnum RollCriticalDice(WeaponRecord weapon)
        {
            AsyncRandom asyncRandom = new AsyncRandom();
            FightSpellCastCriticalEnum result = FightSpellCastCriticalEnum.NORMAL;
            int num = (int)this.CalculateCriticRate(weapon.CriticalHitProbability) / 6;
            if (weapon.CriticalHitProbability != 0 && asyncRandom.Next(num) == 0)
            {
                result = FightSpellCastCriticalEnum.CRITICAL_HIT;
            }
            return result;
        }
        public void SelectRandomEffect(List<ExtendedSpellEffect> effects)
        {
            List<ExtendedSpellEffect> randomEffects = effects.FindAll(x => x.BaseEffect.Random != 0);
            if (randomEffects.Count > 0)
            {
                var reffect = randomEffects.Random<ExtendedSpellEffect>();
                effects.RemoveAll(x => x.BaseEffect.Random != 0);
                effects.Add(reffect);
            }
        }
        public void HandleSpellEffects(SpellLevelRecord spell, short cellid, FightSpellCastCriticalEnum critical)
        {
            List<ExtendedSpellEffect> handledEffects = critical == FightSpellCastCriticalEnum.CRITICAL_HIT ? spell.CriticalEffects : spell.Effects;

            Dictionary<ExtendedSpellEffect, List<Fighter>> validatedEffectsDatas = new Dictionary<ExtendedSpellEffect, List<Fighter>>();


            SelectRandomEffect(handledEffects);
            foreach (var effect in handledEffects)
            {

                short[] cells = ShapesProvider.Handle(effect.ZoneShape, cellid, CellId, effect.ZoneSize).ToArray();
                var actors = GetAffectedActors(cells, effect.Targets);
                validatedEffectsDatas.Add(effect, actors);

            }
            foreach (var data in validatedEffectsDatas)
            {
                Fight.TryStartSequence(ContextualId, 1);
                SpellEffectsHandler.Handle(this, spell, data.Key, data.Value, cellid);
                Fight.TryEndSequence(1, 0);
            }

        }
        bool ValidState(SpellLevelRecord spellLevel)
        {
            foreach (var state in spellLevel.StatesForbidden)
            {
                if (m_stateIds.Contains(state))
                    return false;
            }
            return true;
        }

        public void AddSpellBoost(ushort spellId, short delta)
        {
            m_buffedSpells.Add(new SpellBoost(spellId, delta));
        }
        public void RemoveSpellBoost(ushort spellId, short delta)
        {
            m_buffedSpells.RemoveAll(x => x.SpellId == spellId && x.Delta == delta);
        }
        public virtual bool CastSpellOnCell(ushort spellid, short cellid, int targetId = 0)
        {

            SpellLevelRecord spellLevl = GetSpellLevel(spellid);

            if (!IsPlaying)
            {
                OnSpellCastFailed(CastFailedReason.NOT_PLAYING, spellLevl);
                return false;
            }
            if (Fight.Ended)
            {
                OnSpellCastFailed(CastFailedReason.FIGHT_ENDED, spellLevl);
                return false;
            }

            if (!ValidState(spellLevl))
            {
                OnSpellCastFailed(CastFailedReason.FORBIDDEN_STATE, spellLevl);
                return false;
            }
            if (spellLevl.ApCost > FighterStats.Stats.ActionPoints)
            {
                OnSpellCastFailed(CastFailedReason.AP_COST, spellLevl);
                return false;
            }
            if ((SpellIdEnum)spellid == SpellIdEnum.Punch)
            {
                UsingWeapon = true;
                bool result = UseWeapon(cellid);
                UsingWeapon = false;
                return result;
            }
            if (targetId == 0)
            {
                var target = Fight.GetFighter(cellid);
                if (target != null)
                    targetId = target.ContextualId;
            }
            if (!SpellHistory.CanCast(spellLevl,targetId))
            {
                OnSpellCastFailed(CastFailedReason.CAST_LIMIT, spellLevl);
                return false;
            }
            
            this.Fight.TryStartSequence(this.ContextualId, 1);
            FightSpellCastCriticalEnum critical = RollCriticalDice(spellLevl);

            short[] portals = new short[0];// WHAT THE FUCK

            this.Fight.Send(new GameActionFightSpellCastMessage(0, ContextualId, targetId, cellid, (sbyte)critical, false, spellid, spellLevl.Grade, portals));
            this.SpellHistory.Add(spellLevl, targetId);

            #region EffectHandler

            if (CustomSpellEffectsProvider.Exist(spellid))
            {
                Fight.TryStartSequence(ContextualId, 1);
                CustomSpellEffectsProvider.Handle(spellid, spellLevl.Effects, this);
                Fight.TryEndSequence(1, 0);
            }
            else
            {
                HandleSpellEffects(spellLevl, cellid, critical);
            }
            #endregion

            FighterStats.Stats.ActionPoints -= spellLevl.ApCost;
            GameActionFightPointsVariation(ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, (short)-spellLevl.ApCost);
            Fight.CheckFightEnd();

            if (Fight.Ended)
                return true;
            RefreshStats();
            OnSpellCasted(spellLevl);
            this.Fight.TryEndSequence(1, 100);
            ApplyFighterEvent(FighterEventType.AFTER_USED_AP, spellLevl.ApCost);
            return true;
        }

        public List<Fighter> GetAffectedActors(short[] shape, List<string> targets)
        {
            List<Fighter> results = new List<Fighter>();
            List<Fighter> authorized = new List<Fighter>();

            foreach (var target in targets)
            {
                authorized.AddRange(TargetMaskProvider.Handle(this, target));
            }

            foreach (var target in targets)
            {
                authorized = TargetMaskValidator.Valid(this, target, authorized);
            }

            foreach (var cell in shape)
            {
                var fighter = Fight.GetFighter(cell);
                if (fighter != null && authorized.Contains(fighter))
                    results.Add(fighter);
            }
            if (targets.Contains("C") && !results.Contains(this))
                results.Add(this);
            var t = results.Distinct().ToList();
            return t;
        }
        public virtual void OnSpellCastFailed(CastFailedReason reason, SpellLevelRecord spelllevel) { }

        public virtual void OnSpellCasted(SpellLevelRecord spelllevel) { }

        public bool CanCast(short targetedcell, SpellLevelRecord level, Fighter targetedFighter)
        {
            if (level.CastInLine)
            {
                if (targetedcell == this.CellId && level.MinRange == 0)
                    return true;
                var direction = ShapesProvider.GetDirectionFromTwoCells(this.CellId, targetedcell);
                if (direction == DirectionsEnum.DIRECTION_EAST || direction == DirectionsEnum.DIRECTION_NORTH
                    || direction == DirectionsEnum.DIRECTION_SOUTH || direction == DirectionsEnum.DIRECTION_WEST)
                {
                    OnSpellCastFailed(CastFailedReason.RANGE, level);
                    return false;
                }
                var line = ShapesProvider.GetLineFromDirection(CellId, level.MaxRange, direction);

                if (line.Contains(targetedFighter.CellId))
                    return true;
                else
                {
                    OnSpellCastFailed(CastFailedReason.RANGE, level);
                    return false;
                }
            }
            int Distance = PathHelper.GetDistanceBetween(targetedcell, this.CellId);
            if (Distance > level.MaxRange + FighterStats.Stats._Range || Distance < level.MinRange)
            {
                OnSpellCastFailed(CastFailedReason.RANGE, level);
                return false;
            }
            return true;
        }
        public virtual bool UseWeapon(short cellid) { return false; }

        public virtual bool CastSpellOnTarget(ushort spellid, int targetid)
        {
            var target = Fight.GetFighter(targetid);
            var spell = GetSpellLevel(spellid);
            if (Fight.Ended)
            {
                OnSpellCastFailed(CastFailedReason.FIGHT_ENDED, spell);
                return false;
            }
            if (target == null)
                return false;
            if (!CanCast(target.CellId, spell, target))
            {
                return false;
            }
            return CastSpellOnCell(spellid, target.CellId, targetid);

        }
        bool CheckFighterDie()
        {
            if (FighterStats.Stats.LifePoints <= 0)
            {
                FighterStats.Stats.LifePoints = 0;
                Die();
                return true;
            }
            return false;
        }
        public virtual int CalculateErosionDamage(int damages)
        {
            int num = FighterStats.ErosionPercentage;
            if (num > 50)
            {
                num = 50;
            }
            return (int)((double)damages * ((double)num / 100.0));
        }
        public void LoseLife(TakenDamages damages, int sourceid)
        {
            damages.EvaluateWithResistances(Fight.GetFighter(sourceid), this, Fight.PvP);

            if (damages.Delta <= 0)
                return;

            Fight.TryStartSequence(sourceid, 1);
            if (FighterStats.ShieldPoints > 0)
            {
                if (FighterStats.ShieldPoints - damages.Delta <= 0)
                {
                    ushort rest = (ushort)(damages.Delta - FighterStats.ShieldPoints);
                    Fight.Send(new GameActionFightLifeAndShieldPointsLostMessage((ushort)ActionsEnum.ACTION_CHARACTER_BOOST_SHIELD, sourceid, ContextualId, rest, 0, (ushort)FighterStats.ShieldPoints));
                    FighterStats.ShieldPoints = 0;
                    if (FighterStats.Stats.LifePoints - rest <= 0)
                    {
                        FighterStats.Stats.LifePoints = 0;
                        RefreshStats();
                        Die();


                    }
                    else
                    {
                        ShowFighter();
                        FighterStats.Stats.LifePoints -= (short)rest;
                    }
                    return;
                }
                else
                {
                    Fight.Send(new GameActionFightLifeAndShieldPointsLostMessage(0, sourceid, ContextualId, 0, 0, (ushort)damages.Delta));
                    FighterStats.ShieldPoints -= (int)damages.Delta;
                    ShowFighter();
                    return;
                }
            }
            if (FighterStats.Stats.LifePoints - damages.Delta <= 0)
            {
                Fight.Send(new GameActionFightLifePointsLostMessage(0, sourceid, ContextualId, (ushort)FighterStats.Stats.LifePoints, 0));
                FighterStats.Stats.LifePoints = 0;
                Die();
                return;
            }
            else
            {
                ushort erosionDelta = (ushort)CalculateErosionDamage(damages.Delta);
                FighterStats.RealStats.LifePoints -= (short)erosionDelta;
                FighterStats.ErodedLife += erosionDelta;
                FighterStats.Stats.LifePoints -= damages.Delta;

                Fight.Send(new GameActionFightLifePointsLostMessage(0, sourceid, ContextualId, (ushort)damages.Delta, erosionDelta));

                RefreshStats();
                ApplyFighterEvent(FighterEventType.AFTER_ATTACKED, sourceid, damages);
            }
            Fight.TryEndSequence(1, 103);

        }
        public void AddBuff(Buff buff)
        {
            buff.Initialize(this);
            if (buff.Delay == 0)
                buff.SetBuff();
            Buffs.Add(buff);
            buff.OnBuffAdded();
        }
        public List<BombFighter> GetBombs()
        {
            return Team.GetFighters().OfType<BombFighter>().ToList().FindAll(x => x.Master == this);
        }
        public List<Fighter> GetAliveFighterSummons()
        {
            return Fight.GetAllSummons().FindAll(x => x.FighterStats.SummonerId == ContextualId);
        }
        public List<T> GetAllBuffs<T>(Predicate<T> predicate) where T : Buff
        {
            return Buffs.OfType<T>().ToList().FindAll(predicate);
        }
        public List<T> GetAllBuffs<T>() where T : Buff
        {
            return Buffs.OfType<T>().ToList();
        }
        public void UpdateBuffs()
        {
            Fight.TryStartSequence(ContextualId, 1);

            foreach (var buff in Buffs)
            {
                if (buff.Duration > 0 && buff.Delay == 0)
                    buff.Duration--;

            }
            foreach (var buff in Buffs.FindAll(x => x.Duration == 0))
            {
                buff.RemoveBuff();
            }
            Buffs.RemoveAll(x => x.Duration == 0);
            #region Delay


            for (int i = 0; i < Buffs.Count; i++)
            {

                var buff = Buffs[i];
                if (buff.Delay > 0)
                {
                    buff.Delay--;

                    if (buff.Delay == 0)
                        buff.SetBuff();
                }

            }
            #endregion
            Fight.TryEndSequence(1, 0);

        }
        public void DispellSpell(ushort spellid)
        {
            var buffs = Buffs.FindAll(x => x.SourceSpellId == spellid);
            buffs.ForEach(x => x.RemoveBuff());
            buffs.ForEach(x => Buffs.Remove(x));
        }
        public virtual void Heal(short delta, int sourceid)
        {
            if (HaveState(76)) // state spécial (76) insoignable
                return;
            Fight.TryStartSequence(sourceid, 1);
            if (FighterStats.Stats.LifePoints + delta > FighterStats.RealStats.LifePoints)
            {
                Fight.Send(new GameActionFightLifePointsGainMessage(0, sourceid, ContextualId, (uint)(FighterStats.RealStats.LifePoints - FighterStats.Stats.LifePoints)));
                FighterStats.Stats.LifePoints = FighterStats.RealStats.LifePoints;
            }
            else
            {
                Fight.Send(new GameActionFightLifePointsGainMessage(0, sourceid, ContextualId, (uint)delta));
                FighterStats.Stats.LifePoints += delta;
            }
            Fight.TryEndSequence(1, 0);

        }
        /// <summary>
        /// Dégâts subis =dégâts totaux - résistance fixe - (dégâts totaux / 100/pourcentage de résistance) 
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="sourceid"></param>
        public virtual void TakeDamages(TakenDamages damages, int sourceid)
        {
            if (ApplyFighterEvent(FighterEventType.BEFORE_ATTACKED, sourceid, damages))
                return;
            LoseLife(damages, sourceid);
        }
        public virtual void OnMoved(List<short> cells)
        {

        }
        public virtual void BeforeMove(List<short> cells)
        {

        }
        public bool ApplyFighterEvent(FighterEventType eventtype, object arg1, object arg2 = null, object arg3 = null)
        {
            if (eventtype == FighterEventType.BEFORE_MOVE)
            {
                BeforeMove(arg2 as List<short>);
            }
            if (eventtype == FighterEventType.AFTER_MOVE)
            {
                OnMoved(arg2 as List<short>);
            }
            bool result = false;
            var buffs = Buffs.FindAll(x => x.EventType == eventtype && x.Delay == 0);
            foreach (var buff in buffs)
            {
                if (buff.OnEventCalled(arg1, arg2, arg3))
                    result = true;
            }
            var interactions = Fight.MarkInteractions.FindAll(x => x.EventType == eventtype);
            short fighterCellid = this.CellId; // a remplacer par simplement FighterCellId si problemes Réseaux
            foreach (var interaction in interactions)
            {
                if (interaction.Mark.Cells.Contains(fighterCellid))
                    interaction.Method.Invoke(interaction.Mark, new object[] { this, arg1, arg2, arg3 });
            }
            return result;
        }
        public void AddShieldPoints(uint buffid, short amount, short duration, int sourceid, ushort spellid)
        {
            Fight.TryStartSequence(this.ContextualId, 1);
            FighterStats.ShieldPoints += (int)amount;
            Fight.Send(new GameActionFightDispellableEffectMessage((ushort)ActionsEnum.ACTION_CHARACTER_BOOST_SHIELD, sourceid, new FightTemporaryBoostEffect((uint)buffid,
            this.ContextualId, duration, 1, spellid, (uint)EffectsEnum.Eff_AddShieldPercent, 0, amount)));
            Fight.TryEndSequence(1, 100);

        }
        public bool IsPlaying { get; set; }

        public abstract SpellLevelRecord GetSpellLevel(ushort spellid);
        public abstract int GetInitiative();
        public abstract void RefreshStats();
        public abstract string GetName();

        public short GetSpellBoost(ushort spellid)
        {
            var valuePair = m_buffedSpells.FirstOrDefault(x => x.SpellId == spellid);
            return (short)(valuePair != null ? valuePair.Delta : 0);
        }
        public short CalculateJet(ExtendedSpellEffect record, short statdata)
        {
            short jet = (short)(SpellEffectsHandler.GetRandom(record));
            if (record.BaseEffect.SpellLevelId != 0)
            {
                ushort spellId = SpellLevelRecord.GetLevel(record.BaseEffect.SpellLevelId).SpellId;
                jet += GetSpellBoost(spellId);
            }
            return (short)(Math.Floor((double)jet * (100 + statdata + FighterStats.Stats.AllDamagesBonusPercent) / 100) + FighterStats.Stats.AllDamagesBonus);
        }

        public bool IsAligned(Fighter fighter)
        {
            var direction = ShapesProvider.GetDirectionFromTwoCells(this.CellId, fighter.CellId);
            if (direction == DirectionsEnum.DIRECTION_EAST || direction == DirectionsEnum.DIRECTION_NORTH
                || direction == DirectionsEnum.DIRECTION_SOUTH || direction == DirectionsEnum.DIRECTION_WEST)
                return false;
            else
                return true;

        }

        public void AddToLastPosition(List<short> path)
        {
            foreach (short pos in path) {
                LastPosition.Add(pos);
            }
        }
    }
}