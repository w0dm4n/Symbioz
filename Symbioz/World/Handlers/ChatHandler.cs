using Symbioz.Auth.Models;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.World.Handlers;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    class ChatHandler
    {
        public delegate void ChatHandlerDelegate(WorldClient client, string message);
        public static readonly Dictionary<ChatActivableChannelsEnum, ChatHandlerDelegate> ChatHandlers = new Dictionary<ChatActivableChannelsEnum, ChatHandlerDelegate>();
        [StartupInvoke("Chat Channels", StartupInvokeType.Others)]
        public static void Initialize()
        {
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_GLOBAL, ChannelsHandler.Global);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_ADMIN, ChannelsHandler.Admin);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_SALES, ChannelsHandler.Sales);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_SEEK, ChannelsHandler.Seek);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_PARTY, ChannelsHandler.Group);
            ChatHandlers.Add(ChatActivableChannelsEnum.CHANNEL_GUILD, ChannelsHandler.Guild);
        }
        static void Handle(WorldClient client, string message, ChatActivableChannelsEnum channel)
        {
            var handler = ChatHandlers.FirstOrDefault(x => x.Key == channel);
            if (handler.Value != null)
                handler.Value(client, message);
            else
                client.Character.Reply("Ce chat n'est pas géré");
        }
        [MessageHandler]
        public static void HandleChatSmileyRequest(ChatSmileyRequestMessage message, WorldClient client)
        {
            client.Character.DisplaySmiley(message.smileyId);
        }
        [MessageHandler]
        public static void HandleChatClientMultiWithObject(ChatClientMultiWithObjectMessage message, WorldClient client)
        {
           
        }
        [MessageHandler]
        public static void HandleChatMultiClient(ChatClientMultiMessage message, WorldClient client)
        {
            if (message.content.StartsWith(CommandsHandler.CommandsPrefix))
                CommandsHandler.Handle(message.content, client);
            else
                Handle(client, message.content, (ChatActivableChannelsEnum)message.channel);
        }
        [MessageHandler]
        public static void ChatClientPrivate(ChatClientPrivateMessage message, WorldClient client)
        {
            if (message.receiver == client.Character.Record.Name)
                return;
            var target = WorldServer.Instance.GetOnlineClient(message.receiver);
            if (target != null)
            {
                if(target.Character.PlayerStatus.statusId == (sbyte)PlayerStatusEnum.PLAYER_STATUS_PRIVATE)
                {
                    client.Character.ReplyImportant(target.Character.Record.Name + " est actuellement en mode privé");
                    return;
                }else if(target.Character.PlayerStatus.statusId == (sbyte)PlayerStatusEnum.PLAYER_STATUS_SOLO)
                {
                    client.Character.ReplyImportant(target.Character.Record.Name + " est actuellement en mode solo");
                    return;
                }
                target.Send(new ChatServerMessage((sbyte)ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content, 1, client.Character.Record.Name, client.Character.Id, client.Character.Record.Name, client.Account.Id));
                client.Send(new ChatServerCopyMessage((sbyte)ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content, 1, client.Character.Record.Name, (uint)target.Character.Record.Id, target.Character.Record.Name));
                if(target.Character.PlayerStatus.statusId == (sbyte)PlayerStatusEnum.PLAYER_STATUS_AFK)
                {
                    client.Character.ReplyImportant(target.Character.Record.Name + " est asbent");
                    /*if(target.Character.AFKMessage != null)
                    {
                        client.Send(new ChatServerCopyMessage((sbyte)ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, target.Character.AFKMessage, 1, client.Character.Record.Name, (uint)target.Character.Record.Id, target.Character.Record.Name));
                    }*/
                }
            }
            else
            {
                client.Character.Reply("Le personnage n'éxiste pas ou n'est pas connecté");
            }
        }
        public static void SendAnnounceMessage(string value, Color color)
        {
            WorldServer.Instance.SendToOnlineCharacters(new TextInformationMessage(0, 0, new string[] { string.Format("<font color=\"#{0}\">{1}</font>", color.ToArgb().ToString("X"), value) }));
        }
        public static void SendNotificationToAllMessage(string message)
        {
            WorldServer.Instance.SendToOnlineCharacters(new NotificationByServerMessage(24, new string[] { message }, true));
        }

    }
    public class ChannelsHandler
    {
        public static void Seek(WorldClient client, string message)
        {
            sbyte channel = (sbyte)ChatActivableChannelsEnum.CHANNEL_SEEK;
            WorldServer.Instance.SendOnSubarea(new ChatServerMessage(channel, message, 1, "Symbioz", client.Character.Id, client.Character.Record.Name, client.Account.Id), client.Character.SubAreaId);
        }
        public static void Sales(WorldClient client, string message)
        {
            sbyte channel = (sbyte)ChatActivableChannelsEnum.CHANNEL_SALES;
            WorldServer.Instance.SendOnSubarea(new ChatServerMessage(channel, message, 1, "Symbioz", client.Character.Id, client.Character.Record.Name, client.Account.Id), client.Character.SubAreaId);
        }
        public static void Global(WorldClient client, string message)
        {
            sbyte channel = (sbyte)ChatActivableChannelsEnum.CHANNEL_GLOBAL;
            ChatServerMessage chatMessage = new ChatServerMessage(channel, message, 1, "Symbioz", client.Character.Id, client.Character.Record.Name, client.Account.Id);
            if (client.Character.IsFighting)
                client.Character.FighterInstance.Fight.Send(chatMessage);
            else
            {
                if (!client.Character.Map.Instance.Muted)
                    client.Character.SendMap(chatMessage);
                else
                    client.Character.Reply("Impossible d'envoyer le message, la map a été mute.");
            }

        }
        public static void Admin(WorldClient client, string message)
        {
            sbyte channel = (sbyte)ChatActivableChannelsEnum.CHANNEL_ADMIN;
            if (client.Account.Role >= ServerRoleEnum.ADMINISTRATOR)
            {
                WorldServer.Instance.SendToOnlineCharacters(new ChatServerMessage(channel, message, 1, "Symbioz", client.Character.Id, client.Character.Record.Name, client.Account.Id));
            }
            else
                client.Character.Reply("Vous n'avez pas les droits pour utiliser ce chat");
        }
        public static void Group(WorldClient client, string message)
        {
            sbyte channel = (sbyte)ChatActivableChannelsEnum.CHANNEL_PARTY;
            if(client.Character.PartyMember != null && client.Character.PartyMember.Party != null)
            {
                foreach(WorldClient c in client.Character.PartyMember.Party.Members)
                {
                    c.Send(new ChatServerMessage(channel, message, 1, "Symbioz", client.Character.Id, client.Character.Record.Name, client.Account.Id));
                }
            }
        }
        public static void Guild(WorldClient client, string message)
        {
            sbyte channel = (sbyte)ChatActivableChannelsEnum.CHANNEL_GUILD;
            if (client.Character.GetGuild() != null)
            {
                GuildRecord guild = client.Character.GetGuild();
                foreach (WorldClient c in WorldServer.Instance.GetAllClientsOnline().FindAll(x=>x.Character.GetGuild() == guild))
                {
                    c.Send(new ChatServerMessage(channel, message, 1, "Symbioz", client.Character.Id, client.Character.Record.Name, client.Account.Id));
                }
            }
        }
    }
}
