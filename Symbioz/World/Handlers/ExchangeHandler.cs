using Symbioz.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbioz.DofusProtocol.Messages;
using System.Threading.Tasks;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.Enums;
using Symbioz.World.Models;
using Symbioz.World.Records;
using Symbioz.World.Models.Exchanges;

namespace Symbioz.World.Handlers
{
    class ExchangeHandler
    {

        [MessageHandler]
        public static void HandleExchangeBuy(ExchangeBuyMessage message, WorldClient client)
        {

            if (client.Character.Restrictions.cantExchange == true || client.Character.Record.Energy == 0
                || client.Character.Restrictions.isDead == true)
            {
                client.Character.Reply("Impossible car vous êtes mort.");
                return;
            }
            if (client.Character.NpcShopExchange != null)
                client.Character.NpcShopExchange.Buy((ushort)message.objectToBuyId, message.quantity);
            if (client.Character.ShopStockInstance != null)
                client.Character.ShopStockInstance.Buy(message.objectToBuyId, message.quantity);
            if (client.Character.NpcPointsExchange != null)
            {
                client.Character.NpcPointsExchange.Buy((ushort)message.objectToBuyId, message.quantity);
            }

        }
        [MessageHandler]
        public static void HandleCraftSetRecipe(ExchangeSetCraftRecipeMessage message, WorldClient client)
        {
            client.Character.CraftInstance.SetCraftRecipe(message.objectGID);
        }
        [MessageHandler]
        public static void HandleExchangePlayer(ExchangePlayerRequestMessage message, WorldClient client)
        {
            WorldClient target = WorldServer.Instance.GetOnlineClient((int)message.target);
            if (target.Character.Busy)
            {
                client.Character.Reply("Impossible car le joueur est occupé.");
                return;
            }
            else if (target.Character.Restrictions.cantExchange == true || target.Character.Record.Energy == 0
                || target.Character.Restrictions.isDead == true)
            {
                client.Character.Reply("Impossible car le joueur est mort.");
                return;
            }
            else if (client.Character.Restrictions.cantExchange == true || client.Character.Record.Energy == 0
                || target.Character.Restrictions.isDead == true)
            {
                client.Character.Reply("Impossible car vous êtes mort.");
                return;
            }
            else if (target.Character.IsIgnoring(client.Character.Record.AccountId))
            {
                client.Send(new TextInformationMessage(1, 370, new string[1] { target.Character.Record.Name }));
                return;
            }
            var request = new PlayerTradeRequest(client.Character, target.Character);
            client.Character.Request = request;
            target.Character.Request = request;
            request.Open();
        }
        [MessageHandler]
        public static void HandleExchangeReplay(ExchangeReplayMessage message, WorldClient client)
        {
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.CRAFT:
                    client.Character.CraftInstance.SetReplay(message.count);
                    break;
                case ExchangeTypeEnum.RUNES_TRADE:
                    client.Character.SmithMagicInstance.SetReplay(message.count);
                    break;
            }
        }
        [MessageHandler]
        public static void HandleExchangeReplayStop(ExchangeReplayStopMessage message, WorldClient client)
        {
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.CRAFT:
                    client.Character.CraftInstance.ReplayEngine.Stop();
                    break;
                case ExchangeTypeEnum.RUNES_TRADE:
                    client.Character.SmithMagicInstance.ReplayEngine.Stop();
                    break;
            }
        }
        [MessageHandler]
        public static void HandleExchangeAccept(ExchangeAcceptMessage message, WorldClient client)
        {

            if (client.Character.Request != null)
                client.Character.Request.Accept();
        }
        [MessageHandler]
        public static void HandleExchangeObjectMove(ExchangeObjectMoveMessage message, WorldClient client)
        {
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.CRAFT:
                    client.Character.CraftInstance.MoveItem(message.objectUID, message.quantity);
                    break;
                case ExchangeTypeEnum.BIDHOUSE_SELL:
                    client.Character.BidShopInstance.MoveItem(message.objectUID, message.quantity);
                    break;
                case ExchangeTypeEnum.STORAGE:
                    if (client.Character.BankInstance != null)
                    {
                        client.Character.BankInstance.MoveItem(message.objectUID, message.quantity);
                    }
                    if (client.Character.PrismStorageInstance != null)
                    {
                        client.Character.PrismStorageInstance.MoveItem(message.objectUID, message.quantity);
                    }
                    if (client.Character.ShopStockInstance != null)
                    {
                        client.Character.ShopStockInstance.RemoveItem(message.objectUID);
                    }
                    break;
                case ExchangeTypeEnum.RUNES_TRADE:
                    client.Character.SmithMagicInstance.MoveItem(message.objectUID, message.quantity);
                    break;
                case ExchangeTypeEnum.NPC_TRADE:
                    client.Character.NpcTradeExchange.MoveItem(message.objectUID, message.quantity);
                    break;
            }
            if (client.Character.PlayerTradeInstance != null)
                client.Character.Trader.MoveItem(message.objectUID, message.quantity);
        }
        [MessageHandler]
        public static void HandleExchangeReady(ExchangeReadyMessage message, WorldClient client)
        {
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.CRAFT:
                    client.Character.CraftInstance.Ready(message.ready, message.step);
                    break;
                case ExchangeTypeEnum.RUNES_TRADE:
                    client.Character.SmithMagicInstance.Ready(message.ready, message.step);
                    break;
                case ExchangeTypeEnum.NPC_TRADE:
                    client.Character.NpcTradeExchange.Ready(message.ready);
                    break;
            }
            if (client.Character.PlayerTradeInstance != null)
                client.Character.Trader.Ready(message.ready, client.Character.Id);
        }
        [MessageHandler]
        public static void HandleExchangeObjectMoveKamas(ExchangeObjectMoveKamaMessage message, WorldClient client)
        {
            if (client.Character.Record.Kamas < message.quantity)
                return;
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.STORAGE:
                    client.Character.BankInstance.MoveKamas(message.quantity);
                    break;   
            }
            if (client.Character.PlayerTradeInstance != null)
                client.Character.Trader.MoveKamas(message.quantity, client.Character.Id);
        }

    }
}
