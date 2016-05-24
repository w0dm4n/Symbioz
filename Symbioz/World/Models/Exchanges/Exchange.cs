using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges
{
    public abstract class Exchange 
    {
        public virtual ExchangeTypeEnum ExchangeType { get; set; }
        public abstract void MoveItem(uint uid, int quantity);
        public abstract void Ready(bool ready, ushort step);
        public abstract void CancelExchange();
        public virtual void AddItemToPanel(CharacterItemRecord obj, uint quantity) { }

        public virtual void RemoveItemFromPanel(CharacterItemRecord obj, int quantity) { }

        public static List<WorldClient> GetOnlineClientsExchanging(ExchangeTypeEnum exchange)
        {
            return WorldServer.Instance.GetAllClientsOnline().FindAll(x => x.Character != null && x.Character.ExchangeType != null && x.Character.ExchangeType == exchange);
        }

    }
}
