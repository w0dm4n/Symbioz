using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges
{
    public class PlayerTradeRequest
    {
        private Character FirstTrader;
        private Character SecondTrader;

        public PlayerTradeRequest(Character source, Character target)
        {
            FirstTrader = source;
            SecondTrader = target;
        }

        public void Open()
        {
            FirstTrader.Client.Send(new ExchangeRequestedTradeMessage((sbyte)ExchangeTypeEnum.PLAYER_TRADE, (uint)FirstTrader.Id, (uint)SecondTrader.Id));
            SecondTrader.Client.Send(new ExchangeRequestedTradeMessage((sbyte)ExchangeTypeEnum.PLAYER_TRADE, (uint)FirstTrader.Id, (uint)SecondTrader.Id));
        }

        public void Accept()
        {
            var trade = new PlayerTradeExchange(FirstTrader, SecondTrader);
            trade.Open();
            FirstTrader.Request = null;
            SecondTrader.Request = null;
        }

        public void Deny()
        {
            FirstTrader.Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, false));
            SecondTrader.Client.Send(new ExchangeLeaveMessage((sbyte)DialogTypeEnum.DIALOG_EXCHANGE, false));
            FirstTrader.Request = null;
            SecondTrader.Request = null;
        }
    }
}
