using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections;
using Symbioz;

namespace Symbioz.ORM
{
    public class DatabaseManager
    {
        // FIELDS
        private static DatabaseManager _self;

        internal MySqlConnection m_provider;

        // PROPERTIES

        // CONSTRUCTORS
        public DatabaseManager(string host, string database, string user, string password)
        {
            if (_self == null)
                _self = this;

            this.m_provider = new MySqlConnection(string.Format("Server={0};UserId={1};Password={2};Database={3}", host, user, password, database));
        }
        public DatabaseManager(string host, string port, string database, string user, string password)
        {
            if (_self == null)
                _self = this;

            this.m_provider = new MySqlConnection(string.Format("Server={0};Port={1};UserId={2};Password={3};Database={4}", host, port, user, password, database));
        }

        public MySqlConnection UseProvider()
        {
            if (!this.m_provider.Ping())
            {
                this.m_provider.Close();
                this.m_provider.Open();
            }

            return this.m_provider;
        }

        public void LoadTables(Type[] tables)
        {
            var orderedTables = new Type[tables.Length];
            var dontCatch = new List<Type>();

            foreach (var table in tables)
            {
                var attribute = (TableAttribute)table.GetCustomAttribute(typeof(TableAttribute), false);
                if (attribute == null)
                {
                    Console.WriteLine(string.Format("Warning : the table type '{0}' hasn't got an attribute called 'TableAttribute'", table.GetType().FullName)); 
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
                Logger.Log("[Sql] Loading " + tableName + " ...");
                var method = reader.GetType().GetMethods().FirstOrDefault(x => x.Name == "Read" && x.GetParameters().Length == 1);
                method.Invoke(reader, new object[] { this.UseProvider() });

                var elements = reader.GetType().GetProperty("Elements").GetValue(reader);

                var field = type.GetFields().FirstOrDefault(x => x.IsStatic && x.FieldType.IsGenericType && x.FieldType.GetGenericArguments()[0] == type);
                if (field != null)
                    field.SetValue(null, elements);
                
            }
        }

        public void CloseProvider()
        {
            this.m_provider.Close();
        }

        public static DatabaseManager GetInstance()
        {
            return _self;
        }
    }
}
