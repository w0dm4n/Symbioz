using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections;
using Symbioz;
using Symbioz.Helper;

namespace Symbioz.ORM
{
    public static class DatabaseManager
    {
        private static string Host;
        private static string Port = null;
        private static string Database;
        private static string User;
        private static string Password;

        public static void Initialize(string host, string port, string database, string user, string password)
        {
            Host = host;
            if (!string.IsNullOrEmpty(port))
            {
                Port = port;
            }
            Database = database;
            User = user;
            Password = password;
        }

        public static MySqlConnection GetNewProvider(bool openProvider = false)
        {
            MySqlConnection newProvider = null;
            if(Port != null)
            {
                newProvider = new MySqlConnection(string.Format("Server={0};Port={1};UserId={2};Password={3};Database={4}", Host,
                    Port, User, Password, Database));
            }
            else
            {
                newProvider = new MySqlConnection(string.Format("Server={0};UserId={1};Password={2};Database={3}", Host, User,
                    Password, Database));
            }
            if(openProvider)
            {
                newProvider.Open();
            }
            return newProvider;
        }
        
        public static void LoadTables(Type[] tables)
        {
            var orderedTables = new Type[tables.Length];
            var dontCatch = new List<Type>();

            foreach (var table in tables)
            {
                var attribute = (TableAttribute)table.GetCustomAttribute(typeof(TableAttribute), false);
                if (attribute == null)
                {
                    Console.WriteLine(string.Format("Warning : The table type '{0}' hasn't got an attribute called 'TableAttribute'", table.GetType().FullName)); 
                    continue;
                }
                    
                if (attribute.catchAll)
                {
                    if (attribute.readingOrder >= 0)
                        orderedTables[attribute.readingOrder] = table;
                }
                else
                    dontCatch.Add(table);
            }

            foreach (var table in tables)
            {
                if (orderedTables.Contains(table) || dontCatch.Contains(table))
                    continue;

                for (var i = tables.Length - 1; i >= 0; i--)
                {
                    if (orderedTables[i] == null)
                    {
                        orderedTables[i] = table;
                        break;
                    }
                }
            }

            foreach (var type in orderedTables)
            {
                if (type == null)
                    continue;

                var reader = Activator.CreateInstance(typeof(DatabaseReader<>).MakeGenericType(type));
                var tableName = (string)reader.GetType().GetProperty("TableName").GetValue(reader);
                Logger.Log("[SQL] Chargement de " + tableName + " ...");
                var method = reader.GetType().GetMethods().FirstOrDefault(x => x.Name == "Read" && x.GetParameters().Length == 1);
                method.Invoke(reader, new object[] { GetNewProvider(true) });

                var elements = reader.GetType().GetProperty("Elements").GetValue(reader);

                var field = type.GetFields().FirstOrDefault(x => x.IsStatic && x.FieldType.IsGenericType && x.FieldType.GetGenericArguments()[0] == type);

                if (field != null)
                    field.SetValue(null, elements);
            }
        }
    }
}
