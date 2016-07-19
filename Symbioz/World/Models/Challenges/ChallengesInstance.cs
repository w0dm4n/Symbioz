using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Challenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Challenges
{
    public class ChallengesInstance
    {
        public Fight CurrentFight { get; set; }

        public List<ChallengesRecord> CurrentChallenges = new List<ChallengesRecord>();

        public void GetRandomChallenges()
        {
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
                challengesNumber--;
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

        public void CancelChallenges()
        {
            this.CurrentFight.ChallengesInstance = null;
        }
    }
}
