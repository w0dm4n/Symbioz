using MySql.Data.MySqlClient;
using Symbioz.Core.Startup;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Core
{
    internal static class WorldDatabaseProvider
    {
        public static MySqlConnection Connection { get { return _database.UseProvider(); } }
        private static DatabaseManager _database;

        public static DatabaseManager GetCurrentDatabase { get { return _database; } }



        [StartupInvoke("World Connection",StartupInvokeType.Base)]
        public static void Connect()
        {

            _database = new DatabaseManager(ConfigurationManager.Instance.DatabaseHost,
                                           ConfigurationManager.Instance.DatabaseName,
                                            ConfigurationManager.Instance.DatabaseUser,
                                           ConfigurationManager.Instance.DatabasePassword);
            _database.UseProvider();

        }
        [StartupInvoke(StartupInvokeType.Sql)]
        public static void Load()
        {
            _database.LoadTables(GetTypes());
        }
        public static void Disconnect()
        {
            _database.CloseProvider();
        }

        public static Type[] GetTypes()
        {
            return Assembly.GetAssembly(typeof(WorldDatabaseProvider)).GetTypes().ToList().Where(x => x.GetInterface("ITable") != null).ToArray();
        }
    }
}