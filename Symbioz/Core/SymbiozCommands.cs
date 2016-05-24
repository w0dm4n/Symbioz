
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.World;
using Symbioz.ORM;
using Symbioz.Auth;
using Symbioz.Core.Startup;
using Symbioz.Helper;
using Symbioz.Network.Servers;
using Symbioz.Enums;
using Symbioz.Auth.Handlers;
using Symbioz.Auth.Models;
using MySql.Data.MySqlClient;
using Symbioz.World.Records;
using Symbioz.World.Records.Monsters;
using Symbioz.Network.Messages;
using System.Reflection;
using Symbioz.Providers;
using System.Threading;

namespace Symbioz.Core
{
    class SymbiozCommands
    {
        public static Dictionary<string, Action> commands = new Dictionary<string, Action>();
        public static int Count()
        {
            return commands.Count();
        }
        [StartupInvoke(StartupInvokeType.Internal)]
        public static void LoadCommands()
        {
            commands.Add("help", HelpRequest);
            commands.Add("infos", Infos);
            commands.Add("saveworld",SaveTask.Save);
            commands.Add("off", Offline);
            commands.Add("on", Online);
            commands.Add("restore", Restore);
            commands.Add("rawlist", RawList);
            commands.Add("loadraws", ReloadRaws);
            commands.Add("maxco", MaxCo);
            commands.Add("shutdown", Shutdown);
        }
        public static void HandleCommands()
        {
            while (true)
            {
                string input = Console.ReadLine();
                Handle(input);
            }
           
        }

        private static void Handle(string input)
        {
            if (commands.Keys.Contains(input))
            {
                var command = commands.First(x => x.Key == input);
                command.Value();
            }
            else
                Logger.Init2(input + " n'est pas une commande de SymbiozEmu");

        }
        internal static void Shutdown()
        {
            SaveTask.Save();
            Logger.Init("Server is now shutting down...");
            Thread.Sleep(4000);
            Environment.Exit(0);
        }
        internal static void MaxCo()
        {
            Logger.Init2("Max client connected on this Instance = " + WorldServer.Instance.InstanceMaxConnected);
        }

        internal static void ReloadRaws()
        {
            Logger.Init("Reloading RawDatas...");
            RawDatasManager.Initialize();
            RawList();
            Logger.Init("RawDatas Reloaded");
           
        }
        internal static void RawList()
        {
            Logger.Init("Loaded RawDatas :");
            foreach (var raw in RawDatasManager.Raws)
            {
                Logger.Init2(string.Format("- Name: {0} Size: {1}", raw.Key, raw.Value.Length));
            }
        }
        internal static void Restore()
        {
            AuthDatabaseProvider.Clean(new string[] { "Characters", "CharactersItems", "CharactersJobs",
                "Stats", "BidShopsItems", "SpellsShortcuts", "CharactersSpells","GeneralShortcuts","Guilds","CharactersGuilds" });
            Logger.Init("Database Restored, Characters Deleted");
            System.Threading.Thread.Sleep(2000);
            Environment.Exit(0);
        }
        internal static void HelpRequest()
        {
            Logger.Init("Commands :");
            foreach (var item in commands)
            {
                Logger.Init2("-" + item.Key);
            }
        }
        internal static void Infos()
        {
            Logger.Init2("Version: " + ConstantsRepertory.VERSION);
            Logger.NewLine();
            Logger.Init2("Dofus Version ) " + ConstantsRepertory.DOFUS_REQUIRED_VERSION);
            Logger.Init2("Clients connecteds on AuthServer: " + ServersManager.GetAuthConnectedCount());
            Logger.Init2("Clients connecteds on WorldServer: " + ServersManager.GetWorldConnectedCount());
            Logger.Init2("Total: " + ServersManager.GetConnectedCounts());
        }
        public static void Offline()
        {
            WorldServer.Instance.SetServerState(ServerStatusEnum.NOJOIN);
            
            for (int i = 0; i < WorldServer.Instance.WorldClients.Count(); i++)
            {
                var client = WorldServer.Instance.WorldClients[i];
                if (client.Account.Role != ServerRoleEnum.FONDATOR)
                    client.Disconnect(1000,"Le serveur est desormais inaccessible");
            }
            Logger.World("Le serveur a été passé hors ligne");

        }
        public static void Online()
        {
            WorldServer.Instance.SetServerState(ServerStatusEnum.ONLINE);
            Logger.World("Le serveur est a présent en ligne");
        }
    }
  
}
