using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.World.Models;
using Symbioz.World.Models.Succes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class SuccesHandler
    {
        [MessageHandler]
        public static void AchievementDetailedList(AchievementDetailedListMessage message, WorldClient client)
        {
           // Character c = client.Character;

            //client.Send(new AchievementDetailedListMessage(new List<Achievement>(), new List<Achievement>()));
        }

        [MessageHandler]
        public static void FinishedAchievementMessage(AchievementDetailedListRequestMessage message, WorldClient client)
        {

            List<Achievement> list = new List<Achievement>();

            client.Send(new AchievementRewardSuccessMessage(10));
            list.Add(new Achievement(1, new List<AchievementObjective>(), new List<AchievementStartedObjective>()));
            client.Send(new AchievementDetailedListMessage(list, list));
        }

        [MessageHandler]
        public static void HandleAchivementReward(AchievementRewardRequestMessage message, WorldClient client)
        {

            int wonkamas_ = 0;
            ulong wonxp_ = 0;
            foreach (var reward in client.Character.SuccesShortcuts)
            {
                client.Send(new AchievementRewardSuccessMessage(reward.Id));
                wonkamas_ += reward.KamasWin;
                wonxp_ += (ulong)reward.GainXp;
                if (!client.Character.GetSuccess().Contains(reward.Id.ToString()))
                    client.Character.AddAchievements(reward.Id.ToString());
            }

            wonkamas_ = client.Character.Record.Level * wonkamas_;
            wonxp_ = client.Character.Record.Level * wonxp_;
            client.Character.AddXp(wonxp_);

            client.Character.SendMessage("Vous avez gagné " + wonxp_ + " points d'experience.");
            client.Character.AddKamas(wonkamas_);
            client.Character.SendMessage("Vous avez reçu " + wonkamas_ + " kamas.");
            client.Character.RefreshStats();
            client.Character.SuccesShortcuts.Clear();
        }

        public void SendFinishedAchievementMessage(WorldClient client, short ID, int wonKamas, int wonXp)
        {

            client.Send(new AchievementFinishedMessage((ushort)ID, 0));
            client.Character.SuccesShortcuts.Add(new SuccesRewards { Id = ID, KamasWin = (short)wonKamas, GainXp = (short)wonXp });

        }
    }
}
