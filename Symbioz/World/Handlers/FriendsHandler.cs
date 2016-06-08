using Symbioz.Auth.Records;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.World.Records;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    public class FriendsHandler
    {
        [MessageHandler]
        public static void HandleFriendsGetListMessage(FriendsGetListMessage message, WorldClient client)
        {
            List<FriendInformations> Friends = new List<FriendInformations>();
            List<string> FriendsAccount = new List<string>();
            foreach (var Friend in client.Character.Friends)
            {
                var characterBase = CharacterRecord.GetCharacterRecordById(Friend.FriendCharacterId);
                var account = AccountsProvider.GetAccountFromDb(characterBase.AccountId);
                var characters = CharacterRecord.GetAccountCharacters(characterBase.AccountId);
                foreach (var character in characters)
                {
                    if (character != null && account != null)
                    {
                        var target = WorldServer.Instance.GetOnlineClient(character.Name);
                        if (target == null && !FriendsAccount.Contains(account.Username))
                        {
                            Friends.Add(new FriendInformations(character.AccountId, account.Username, (sbyte)PlayerStateEnum.NOT_CONNECTED, (ushort)character.LastConnection, 0));
                            FriendsAccount.Add(account.Username);
                        }
                        else if (target != null)
                        {
                            if (target.Character.isFriendWith(client.Character.Record.Id) == true)
                            {
                                BasicGuildInformations guildInfo = null;
                                if (target.Character.HasGuild)
                                {
                                    var guild = target.Character.GetGuild();
                                    guildInfo = new BasicGuildInformations((uint)guild.Id, guild.Name);
                                }
                                else
                                    guildInfo = new BasicGuildInformations(0, "-");
                                var statusPlayer = new PlayerStatus(target.Character.PlayerStatus.statusId);
                                Friends.Add(new FriendOnlineInformations(character.AccountId, account.Username, (sbyte)PlayerStateEnum.UNKNOWN_STATE,
                                    (ushort)character.LastConnection, 0, (uint)character.Id, character.Name, character.Level, 0, character.Breed, character.Sex, guildInfo, 0, statusPlayer));
                            }
                            else
                            {
                                var statusPlayer = new PlayerStatus((sbyte)PlayerStateEnum.UNKNOWN_STATE);
                                Friends.Add(new FriendOnlineInformations(character.AccountId, account.Username, (sbyte)PlayerStateEnum.UNKNOWN_STATE,
                                    (ushort)character.LastConnection, 0, (uint)character.Id, character.Name, 0, 0, character.Breed, character.Sex, new BasicGuildInformations(0, "-"), 0, statusPlayer));

                            }
                        }
                    }
                }
           }
            client.Send(new FriendsListMessage(Friends));
        }

        [MessageHandler]
        public static void HandleFriendAddRequestMessage(FriendAddRequestMessage message, WorldClient client)
        {
            var toAdd = WorldServer.Instance.GetOnlineClient(message.name);
            if (toAdd != null)
            {
                if (!client.Character.AlreadyFriend(toAdd.Character.Record.Id))
                {
                    client.Character.AddFriend(toAdd.Character.Record.Id);
                    client.Character.SaveAllFriends();
                    FriendsHandler.HandleFriendsGetListMessage(new FriendsGetListMessage(), client);
                }
            }
            else
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté !");
        }

        [MessageHandler]
        public static void HandleFriendDeleteRequestMessage(FriendDeleteRequestMessage message, WorldClient client)
        {
            foreach (var Friend in client.Character.Friends)
            {
                var character = CharacterRecord.GetCharacterRecordById(Friend.FriendCharacterId);
                if (character.AccountId == message.accountId)
                {
                    client.Character.RemoveFriend(character.Id);
                    client.Send(new FriendDeleteResultMessage(true, character.Name));
                    FriendsHandler.HandleFriendsGetListMessage(new FriendsGetListMessage(), client);
                    break;
                }
            }
        }

        [MessageHandler]
        public static void HandleFriendSetWarnOnConnectionMessage(FriendSetWarnOnConnectionMessage message, WorldClient client)
        {
            if (client.Character.Record.WarnOnFriendConnection)
            {
                client.Character.Record.WarnOnFriendConnection = false;
                client.Character.Reply("Vous ne recevez plus de notification lorsqu'un de vos ami se connecte !");
            }
            else
            {
                client.Character.Record.WarnOnFriendConnection = true;
                client.Character.Reply("Vous recevez à nouveau des notifications lorsqu'un de vos ami se connecte !");
            }
            //client.Character.Record.WarnOnFriendConnection = (client.Character.Record.WarnOnFriendConnection) ? false : true;
        }
    }
}
