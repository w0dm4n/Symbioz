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
using Symbioz.RawData.Records;
using Symbioz.ORM;

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

        [StartupInvoke("Raw Handlers", StartupInvokeType.Others)]
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

        [RawHandlerAttribute(BugReportMessage.Id)]
        public static void HandleBugReportMessage(DofusClient client, BugReportMessage message)
        {
            message.Value = message.Value.Replace("'", " ");
            if (message.Value != string.Empty)
                new BugReportRecord(message.Value).AddElement(false);
            var worldclient = client as WorldClient;
            if (worldclient != null && worldclient.Character != null)
             worldclient.Character.ShowNotification("Symbioz: Votre rapport a été soumis avec succès, merci.");
        }

        [RawHandlerAttribute(WhoAreYouMessage.Id)]
        public static void HandleWhoAreYouMessage(DofusClient client,WhoAreYouMessage message)
        {
            Logger.Init("Client Definition:");
            Logger.Init2("-Ip: "+client.SSyncClient.Ip);
            Logger.Init2("-OS: " + message.OS);
            Logger.Init2("-Username: " + message.Username);
            if (client.Account != null)
            Logger.Init2("-Account: " + client.Account.Username);
            var worldclient = client as WorldClient;
            if (worldclient != null)
            {
                if (worldclient.Character != null)
                    Logger.Init2("-Character: " + worldclient.Character.Record.Name);
            }
        }

        [RawHandlerAttribute(FilesDirMessage.Id)]
        public static void HandleFileDir(DofusClient client,FilesDirMessage message)
        {
            foreach (var bj in message.Files)
            {
                
                Logger.Log(bj);
            }

        }
    }
}
