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

namespace Symbioz.World.Handlers
{
    class IgnoredHandler
    {
        [MessageHandler]
        public static void HandleIgnoredGetListMessage(IgnoredGetListMessage message, WorldClient client)
        {
            List<IgnoredInformations> AllIgnoreds = new List<IgnoredInformations>();
            bool isOnline = false;
            foreach (var Ignored in client.Character.IgnoredList)
            {
                isOnline = false;
                var characters = CharacterRecord.GetAccountCharacters(Ignored.IgnoredAccountId);
                var account = AccountsProvider.GetAccountFromDb(Ignored.IgnoredAccountId);
                foreach (var character in characters)
                {
                    var target = WorldServer.Instance.GetOnlineClient(character.Name);
                    if (target != null)
                    {
                        AllIgnoreds.Add(new IgnoredOnlineInformations(account.Id, account.Username, (uint)target.Character.Record.Id, target.Character.Record.Name,
                            target.Character.Record.Breed, target.Character.Record.Sex));
                        isOnline = true;
                    }
                }
                if (isOnline == false)
                    AllIgnoreds.Add(new IgnoredInformations(account.Id, account.Username));
            }
            client.Send(new IgnoredListMessage(AllIgnoreds));
        }

        [MessageHandler]
        public static void HandleIgnoredAddRequestMessage(IgnoredAddRequestMessage message, WorldClient client)
        {
            var toAdd = WorldServer.Instance.GetOnlineClient(message.name);
            if (toAdd != null)
            {
                if (!client.Character.AlreadyIgnored(toAdd.Character.Record.AccountId))
                {
                    client.Character.AddIgnored(toAdd.Character.Record.AccountId);
                    client.Character.SaveAllIgnoreds();
                    IgnoredHandler.HandleIgnoredGetListMessage(new IgnoredGetListMessage(), client);
                }
            }
            else
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté !");
        }

        [MessageHandler]
        public static void HandleIgnoredDeleteRequestMessage(IgnoredDeleteRequestMessage message, WorldClient client)
        {
            foreach (var Ignored in client.Character.IgnoredList)
            {
                var characters = CharacterRecord.GetAccountCharacters(Ignored.IgnoredAccountId);
                foreach (var character in characters)
                {
                    var target = WorldServer.Instance.GetOnlineClient(character.Name);
                    if (target != null)
                    {
                        client.Character.RemoveIgnored(Ignored.IgnoredAccountId);
                        client.Send(new IgnoredDeleteResultMessage(true, true, target.Character.Record.Name));
                        IgnoredHandler.HandleIgnoredGetListMessage(new IgnoredGetListMessage(), client);
                        return;
                    }
                }
            }
        }
    }
}
