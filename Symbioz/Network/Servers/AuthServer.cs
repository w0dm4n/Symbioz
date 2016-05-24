using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbioz.Core;
using Symbioz.SSync;
using System.Threading.Tasks;
using System.Net.Sockets;
using Symbioz.Network.Clients;
using Symbioz.Utils;
using Symbioz.Helper;
using Symbioz.Enums;
using Symbioz.DofusProtocol.Messages;

namespace Symbioz.Network.Servers
{
    public class AuthServer : Singleton<AuthServer>
    {
        public ServerStatusEnum ServerState = ServerStatusEnum.ONLINE;

        public List<AuthClient> AuthClients = new List<AuthClient>();

        public SSyncServer Server { get; set; }

        public AuthServer()
        {
            this.Server = new SSyncServer(ConfigurationManager.Instance.Host,ConfigurationManager.Instance.AuthPort);
            this.Server.OnServerStarted += Server_OnServerStarted;
            this.Server.OnServerFailedToStart += Server_OnServerFailedToStart;
            this.Server.OnSocketAccepted += Server_OnSocketAccepted;
        }
        void Server_OnSocketAccepted(Socket socket)
        {
            Logger.Auth("New client connected!");
            new AuthClient(socket);
        }

        void Server_OnServerFailedToStart(Exception ex)
        {
            Logger.Error("Unable to start AuthServer! : (" + ex.Message + ")");
        }
        void Server_OnServerStarted()
        {
            Logger.Auth("Server Started (" + Server.EndPoint.AsIpString() + ")");
        }
        public void Start()
        {
            Server.Start();
        }
        public void Send(Message message)
        {
            AuthClients.ForEach(x => x.Send(message));
        }
        public void RemoveClient(AuthClient client)
        {
            AuthClients.Remove(client);
            Logger.Auth("Client Disconnected!");
        }
    }
}
