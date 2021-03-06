﻿using MySql.Data.MySqlClient;
using Symbioz.Core.Startup;
using Symbioz.Helper;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.Core
{
    public class AuthDatabaseProvider
    {
        public static MySqlConnection AuthConnection { get; set; }

        [StartupInvoke("AuthConnection",StartupInvokeType.SQL)]
        public static void Initialize() 
        {
            AuthConnection = DatabaseManager.GetNewProvider();
            AuthConnection.Open();
        }
      /*  private static void CheckConnectionState()
        {
            if (!AuthConnection.Ping())
            {
                AuthConnection.Close();
                AuthConnection.Open();
            }
        }

      /*  public static MySqlConnection Connection
        {
            get
            {
                CheckConnectionState();
                return AuthConnection;
            }
        }*/

        public static void Update(string table, string column, string value, string where, string wherevalue)
        {
            string query = "UPDATE " + table + " SET " + column + "='" + value + "' WHERE " + where + "='" + wherevalue + "'";
            Execute(query);
        }

        public static void Insert(string tablename,List<string> fields,List<string> values)
        {
            string fieldsStr = fields.ToSplitedString();
            string valuesStr = string.Empty;
            foreach (var value in values)
            {
                valuesStr += "'" + value + "',";
            }
            valuesStr = valuesStr.Remove(valuesStr.Length - 1);
            string query = "INSERT INTO " + tablename + " (" + fieldsStr + ") VALUES (" + valuesStr + ")";
            Execute(query);
           
        }

        public static void Delete(string tablename,object where,string werefield = "Id")
        {
            Execute(string.Format("DELETE FROM {0} WHERE {1}={2}",tablename,werefield,where));
        }

        public static void Execute(string query)
        {
            //CheckConnectionState();
            Monitor.Enter(AuthConnection);
            try
            {
                if (AuthConnection.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand(query, AuthConnection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("[SQL Execute Query]" + ex.Message);
                    }

                }
                else
                {
                    Logger.Error("Unable to execute query " + query + " SqlConnection cannot be oppened");
                }
            }
            finally
            {
                Monitor.Exit(AuthConnection);
            }
            
        }

        public static void Clean(IEnumerable<string> tables)
        {
            string query;
            foreach (var table in tables)
            {
                query = "delete from " + table + "";
                Execute(query);
                Logger.Init2(table + " deleted...");
            } 
        }

        public static string SelectData(string table, string where, string wherevalue, string resultcolum)
        {
            //  CheckConnectionState();
            Monitor.Enter(AuthConnection);
            try
            {
                string query = "SELECT * FROM " + table + " WHERE " + where + " = '" + wherevalue + "'";
                string result = "";
                MySqlCommand cmd = new MySqlCommand(query, AuthConnection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    result = dataReader[resultcolum].ToString();
                }
                dataReader.Close();
                return result;
            }
            finally
            {
                Monitor.Exit(AuthConnection);
            }
        }

        public static List<int> GetAccountsOnline()
        {
            Monitor.Enter(AuthConnection);
            try
            {
                List<int> Accounts = new List<int>();
                //  CheckConnectionState();
                string query = "SELECT * FROM accounts WHERE isOnline = '1'";
                MySqlCommand cmd = new MySqlCommand(query, AuthConnection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Accounts.Add(Int32.Parse(dataReader["Id"].ToString()));
                }
                dataReader.Close();
                return (Accounts);
            }finally
            {
                Monitor.Exit(AuthConnection);
            }
        }

        public static void UpdatePoints(int Points, int account)
        {
            Monitor.Enter(AuthConnection);
            try
            {
                string query = "UPDATE accounts SET PointCount = '" + Points + "' WHERE Id = '" + account + "'";
                MySqlCommand cmd = new MySqlCommand(query, AuthConnection);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                Monitor.Exit(AuthConnection);
            }
        }

        public static int GetPoints(int account)
        {
            int Points = 0;
            Monitor.Enter(AuthConnection);
            try
            {         
                string query = "SELECT * FROM accounts WHERE Id = '" + account + "'";
                MySqlCommand cmd = new MySqlCommand(query, AuthConnection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    Points = Int32.Parse(dataReader["PointCount"].ToString());
                dataReader.Close();
                return Points;
            }
            finally
            {
                Monitor.Exit(AuthConnection);
            }
            return 0;
        }
    }
}
