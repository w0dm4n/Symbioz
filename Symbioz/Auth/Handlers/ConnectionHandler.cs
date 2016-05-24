using Symbioz.Auth.Models;
using Symbioz.Auth.Records;
using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Messages;
using Symbioz.Network.Servers;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Auth.Handlers
{
    class ConnectionHandler
    {
        public static object LockObject = new object();

        [MessageHandler]
        public static void HandleIdentificationMessage(IdentificationMessage message,AuthClient client)
        {
            lock (LockObject)
            {
                Account account = AccountsProvider.DecryptCredentialsFromClient(client, message.sessionOptionalSalt.ToString(), message.credentials);
                Account theoricalAccount = AccountsProvider.GetAccountFromDb(account.Username);
                if (theoricalAccount == null)
                {
                    client.Send(new IdentificationFailedMessage((sbyte)IdentificationFailureReasonEnum.WRONG_CREDENTIALS));
                    return;
                }
                if (theoricalAccount.Password == null || theoricalAccount.Password != account.Password)
                {
                    client.Send(new IdentificationFailedMessage((sbyte)IdentificationFailureReasonEnum.WRONG_CREDENTIALS));
                    return;
                }
                if (theoricalAccount.Banned || BanIpRecord.IsBanned(client.SSyncClient.Ip.Split(':')[0]))
                {
                    client.Send(new IdentificationFailedMessage((sbyte)IdentificationFailureReasonEnum.BANNED));
                    return;
                }
                client.Account = theoricalAccount;
                AccountInformationsRecord.CheckAccountInformations(client.Account.Id, ConfigurationManager.Instance.StartBankKamas);
                if (client.Account.Nickname == string.Empty || client.Account.Nickname == null)
                {
                    client.Send(new NicknameRegistrationMessage());
                    return;
                }
                Login(client, message.autoconnect);
            }
          
        }
        [MessageHandler]
        public static void HandleNicknameChoice(NicknameChoiceRequestMessage message, AuthClient client)
        {
            if (client.Account.Username == message.nickname)
            {
                client.Send(new NicknameRefusedMessage((sbyte)NicknameErrorEnum.SAME_AS_LOGIN));
                return;
            }
            if (message.nickname == string.Empty || message.nickname.Length >= 15 || message.nickname.Contains('\''))
            {
                client.Send(new NicknameRefusedMessage((sbyte)NicknameErrorEnum.INVALID_NICK));
                return;
            }
            if (AccountsProvider.CheckAndApplyNickname(client, message.nickname))
            {
                client.Send(new NicknameAcceptedMessage());
                Login(client, false);
            }
            else
                client.Send(new NicknameRefusedMessage((sbyte)NicknameErrorEnum.ALREADY_USED));
        }
        private static void Login(AuthClient client, bool autoconnect)
        {   
            ServersManager.DisconnectAlreadyConnectedClient(client,client.Account.Id);
            AuthServer.Instance.AuthClients.Add(client);
            bool hasRights = false;
            if (client.Account.Role > ServerRoleEnum.PLAYER)
                hasRights = true;
            client.Send(new IdentificationSuccessMessage(hasRights, false, client.Account.Username, client.Account.Nickname, client.Account.Id, 0, "Supprimer ce personnage?", 2, 1, 0));
            var characters = CharacterRecord.GetAccountCharacters(client.Account.Id);
            if (!autoconnect)
            {
                SendServerListMessage(client, characters.Count);
                return;
            }
            else
            {
                if (WorldServer.Instance.ServerState == ServerStatusEnum.ONLINE)
                SelectedServer(null, client);
                else
                    SendServerListMessage(client, characters.Count);
            }
        }
        [MessageHandler]
        public static void SelectedServer(ServerSelectionMessage message,AuthClient client)
        {
            string host = ConfigurationManager.Instance.IsCustomHost == true ? ConfigurationManager.Instance.RealHost : ConfigurationManager.Instance.Host;
            var encrypted = AccountsProvider.EncryptTicket(ServersManager.TransfertToGame(client.Account),client.AesKey);
            client.Send(new SelectedServerDataMessage((ushort)ConfigurationManager.Instance.ServerId, host, (ushort)ConfigurationManager.Instance.WorldPort, true, encrypted));
            client.Disconnect();
        }
        public static void SendServerListMessage(DofusClient client, int charactercount)
        {
            var servers = new List<GameServerInformations>();
            servers.Add(new GameServerInformations((ushort)ConfigurationManager.Instance.ServerId, (sbyte)WorldServer.Instance.ServerState,0, true, (sbyte)charactercount, 1));
            client.Send(new ServersListMessage(servers, 0, true));
        }
        public static void SendSystemMessage(DofusClient client, string message)
        {
            client.Send(new SystemMessageDisplayMessage(true, 61, new List<string> { message }));
        }
        [MessageHandler]
        public static void HandleReloginToken(ReloginTokenRequestMessage message, WorldClient client)
        {
            client.Send(new ReloginTokenStatusMessage(false, string.Empty));
        }
    }
}
