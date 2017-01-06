﻿using Symbioz.Auth.Handlers;
using Symbioz.Auth.Models;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Auth
{
    class ServersManager
    {
        private static Dictionary<string, Account> _tickets = new Dictionary<string, Account>();

        public static string TransfertToGame(Account account)
        {
            string text = new AsyncRandom().RandomString(32);
            _tickets.Add(text, account);
            return text;
        }
        public static Account GetAccount(string ticket)
        {
            Account result = _tickets[ticket];
            _tickets.Remove(ticket);
            return result;
        }

        public static void DisconnectAlreadyConnectedClient(DofusClient client, int accountid)
        {
            var authClient = AuthServer.Instance.AuthClients.FindAll(x => x.Account.Id == accountid && x != client);
            var worldClient = WorldServer.Instance.WorldClients.FindAll(x => x.Account != null && x.Account.Id == accountid && x != client);

            foreach (var tmp in worldClient)
            {
                if (tmp.Character != null)
                    tmp.Character.Dispose();
                tmp.Disconnect(0, "La connexion a été interrompu par un nouveau client.");
            }

            foreach (var auth in authClient)
            {
                auth.Disconnect(0, "La connexion a été interrompu par un nouveau client.");
            }
        }

        public static int GetWorldConnectedCount()
        {
            return WorldServer.Instance.WorldClients.Count();
        }

        public static int GetAuthConnectedCount()
        {
            return AuthServer.Instance.AuthClients.Count();
        }

        public static int GetConnectedCounts()
        {
            return GetWorldConnectedCount() + GetAuthConnectedCount();
        }
    }
}
