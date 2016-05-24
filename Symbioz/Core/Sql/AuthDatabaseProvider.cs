using MySql.Data.MySqlClient;
using Symbioz.Core.Startup;
using Symbioz.Helper;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Core
{
    public class AuthDatabaseProvider
    {
        [StartupInvoke("Auth Connection",StartupInvokeType.Sql)]
        public static void Initialize() 
        {
           m_connection = (MySqlConnection)DatabaseManager.GetInstance().UseProvider().Clone();
           m_connection.Open();
        }
        private static void CheckConnectionState()
        {
            if (!m_connection.Ping())
            {
                m_connection.Close();
                m_connection.Open();
            }
        }
        private static MySqlConnection m_connection { get; set; }

        public static MySqlConnection Connection
        {
            get
            {
                CheckConnectionState();
                return m_connection;
            }
        }

        public static void Update(string table, string colum, string value, string where, string wherevalue)
        {
            string query = "UPDATE " + table + " SET " + colum + "='" + value + "' WHERE " + where + "='" + wherevalue + "'";
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
            CheckConnectionState();
            if (m_connection.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand(query,m_connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) 
                {
                    Logger.Error("[SQL Execute Query]"+ ex.Message);
                }

            }
            else
            {
                Logger.Error("Unable to execute query " + query + " SqlConnection cannot be oppened");
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
            CheckConnectionState();
            string query = "SELECT * FROM " + table + " WHERE " + where + " = '" + wherevalue + "'";
            string result = "";
            MySqlCommand cmd = new MySqlCommand(query, m_connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                result = dataReader[resultcolum].ToString();
            }
            dataReader.Close();
            return result;

        }
    }
}
