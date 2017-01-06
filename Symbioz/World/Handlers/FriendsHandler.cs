using Symbioz.Auth.Records;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.ORM;
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
            SendFriendsList(client);
        }

        [MessageHandler]
        public static void HandleFriendAddRequestMessage(FriendAddRequestMessage message, WorldClient client)
        {
            var toAdd = WorldServer.Instance.GetOnlineClient(message.name);
            if (toAdd != null)
            {
                if (!client.Character.IsFriendWith(toAdd.Character.Record.AccountId))
                {
                    SaveTask.AddElement(client.Character.AddFriend(toAdd.Character.Record.AccountId), client.Character.Id);
                    SendFriendsList(client);
                    //TODO: Failure, Quota
                }
            }
            else
                client.Send(new TextInformationMessage(1, 211, new string[0]));
        }

        [MessageHandler]
        public static void HandleFriendDeleteRequestMessage(FriendDeleteRequestMessage message, WorldClient client)
        {
            if(client.Character.IsFriendWith(message.accountId))
            {
                Singleton<Startup>.Instance.IOTask.AddMessage(new Action(() => {
                    client.Send(new FriendDeleteResultMessage(client.Character.RemoveFriend(message.accountId), client.Character.GetFriendName(message.accountId)));
                SendFriendsList(client);
                }));
            }
        }

        [MessageHandler]
        public static void HandleFriendSetWarnOnConnectionMessage(FriendSetWarnOnConnectionMessage message, WorldClient client)
        {
            client.Account.WarnOnFriendConnection = message.enable;
            AccountsProvider.UpdateAccountsWarningEvent(client.Account);
        }

        [MessageHandler]
        public static void HandleMoodSmileyRequestMessage(MoodSmileyRequestMessage message, WorldClient client)
        {
            if (client.Character.Record.MoodSmileyId != message.smileyId)
            {
                client.Character.Record.MoodSmileyId = message.smileyId;
                SaveTask.UpdateElement(client.Character.Record, client.Character.Id);
            }
        }

        #region Send

        public static void SendFriendsList(WorldClient client)
        {
            Singleton<Startup>.Instance.IOTask.AddMessage(new Action(() => {
                List<FriendInformations> friends = new List<FriendInformations>();
            foreach (var friend in client.Character.Friends)
            {
                var characters = CharacterRecord.GetAccountCharacters(friend.FriendAccountId);
                var account = AccountsProvider.GetAccountFromDb(friend.FriendAccountId);
                foreach (var character in characters)
                {
                    var target = WorldServer.Instance.GetOnlineClient(character.Name);
                    if (target != null)
                    {
                        BasicGuildInformations guildInfo = null;
                        if (target.Character.IsFriendWith(client.Character.Record.AccountId))
                        {
                            if (target.Character.HasGuild)
                            {
                                var guild = target.Character.GetGuild();
                                guildInfo = new BasicGuildInformations((uint)guild.Id, guild.Name);
                            }
                            else
                                guildInfo = new BasicGuildInformations(0, "-");

                            var playerState = PlayerStateEnum.UNKNOWN_STATE;
                            if (target.Character.IsFighting)
                            {
                                playerState = PlayerStateEnum.GAME_TYPE_FIGHT;
                            }
                            else
                                playerState = PlayerStateEnum.GAME_TYPE_ROLEPLAY;

                            friends.Add(new FriendOnlineInformations(character.AccountId, account.Username, (sbyte)playerState,
                                (ushort)character.LastConnection, 0, (uint)character.Id, character.Name, character.Level, character.AlignmentSide, character.Breed, character.Sex, guildInfo,
                                (target.Character.Record.MoodSmileyId != 0) ? (sbyte)target.Character.Record.MoodSmileyId : (sbyte)0, target.Character.PlayerStatus));
                        }
                        else
                        {
                            var statusPlayer = new PlayerStatus((sbyte)PlayerStateEnum.UNKNOWN_STATE);
                            friends.Add(new FriendOnlineInformations(character.AccountId, account.Username, (sbyte)PlayerStateEnum.UNKNOWN_STATE,
                                (ushort)character.LastConnection, 0, (uint)character.Id, character.Name, 0, 0, character.Breed, character.Sex, new BasicGuildInformations(0, "-"), 0, statusPlayer));
                        }
                    }
                    else
                    {
                        friends.Add(new FriendInformations(account.Id, account.Username, (sbyte)PlayerStateEnum.NOT_CONNECTED, 0, 0));
                    }
                }
            }
            client.Send(new FriendsListMessage(friends));
            }));
        }

        #endregion
    }
}
