using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Challenges
{
    [Table("Challenges", true)]
    public class ChallengesRecord : ITable
    {
        public static List<ChallengesRecord> Challenges = new List<ChallengesRecord>();

        [Primary]
        public int Id;
        public int ChallengeId;
        public string ChallengeName;
        public int ChallengeXpBonus;
        public int ChallengeDropBonus;

        public ChallengesRecord(int id, int challengeId, string challengeName, int challengeXpBonus, int challengeDropBonus)
        {
            this.Id = id;
            this.ChallengeId = challengeId;
            this.ChallengeName = challengeName;
            this.ChallengeXpBonus = challengeXpBonus;
            this.ChallengeDropBonus = challengeDropBonus;
        }

        public static ChallengesRecord GetRandomChallenge()
        {
            Random rand = new Random();
            int challengesNumber = rand.Next(1, (ChallengesRecord.Challenges.Count() + 1));
            Logger.Log(challengesNumber);
            int i = 1;
            foreach (var challenge in ChallengesRecord.Challenges)
            {
                if (i == challengesNumber)
                    return (challenge);
                i++;
            }
            return (null);
        }
    }
}
