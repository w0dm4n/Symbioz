using Symbioz.Auth.Handlers;
using Symbioz.Auth.Models;
using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Messages;
using Symbioz.SSync;
using Symbioz.Utils;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Network.Clients
{
    public class DofusClient
    {
        public Account Account { get; set; }

        public DofusClient(Socket socket)
        {
            this.SSyncClient = new SSyncClient(socket);
            this.SSyncClient.OnDataArrival += SSyncClient_OnDataArrival;
        }
        void SSyncClient_OnDataArrival(byte[] datas)
        {
            MessagesHandler.Handle(MessagesHandler.BuildMessage(datas), this);
        }
        public SSyncClient SSyncClient { get; set; }

        public void Send(Message message)
        {
            CustomDataWriter writer = new CustomDataWriter();
            message.Pack(writer);
            var packet = writer.Data;
            SSyncClient.Send(packet);

            if (ConfigurationManager.Instance.ShowProtocolMessages)
            Logger.Info(string.Format("[Send] {0}", message.ToString()));
        }

        public void SendRaw(string rawname)
        {
           Send(new RawDataMessage(RawDatasManager.GetRawData(rawname)));
        }

        public void Disconnect(int timeout = 0, string reason = null)
        {
            if (timeout == 0)
            {
                if (reason != null)
                    ConnectionHandler.SendSystemMessage(this, reason);
                SSyncClient.Sock.Close();
            }
            else
                DisconnectEngine.New(this, timeout, reason);
        }
    }
}
