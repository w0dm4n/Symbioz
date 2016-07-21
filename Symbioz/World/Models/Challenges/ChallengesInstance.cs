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
                }
            }
        }

        public void CancelChallenges()
        {
            this.CurrentFight.ChallengesInstance = null;
        }
    }
}
