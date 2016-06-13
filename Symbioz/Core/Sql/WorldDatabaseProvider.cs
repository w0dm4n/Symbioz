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
        [StartupInvoke("WorldConnection",StartupInvokeType.Base)]
        public static void Connect()
        {
            DatabaseManager.Initialize(ConfigurationManager.Instance.DatabaseHost,
                ConfigurationManager.Instance.DatabaseName,
                ConfigurationManager.Instance.DatabaseUser,
                ConfigurationManager.Instance.DatabasePassword);
        }

        [StartupInvoke(StartupInvokeType.SQL)]
        public static void Load()
        {
            DatabaseManager.LoadTables(GetTypes());
        }

        public static Type[] GetTypes()
        {
            return Assembly.GetAssembly(typeof(WorldDatabaseProvider)).GetTypes().ToList().Where(x => x.GetInterface("ITable") != null).ToArray();
        }
    }
}