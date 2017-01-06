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
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Records;
using Symbioz.World.Models;
using Shader.Helper;
using Symbioz.ORM;
using Symbioz.Auth.Records;
using System.Threading;

namespace Symbioz.Network.Servers
{
    public class WorldServer : Singleton<WorldServer>
    {
        public int InstanceMaxConnected = 0;
        public ServerStatusEnum ServerState = ServerStatusEnum.ONLINE;
        public List<WorldClient> WorldClients = new List<WorldClient>();
        public SSyncServer Server { get; set; }
        public DateTime StartTime { get; set; }

        public WorldServer()
        {
            this.Server = new SSyncServer(ConfigurationManager.Instance.Host, ConfigurationManager.Instance.WorldPort);
            this.Server.OnServerStarted += Server_OnServerStarted;
            this.Server.OnServerFailedToStart += Server_OnServerFailedToStart;
            this.Server.OnSocketAccepted += Server_OnSocketAccepted;
           
        }
        void Server_OnSocketAccepted(Socket socket)
        {
            Logger.World("New client connected!");

            Monitor.Enter(this.WorldClients);
            try
            {
                WorldClients.Add(new WorldClient(socket));
                if (WorldClients.Count > InstanceMaxConnected)
                    InstanceMaxConnected = WorldClients.Count;
            }
            finally
            {
                Monitor.Exit(this.WorldClients);
            }
            
        }

        void Server_OnServerFailedToStart(Exception ex)
        {
            Logger.Error("Unable to start WorldServer! : (" + ex.Message + ")");
        }
        
        void Server_OnServerStarted()
        {
            Logger.World("Server started (" + Server.EndPoint.AsIpString() + ")");
            this.StartTime = new DateTime();
            this.StartTime = DateTime.Now;

        }
        public void Start()
        {
            Server.Start();
        }
        public void SetServerState(ServerStatusEnum state)
        {
            ServerState = state;
            foreach (var client in AuthServer.Instance.AuthClients)
            {
                var count = CharacterRecord.GetAccountCharacters(client.Account.Id).Count();
                var servers = new List<GameServerInformations>();
                servers.Add(new GameServerInformations((ushort)ConfigurationManager.Instance.ServerId, (sbyte)WorldServer.Instance.ServerState, 0, true, (sbyte)count, 1));
                client.Send(new ServersListMessage(servers, 0, true));
            }
        }
        
        public string GetUptime()
        {
            DateTime now = DateTime.Now;
            TimeSpan span = now.Subtract(this.StartTime);
            return span.Hours + ":" + span.Minutes + ":" + span.Seconds;
        }

        public void Send(Message message)
        {
            WorldClients.ForEach(x => x.Send(message));
        }
        public void SendToOnlineCharacters(Message message)
        {
            GetAllClientsOnline().ForEach(x => x.Send(message));
        }
        public void SendOnSubarea(Message message,int subareaid)
        {
            GetAllClientsOnline().FindAll(x => x.Character.SubAreaId == subareaid).ForEach(x=>x.Send(message));
        }
        public void RemoveClient(WorldClient client)
        {
            try
            {
                bool remove = true;
                if (client != null && client.Character != null)
                {
                   /* if (client.Character.FighterInstance != null && client.Character.FighterInstance.Fight != null)
                    {
                        CharactersDisconnected.add(client.Character);
                        client.Character.FighterInstance.Fight.FighterDisconnect(client.Character.FighterInstance);
                        remove = false;
                    }*/
                    if (client.Character != null && client.Character.CurrentStats != null)
                    {
                        client.Character.Record.CurrentLifePoint = client.Character.CurrentStats.LifePoints;
                        if (client.Character.IsRegeneratingLife)
                            client.Character.StopRegenLife();
                    }
                    AccountsProvider.UpdateAccountsOnlineState(client.Account.Id, false);
                    client.Character.Record.LastConnection = (int)DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                    if (client.Character != null)
                    {
                        client.Character.Dispose();
                        client.Character = null;
                    }
                }

                if (remove == true)
                {
                    Monitor.Enter(this.WorldClients);
                    try
                    {
                        WorldClients.Remove(client);

                    }
                    finally
                    {
                        Monitor.Exit(this.WorldClients);
                    }
                }
                Logger.World("Client disconnected!");
            }
            catch(Exception error)
            {
                Logger.Log(error);
            }
        }
        public List<WorldClient> GetAllClientsOnline()
        {
            List<WorldClient> clients = null;
            Monitor.Enter(this.WorldClients);
            try
            {
                clients = WorldClients.FindAll(x => x.Character != null);
            }
            finally
            {
                Monitor.Exit(this.WorldClients);       
            }
            return clients;
        }
        public WorldClient GetOnlineClient(int characterid)
        {
            return GetAllClientsOnline().Find(x => x.Character.Id == characterid);
        }
        public WorldClient GetOnlineClient(string characterName)
        {
            return GetAllClientsOnline().Find(x => x.Character.Record.Name == characterName);
        }
        public WorldClient GetOnlineClientByAccountId(int accountId)
        {
            return GetAllClientsOnline().Find(x => x.Account.Id == accountId);
        }

        public List<WorldClient> GetAllOnlineClientByAccountId(int accountId)
        {
            List<WorldClient> accounts = new List<WorldClient>();
            foreach (var client in this.GetAllClientsOnline())
            {
                if (client.Account.Id == accountId)
                    accounts.Add(client);
            }
            return accounts;
        }
        public List<WorldClient> GetOnlineClientOnMap(int mapId)
        {
           var onlineClients =  WorldClients.FindAll(x => x.Character != null);
            List <WorldClient> onTheMap = new List<WorldClient>();
            foreach (var client in onlineClients)
            {
                if (client.Character.Record.MapId == mapId)
                    onTheMap.Add(client);
            }
            return onTheMap;
        }
 
        public bool IsConnected(string characterName)
        {
            return GetOnlineClient(characterName) != null;
        }
        public bool IsConnected(int characterId)
        {
            return GetOnlineClient(characterId) != null;
        }

    }
}
