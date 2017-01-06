using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Records;
using Symbioz.Network.Servers;
using Shader.Helper;
using Symbioz.Helper;
using Symbioz.Network.Messages;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Clients;
using System.Collections.Generic;
using Symbioz.Auth.Records;
using Symbioz.ORM;
using Symbioz.Enums;
using Symbioz.Core.Startup;
using System;

namespace Symbioz.World.Handlers
{
    public class IgnoredHandler
    {
        [MessageHandler]
        public static void HandleIgnoredGetListMessage(IgnoredGetListMessage message, WorldClient client)
        {
            SendIgnoredList(client);
        }

        [MessageHandler]
        public static void HandleIgnoredAddRequestMessage(IgnoredAddRequestMessage message, WorldClient client)
        {
            var toAdd = WorldServer.Instance.GetOnlineClient(message.name);
            if (toAdd != null)
            {
                int accountId = toAdd.Character.Record.AccountId;
                if (!message.session)
                {
                    if (!client.Character.AlreadyEnemy(accountId))
                    {
                        SaveTask.AddElement(client.Character.AddEnemy(accountId), client.Character.Id);
                        SendIgnoredList(client);
                        //TODO: Failure, Quota
                    }
                }
                else
                {
                    if(!client.Character.AlreadyIgnored(accountId))
                    {
                        if(client.Character.AddIgnored(accountId))
                        {
                            client.Character.Reply("Ce joueur a été ignoré pour la session.");
                            client.Send(new IgnoredAddedMessage(new IgnoredInformations(accountId, client.Character.GetIgnoredName(accountId)), true));
                        }
                        else
                        {
                            client.Send(new IgnoredAddFailureMessage((sbyte)ListAddFailureEnum.LIST_ADD_FAILURE_UNKNOWN));
                        }
                    }
                }
            }
            else
                client.Send(new TextInformationMessage(1, 211, new string[0]));
        }

        [MessageHandler]
        public static void HandleIgnoredDeleteRequestMessage(IgnoredDeleteRequestMessage message, WorldClient client)
        {
            Singleton<Startup>.Instance.IOTask.AddMessage(new Action(() => {
                if (!message.session)
            {
                if(client.Character.AlreadyEnemy(message.accountId))
                {
                    string name = client.Character.GetEnemyName(message.accountId);
                    bool success = client.Character.RemoveEnemy(message.accountId);
                    client.Send(new IgnoredDeleteResultMessage(success, false, name));
                    SendIgnoredList(client);
                }
            }
            else
            {
                if(client.Character.AlreadyIgnored(message.accountId))
                {
                    string name = client.Character.GetIgnoredName(message.accountId);
                    bool success = client.Character.RemoveIgnored(message.accountId);
                    client.Send(new IgnoredDeleteResultMessage(success, true, name));
                }
            }
            }));
        }

        #region Send

        public static void SendIgnoredList(WorldClient client)
        {
            Singleton<Startup>.Instance.IOTask.AddMessage(new Action(() => {
                List<IgnoredInformations> allIgnoreds = new List<IgnoredInformations>();
            foreach (var ignored in client.Character.Enemies)
            {
                var characters = CharacterRecord.GetAccountCharacters(ignored.IgnoredAccountId);
                var account = AccountsProvider.GetAccountFromDb(ignored.IgnoredAccountId);
                foreach (var character in characters)
                {
                    var target = WorldServer.Instance.GetOnlineClient(character.Name);
                    if (target != null)
                    {
                        allIgnoreds.Add(new IgnoredOnlineInformations(account.Id, account.Username, (uint)target.Character.Record.Id, target.Character.Record.Name,
                            target.Character.Record.Breed, target.Character.Record.Sex));
                    }
                    else
                    {
                        allIgnoreds.Add(new IgnoredInformations(account.Id, account.Username));
                    }
                }
            }
            client.Send(new IgnoredListMessage(allIgnoreds));
            }));
        }

        #endregion
    }
}
