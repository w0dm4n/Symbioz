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

            if (client.Character.NpcShopExchange != null)
                client.Character.NpcShopExchange.Buy((ushort)message.objectToBuyId, message.quantity);

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
            target.Character.PlayerTradeInstance = new PlayerTradeExchange(target, client);
            client.Character.PlayerTradeInstance = new PlayerTradeExchange(client, target);
            client.Character.PlayerTradeInstance.Ask();
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
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    client.Character.PlayerTradeInstance.AcceptExchange();
                    break;
            }

        }
        [MessageHandler]
        public static void HandleExchangeObjectMove(ExchangeObjectMoveMessage message, WorldClient client)
        {
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    client.Character.PlayerTradeInstance.MoveItem(message.objectUID, message.quantity);
                    break;
                case ExchangeTypeEnum.CRAFT:
                    client.Character.CraftInstance.MoveItem(message.objectUID, message.quantity);
                    break;
                case ExchangeTypeEnum.BIDHOUSE_SELL:
                    client.Character.BidShopInstance.MoveItem(message.objectUID, message.quantity);
                    break;
                case ExchangeTypeEnum.STORAGE:
                    client.Character.BankInstance.MoveItem(message.objectUID, message.quantity);
                    break;
                case ExchangeTypeEnum.RUNES_TRADE:
                    client.Character.SmithMagicInstance.MoveItem(message.objectUID, message.quantity);
                    break;
            }

        }
        [MessageHandler]
        public static void HandleExchangeReady(ExchangeReadyMessage message, WorldClient client)
        {
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    client.Character.PlayerTradeInstance.Ready(message.ready, message.step);
                    break;
                case ExchangeTypeEnum.CRAFT:
                    client.Character.CraftInstance.Ready(message.ready, message.step);
                    break;
                case ExchangeTypeEnum.RUNES_TRADE:
                    client.Character.SmithMagicInstance.Ready(message.ready, message.step);
                    break;
            }
        }
        [MessageHandler]
        public static void HandleExchangeObjectMoveKamas(ExchangeObjectMoveKamaMessage message, WorldClient client)
        {
            if (client.Character.Record.Kamas < message.quantity)
                return;
            switch (client.Character.ExchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    client.Character.PlayerTradeInstance.MoveKamas(message.quantity);
                    break;
                case ExchangeTypeEnum.STORAGE:
                    client.Character.BankInstance.MoveKamas(message.quantity);
                    break;
            }

        }

    }
}
