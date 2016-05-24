using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers
{
    public class CustomObjectUseHandler
    {
        public static Dictionary<ushort, Action<WorldClient,CharacterItemRecord>> CustomHandlers = new Dictionary<ushort, Action<WorldClient,CharacterItemRecord>>();

        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            CustomHandlers.Add(14485, Mimicry);
        }
        public static bool CustomHandlerExist(ushort itemgid)
        {
            if (CustomHandlers.ContainsKey(itemgid))
                return true;
            else
                return false;
        }
        public static void Handle(WorldClient client,CharacterItemRecord item)
        {
            var handler = CustomHandlers.FirstOrDefault(x=>x.Key == item.GID);
            handler.Value(client,item);
        }
        static void Mimicry(WorldClient client,CharacterItemRecord item)
        {
            client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            client.Send(new ClientUIOpenedByObjectMessage(3, item.UID));
        }
        
    }
}
