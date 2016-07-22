using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Challenges;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.World.Models.Challenges
{
    public class ChallengesInstance
    {
        public Fight CurrentFight { get; set; }

        public List<ChallengesRecord> CurrentChallenges = new List<ChallengesRecord>();

        public List<MonsterFighter> ToFocus = new List<MonsterFighter>();

        public void GetRandomChallenges()
        {
            int infinite = 0;
            Random rand = new Random();
            int challengesNumber = rand.Next(1, 3);
            ChallengesRecord currentChallenge = null;
            while (challengesNumber > 0)
            {
                if ((currentChallenge = ChallengesRecord.GetRandomChallenge()) != null)
                {
                    if (!CurrentChallenges.Contains(currentChallenge))
                        CurrentChallenges.Add(currentChallenge);
                    else
                        challengesNumber++;
                    currentChallenge = null;
                }
                if (infinite > 100000) // in case that there is only one chall in database avoid an infinite while
                    break;
                challengesNumber--;
                infinite++;
            }
            if (this.CurrentChallenges.Count() == 0)
                this.CancelChallenges();
        }

        public void SendCurrentChallenge(WorldClient client)
        {
            foreach (var challenge in CurrentChallenges)
                client.Send(new ChallengeInfoMessage((ushort)challenge.ChallengeId, client.CharacterId, (uint)challenge.ChallengeXpBonus, (uint)challenge.ChallengeDropBonus));
        }

        public void SendAllFighterChallengeMessage()
        {
            var charactersFighter = this.CurrentFight.GetAllCharacterFighters();
            foreach (var character in charactersFighter)
            {
                if (character.Client != null)
                    this.SendCurrentChallenge(character.Client);
            }
        }

        public ChallengesInstance(Fight currentFight)
        {
            this.CurrentFight = currentFight;
            this.GetRandomChallenges();
            if (CurrentChallenges.Count() != 0)
            {
                this.SendAllFighterChallengeMessage();
            }
            else
                this.CancelChallenges();
        }

        public void SendResultMessage(WorldClient client)
        {
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == true)
                    client.Send(new ChallengeResultMessage((ushort)challenge.ChallengeId, true));
                else
                    client.Send(new ChallengeResultMessage((ushort)challenge.ChallengeId, false));
            }
        }

        public double GetDropBonus(double currentRate)
        {
            double bonus = 0;
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == true)
                    bonus += challenge.ChallengeDropBonus;
            }
            bonus = (bonus / 100) * 2;
            if (bonus > 1.0)
                return currentRate * bonus;
            else
                return currentRate;
        }

        public double GetXpBonus(double currentRate)
        {
            double bonus = 0;
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == true)
                    bonus += challenge.ChallengeXpBonus;
            }
            bonus = (bonus / 100) * 2;
            if (bonus > 1.0)
                return currentRate * bonus;
            else
                return currentRate;
        }

        static void SendMessageFailure(Fight CurrentFight, ChallengesRecord challenge, Fighter dueTo, System.Timers.Timer Timer)
        {
            var characters = CurrentFight.GetAllCharacterFighters(true);
            foreach (var character in characters)
            {
                character.Client.Send(new ChallengeResultMessage((ushort)challenge.ChallengeId, challenge.ChallengeSuccess));
                character.Client.Character.Reply("<b>" + dueTo.GetName() + "</b>" + " a fait échouer le challenge " + challenge.ChallengeName + ".");
            }
            Timer.Enabled = false;
        }

        public void ChallengeFailure(ChallengesRecord challenge, Fighter dueTo)
        {
            foreach (var tmp in CurrentChallenges)
            {
                if (tmp == challenge)
                {
                    challenge.ChallengeSuccess = false;
                    break;
                }
            }
            var Timer = new System.Timers.Timer();
            Timer.Interval = 2000;
            Timer.Elapsed += (sender, e) => { SendMessageFailure(this.CurrentFight, challenge, dueTo, Timer); };
            Timer.Enabled = true;
        }

        public int GetActorShieldPoints(List<Fighter> actors, Fighter fighter, int[] actorsShieldPoints)
        {
            int i = 0;
            foreach (var actor in actors)
            {
                if (actor == fighter)
                    return (actorsShieldPoints[i]);
                i++;
            }
            return (0);
        }

        public void HandleWeaponUse(CharacterFighter fighter)
        {
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == false)
                    continue;
                switch (challenge.ChallengeId)
                {
                    case 11: // Mystique - utiliser uniquement des sorts pendant toute la durée du combat.
                        this.ChallengeFailure(challenge, fighter);
                    break;

                    case 9: // Barbare - Occasionner des dommages avec une arme sur alliés ou ennemis, à chaque tour de jeu.
                        fighter.WeaponUsedOnLastTurn = true;
                    break;
                }
            }
        }

        public void HandleDeath(Fighter fighter)
        {
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == false)
                    continue;
                switch (challenge.ChallengeId)
                {
                    case 33: // Survivant - Tous les alliés doivent terminer le combat vivants.
                        this.ChallengeFailure(challenge, fighter);
                    break;
                }
            }
        }

        public void HandleMonsterDeath(Fighter fighter)
        {
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == false)
                    continue;
                switch (challenge.ChallengeId)
                {
                    case 31: // Focus - Lorsqu'un adversaire est attaqué, il doit être achevé avant qu'un autre adversaire soit attaqué..
                        foreach (var tmp in this.ToFocus)
                        {
                            if (tmp == fighter)
                            {
                                this.ToFocus.Remove(tmp);
                                break;
                            }
                        }
                    break;
                }
            }
        }

        public void HandleEndTurn(CharacterFighter fighter)
        {
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == false)
                    continue;
                switch (challenge.ChallengeId)
                {
                    case 41: // Pétulant - Utiliser tous les points d'action disponibles avant la fin de son tour de jeu.
                        if (fighter.FighterStats.Stats.ActionPoints != 0)
                            this.ChallengeFailure(challenge, fighter);
                    break;

                    case 8: // Nomade - Utiliser tous ses PM disponibles à chaque tour et ne pas se faire tacler pendant toute la durée du combat.
                        if (fighter.FighterStats.Stats.MovementPoints != 0)
                            this.ChallengeFailure(challenge, fighter);
                    break;

                    case 1: //Zombie - Utiliser un seul point de mouvement par tour de jeu.
                        if ((fighter.FighterStats.RealStats.MovementPoints - 1) > 0)
                        {
                            if (fighter.FighterStats.Stats.MovementPoints != (fighter.FighterStats.RealStats.MovementPoints - 1))
                                this.ChallengeFailure(challenge, fighter);
                        }
                        else
                        {
                            if (fighter.FighterStats.Stats.MovementPoints != 0)
                                this.ChallengeFailure(challenge, fighter);
                        }
                    break;

                    case 6: // Versatile - Chaque joueur n'a le droit d'effectuer qu'une seule fois une même action pendant son tour de jeu.
                        if (fighter.CastedOnTurn != null)
                            fighter.CastedOnTurn.Clear();
                    break;

                    case 9:  // Barbare - Occasionner des dommages avec une arme sur alliés ou ennemis, à chaque tour de jeu.
                        if (fighter.WeaponUsedOnLastTurn == false)
                            this.ChallengeFailure(challenge, fighter);
                        else
                            fighter.WeaponUsedOnLastTurn = false;
                    break;
                }
            }
        }

        public void HandleSpellLaunch(Fighter fighter, SpellLevelRecord spell)
        {
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == false)
                    continue;
                switch (challenge.ChallengeId)
                {
                    case 5: // Econome - Tous les personnages ne doivent utiliser qu'une seule fois la même action durant toute la durée du combat.
                        if (fighter.CastedOnFight.Contains(spell))
                            this.ChallengeFailure(challenge, fighter);
                        else
                            fighter.CastedOnFight.Add(spell);
                    break;

                    case 6: // Versatile - Chaque joueur n'a le droit d'effectuer qu'une seule fois une même action pendant son tour de jeu.
                        if (fighter.CastedOnTurn != null)
                        {
                            if (fighter.CastedOnTurn.Contains(spell))
                                this.ChallengeFailure(challenge, fighter);
                            else
                                fighter.CastedOnTurn.Add(spell);
                        }
                    break;
                }
            }
        }

        public void HandleSpellCast(Fighter fighter, List<Fighter> actors, ExtendedSpellEffect effect, int[] actorsShieldPoints)
        {
            /*if (fighter is MonsterFighter)
                Logger.Log("Monster just cast a spell !");
            else if (fighter is CharacterFighter)
                Logger.Log("A player just cast a spell !");
            */
            foreach (var challenge in CurrentChallenges)
            {
                if (challenge.ChallengeSuccess == false)
                    continue;
                switch (challenge.ChallengeId)
                {
                    case 17: // Intouchable - ne pas perdre de points de vie/points de bouclier pendant toute la durée du combat.
                        foreach (var actor in actors)
                        {
                            if (actor is CharacterFighter)
                            {
                                var actorShieldPoint = GetActorShieldPoints(actors, actor, actorsShieldPoints);
                                if (actorShieldPoint != 0)
                                {
                                    if (actor.FighterStats.ShieldPoints != actorShieldPoint)
                                    {
                                        this.ChallengeFailure(challenge, actor); 
                                        break;
                                    }
                                }
                                if ((actor.FighterStats.RealStats.LifePoints != actor.FighterStats.Stats.LifePoints))
                                {
                                    this.ChallengeFailure(challenge, actor);
                                    break;
                                }
                            }
                        }

                        /*var Fighters = CurrentFight.GetAllCharacterFighters();
                        foreach (var tmp in Fighters)
                        {
                            if (actors.Contains(tmp))
                            {
                                switch (effect.BaseEffect.EffectType)
                                {
                                    case EffectsEnum.Eff_DamagePercentWater:
                                    case EffectsEnum.Eff_DamagePercentEarth:
                                    case EffectsEnum.Eff_DamagePercentAir:
                                    case EffectsEnum.Eff_DamagePercentFire:
                                    case EffectsEnum.Eff_DamagePercentNeutral:
                                    case EffectsEnum.Eff_DamageWater:
                                    case EffectsEnum.Eff_DamageEarth:
                                    case EffectsEnum.Eff_DamageAir:
                                    case EffectsEnum.Eff_DamageFire:
                                    case EffectsEnum.Eff_DamageNeutral:
                                        Logger.Log("Challenge failed !");
                                    break;
                                }
                                Logger.Log("Fighter hit by one spell : " + effect.BaseEffect.Value);
                            }
                        }*/
                    break;

                    case 18: // Incurable - Ne pas regagner de points de vie pendant toute la durée du combat.
                        if (fighter is MonsterFighter)
                            return;
                        foreach (var actor in actors)
                        {
                            if (actor is CharacterFighter)
                            {
                                if (effect.BaseEffect.EffectType == EffectsEnum.Eff_HealHP_108)
                                {
                                    this.ChallengeFailure(challenge, actor);
                                    break;
                                }
                            }
                        }
                    break;

                    case 20: // Elementaire - Utiliser le même élément d'attaque pendant toute la durée du combat.
                        if (fighter is MonsterFighter)
                            return;
                        if (fighter.LastElementUse.Count() == 0)
                        {
                            switch (effect.BaseEffect.EffectType)
                            {
                                case EffectsEnum.Eff_DamagePercentWater:
                                case EffectsEnum.Eff_DamagePercentEarth:
                                case EffectsEnum.Eff_DamagePercentAir:
                                case EffectsEnum.Eff_DamagePercentFire:
                                case EffectsEnum.Eff_DamagePercentNeutral:
                                case EffectsEnum.Eff_DamageWater:
                                case EffectsEnum.Eff_DamageEarth:
                                case EffectsEnum.Eff_DamageAir:
                                case EffectsEnum.Eff_DamageFire:
                                case EffectsEnum.Eff_DamageNeutral:
                                    fighter.LastElementUse.Add(effect.BaseEffect.EffectType);
                                break;
                            }
                        }
                        else
                        {
                            switch (effect.BaseEffect.EffectType)
                            {
                                case EffectsEnum.Eff_DamagePercentWater:
                                case EffectsEnum.Eff_DamagePercentEarth:
                                case EffectsEnum.Eff_DamagePercentAir:
                                case EffectsEnum.Eff_DamagePercentFire:
                                case EffectsEnum.Eff_DamagePercentNeutral:
                                case EffectsEnum.Eff_DamageWater:
                                case EffectsEnum.Eff_DamageEarth:
                                case EffectsEnum.Eff_DamageAir:
                                case EffectsEnum.Eff_DamageFire:
                                case EffectsEnum.Eff_DamageNeutral:
                                    if (!fighter.LastElementUse.Contains(effect.BaseEffect.EffectType))
                                        this.ChallengeFailure(challenge, fighter);
                                break;
                            }
                        }
                    break;

                    case 21: // Circulez - Ne pas retirer de PM aux adversaires pendant toute la durée du combat.
                        if (fighter is MonsterFighter)
                            return;
                        if (effect.BaseEffect.EffectType == EffectsEnum.Eff_LostMP)
                            this.ChallengeFailure(challenge, fighter);
                    break;

                    case 22: // Le temps qui court - Ne pas retirer de PA aux adversaires pendant toute la durée du combat.
                        if (fighter is MonsterFighter)
                            return;
                        if (effect.BaseEffect.EffectType == EffectsEnum.Eff_RemoveAP)
                            this.ChallengeFailure(challenge, fighter);
                    break;

                    case 23: // Perdu de vue - Ne pas retirer de portée aux adversaires pendant toute la durée du combat.
                        if (fighter is MonsterFighter)
                            return;
                        if (effect.BaseEffect.EffectType == EffectsEnum.Eff_SubRange)
                            this.ChallengeFailure(challenge, fighter);
                    break;

                    case 31: // Focus - Lorsqu'un adversaire est attaqué, il doit être achevé avant qu'un autre adversaire soit attaqué.
                        if (fighter is MonsterFighter)
                            return;
                        foreach (var actor in actors)
                        {
                            if (actor is MonsterFighter)
                            {
                                if (this.ToFocus.Count() == 0)
                                    this.ToFocus.Add((MonsterFighter)actor);
                                else
                                {
                                    if (!this.ToFocus.Contains(actor))
                                        this.ChallengeFailure(challenge, fighter);
                                }
                            }
                        }
                    break;

                    case 32: // Elitiste - Toutes les attaques doivent être concentrées sur un adversaire jusqu'à ce qu'il meurt.

                    break;
                }
            }
        }

        public void CancelChallenges()
        {
            this.CurrentFight.ChallengesInstance = null;
        }
    }
}
