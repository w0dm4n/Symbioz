using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Helper;
using Symbioz.RawData.RawMessages;
using Symbioz.ORM;
using Symbioz.Network.Servers;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Models.Alliances;

namespace Symbioz.RawData
{
    public class RawDatasHandler
    {
        static Type[] HandlerMethodParameterTypes = new Type[] { typeof(DofusClient), typeof(RawMessage) };

        private static Dictionary<short, Delegate> Handlers = new Dictionary<short, Delegate>();

        [MessageHandler]
        public static void HandleRawDataMessage(RawDataMessage message, DofusClient client)
        {
            BigEndianReader reader = new BigEndianReader(message._content);

            short rawMessageId = reader.ReadShort();
            var handler = Handlers.FirstOrDefault(x => x.Key == rawMessageId);
            var rawMessage = RawReceiver.BuildMessage(rawMessageId, reader);
            if (handler.Value != null)
            {
                handler.Value.DynamicInvoke(null, client, rawMessage);
            }
        }

        [StartupInvoke("RawHandlers", StartupInvokeType.Others)]
        public static void Initialize()
        {
            foreach (var method in typeof(RawDatasHandler).GetMethods())
            {
                var attribute = method.GetCustomAttribute(typeof(RawHandlerAttribute)) as RawHandlerAttribute;
                if (attribute != null)
                {
                    Handlers.Add(attribute.RawMessageId, method.CreateDelegate(HandlerMethodParameterTypes));
                }
            }
        }

        [RawHandlerAttribute(MotdGuildMessage.Id)]
        public static void HandleMotdGuildMessage(DofusClient client, MotdGuildMessage message)
        {
            var worldClient = WorldServer.Instance.GetOnlineClientByAccountId(client.Account.Id);
            if (worldClient != null)
            {
                if (worldClient.Character.HasGuild && GuildProvider.IsLeader(worldClient.Character.Id, worldClient.Character.GetGuild().Id))
                {
                    if (!string.IsNullOrEmpty(message.Message))
                    {
                        worldClient.Character.GetGuild().GuildWelcomeMessage = message.Message;
                        worldClient.Character.Reply("Vous avez modifié le message d'accueil de votre guilde.");
                    }
                }
            }
        }

        [RawHandlerAttribute(MotdAllianceMessage.Id)]
        public static void HandleMotdAllianceMessage(DofusClient client, MotdAllianceMessage message)
        {
            var worldClient = WorldServer.Instance.GetOnlineClientByAccountId(client.Account.Id);
            if (worldClient != null)
            {
                if (worldClient.Character.HasAlliance && worldClient.Character.HasGuild && AllianceProvider.IsLeader(worldClient.Character.Id, worldClient.Character.GuildId, worldClient.Character.AllianceId))
                {
                    if (!string.IsNullOrEmpty(message.Message))
                    {
                        worldClient.Character.GetAlliance().AllianceWelcomeMessage = message.Message;
                        worldClient.Character.Reply("Vous avez modifié le message d'accueil de votre alliance.");
                    }
                }
            }
        }
    }
}
