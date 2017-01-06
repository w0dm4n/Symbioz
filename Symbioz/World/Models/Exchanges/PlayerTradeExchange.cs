using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges
{
    public class PlayerTradeExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.PLAYER_TRADE;
            }
        }

        public PlayerTrader FirstTrader;
        public PlayerTrader SecondTrader;

        public PlayerTradeExchange(Character source, Character target)
        {
            this.FirstTrader = new PlayerTrader(source);
            this.SecondTrader = new PlayerTrader(target);
        }

        public override void CancelExchange()
        {
            FirstTrader.Character.Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, false));
            SecondTrader.Character.Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, false));
            FirstTrader.Character.ResetTrade();
            SecondTrader.Character.ResetTrade();
        }

        public void Close(bool succes)
        {
            FirstTrader.Character.Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, succes));
            SecondTrader.Character.Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, succes));
            FirstTrader.Character.ResetTrade();
            SecondTrader.Character.ResetTrade();
        }

        public void Open()
        {
            FirstTrader.Character.Client.Send(new ExchangeStartedWithPodsMessage((sbyte)ExchangeType, FirstTrader.Character.Id, (uint)FirstTrader.Character.Inventory.GetWeight(), 5000,
                SecondTrader.Character.Id, SecondTrader.Character.Inventory.GetWeight(), 5000));
            SecondTrader.Character.Client.Send(new ExchangeStartedWithPodsMessage((sbyte)ExchangeType, FirstTrader.Character.Id, (uint)FirstTrader.Character.Inventory.GetWeight(), 5000,
                SecondTrader.Character.Id, SecondTrader.Character.Inventory.GetWeight(), 5000));
            FirstTrader.Character.Trader = FirstTrader;
            FirstTrader.Character.PlayerTradeInstance = this;
            SecondTrader.Character.Trader = SecondTrader;
            SecondTrader.Character.PlayerTradeInstance = this;
        }

        public override void MoveItem(uint uid, int quantity)
        {
        }
        public override void AddItemToPanel(CharacterItemRecord obj, uint quantity)
        {
        }
        public override void RemoveItemFromPanel(CharacterItemRecord obj, int quantity)
        {
        }
        public void MoveKamas(int quantity)
        {
        }

        public override void Ready(bool ready, ushort step)
        {
        }
    }
}
