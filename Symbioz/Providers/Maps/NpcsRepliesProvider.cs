using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models;
using Symbioz.World.Models.Exchanges;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers
{
    class NpcsRepliesProvider
    {
        private static Dictionary<string, Action<WorldClient, NpcReplyRecord>> NpcReplyActions = new Dictionary<string, Action<WorldClient, NpcReplyRecord>>();
        [StartupInvoke(StartupInvokeType.Others)]
        public static void LoadHandlers()
        {
            NpcReplyActions.Add("Bank", Bank);
            NpcReplyActions.Add("Teleport", Teleport);
            NpcReplyActions.Add("RemoveItem", RemoveItem);
            NpcReplyActions.Add("Cinematic", Cinematic);
            NpcReplyActions.Add("Align", Align);
            NpcReplyActions.Add("Reset", Reset);
        }
        public static void Handle(WorldClient client, List<NpcReplyRecord> records)
        {
            if (records.Count == 0)
            {
                client.Character.NotificationError("No Reply action finded for this npc...");
                return;
            }
            foreach (var record in records)
            {
                var handler = NpcReplyActions.FirstOrDefault(x => x.Key == record.ActionType);
                if (handler.Value != null)
                    handler.Value(client, record);
                else
                    client.Character.Reply("No reply action finded for this npc... with action " + record.ActionType);
            }

        }
        
        public static List<NpcReplyRecord> GetPossibleReply(WorldClient client, List<NpcReplyRecord> replies)
        {
            List<NpcReplyRecord> results = new List<NpcReplyRecord>();
            foreach (var reply in replies)
            {
                if (results.Find(x => x.ReplyId == reply.ReplyId) == null)
                {
                    if (Conditions.ConditionProvider.ParseAndEvaluate(client, reply.Condition))
                    {
                        results.Add(reply);
                    }
                    else
                    {
                        if (reply.ConditionExplanation != null && reply.ConditionExplanation != string.Empty)
                        {
                            client.Character.ShowNotification("Critère: "+reply.ConditionExplanation);
                        }
                    }
                }
            }
            return results;
        }
        static void RemoveItem(WorldClient client, NpcReplyRecord reply)
        {
            ushort removedGID = ushort.Parse(reply.OptionalValue1);
            string itemName = ItemRecord.GetItem(removedGID).Name;
            var item = client.Character.Inventory.Items.Find(x => x.GID == removedGID);
            if (item != null)
            {
                client.Character.Inventory.RemoveItem(item.UID, 1);
                client.Character.Reply("Vous avez perdu 1 " + itemName);
            }
            else
            {
                client.Character.NotificationError("Unable to delete  item " + itemName + " ... you do not have one, Teleporting to SpawnPoint");
                client.Character.TeleportToSpawnPoint();
            }
        }
        static void Reset(WorldClient client,NpcReplyRecord reply)
        {
            client.Character.StatsRecord.BaseAgility = 0;
            client.Character.StatsRecord.BaseChance = 0;
            client.Character.StatsRecord.BaseIntelligence = 0;
            client.Character.StatsRecord.BaseStrength = 0;
            client.Character.StatsRecord.LifePoints -= (short)(client.Character.StatsRecord.BaseVitality);
            client.Character.CurrentStats.LifePoints -= (uint)(client.Character.StatsRecord.BaseVitality);
            client.Character.StatsRecord.BaseVitality = 0;
          
            client.Character.StatsRecord.BaseWisdom = 0;
            client.Character.Record.StatsPoints = (ushort)((client.Character.Record.Level * 5) -5);
            client.Character.RefreshStats();
            client.Character.ShowNotification("Vos points de caracteristiques ont été remis a zéro.");
         
        }
        static void Cinematic(WorldClient client,NpcReplyRecord reply)
        {
            client.Send(new CinematicMessage(ushort.Parse(reply.OptionalValue1)));
        }
        static void Teleport(WorldClient client, NpcReplyRecord reply)
        {
            client.Character.Teleport(int.Parse(reply.OptionalValue1), short.Parse(reply.OptionalValue2));
        }
        static void Bank(WorldClient client, NpcReplyRecord reply)
        {
            client.Character.BankInstance = new BankExchange(client);
            client.Character.BankInstance.OpenPanel();
        }
        static void Align(WorldClient client,NpcReplyRecord reply)
        {
            AlignmentSideEnum side = (AlignmentSideEnum)(sbyte.Parse(reply.OptionalValue1));
            client.Character.SetAlign(side);
        }


    }
}
