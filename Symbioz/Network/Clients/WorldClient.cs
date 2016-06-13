﻿using Symbioz.DofusProtocol.Messages;
using Symbioz.Network.Servers;
using Symbioz.World.Models;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Network.Clients
{
    public class WorldClient : DofusClient
    {
        public Character Character { get; set; }

        public int CharacterId
        {
            get
            {
                int res = -1;
                if (this.Character != null)
                {
                    res = this.Character.Id;
                }
                return res;
            }
        }

        public List<CharacterRecord> Characters { get; set; }

        public WorldClient(Socket socket) : base(socket)
        {
            this.SSyncClient.OnClosed += SSyncClient_OnClosed;
            Send(new HelloGameMessage());
        }

        void SSyncClient_OnClosed()
        {
            WorldServer.Instance.RemoveClient(this);
        }
    }
}
