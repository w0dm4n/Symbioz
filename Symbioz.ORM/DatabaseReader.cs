using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.ORM
{
    public class DatabaseReader<T>
       where T : ITable
    {
        // FIELDS
        private const char LIST_SPLITER = ',';
        private const char DICTIONARY_SPLITER = ';';

        private List<T> m_elements;
        private MySqlDataReader m_reader;
        private FieldInfo[] m_fields;
        private MethodInfo[] m_methods;

        private string m_tableName;

        // PROPERTIES
        public string TableName { get { return this.m_tableName; } }
        public List<T> Elements { get { return this.m_elements; } }

        // CONSTRUCTORS
        public DatabaseReader()
        {
            this.m_elements = new List<T>();

            this.Initialize();
        }

        // METHODS
        private void Initialize()
        {
            this.m_fields = typeof(T).GetFields().Where(field =>
                field.GetCustomAttribute(typeof(IgnoreAttribute), false) == null &&
                !field.IsStatic).ToArray();

            this.m_methods = typeof(T).GetMethods().Where(method => method.IsStatic && 
                method.GetParameters().Length == 1 &&
                method.GetParameters()[0].ParameterType == typeof(string)).ToArray();

            if (typeof(T).GetCustomAttribute(typeof(TableAttribute)) == null)
                throw new Exception("");

            this.m_tableName = (typeof(T).GetCustomAttribute(typeof(TableAttribute)) as TableAttribute).tableName;
        }

        private void ReadTable(MySqlConnection connection, string parameter)
        {
            var command = new MySqlCommand(parameter, connection);

            this.m_reader = command.ExecuteReader();
            while (this.m_reader.Read())
            {
                var obj = new object[this.m_fields.Length];
                for (var i = 0; i < this.m_fields.Length; i++)
                    obj[i] = this.m_reader[i];

                this.VerifyFieldsType(obj);

                
                    this.m_elements.Add((T)Activator.CreateInstance(typeof(T), obj));
                
               
            }
            this.m_reader.Close();
        }

        public void Read(MySqlConnection connection)
        {
            this.ReadTable(connection, string.Format("SELECT * FROM `{0}` WHERE 1", this.m_tableName));
        }
        /// <param name="condition">WHERE {0}</param>
        public void Read(MySqlConnection connection, string condition)
        {
            this.ReadTable(connection, string.Format("SELECT * FROM `{0}` WHERE {1}", this.m_tableName, condition));
        }

        private void VerifyFieldsType(object[] obj)
        {
            for (var i = 0; i < this.m_fields.Length; i++)
            {
                if (obj[i].GetType() == this.m_fields[i].FieldType)
                    continue;

                var method = this.m_methods.FirstOrDefault(element => element.ReturnType == this.m_fields[i].FieldType);
                if (method != null)
                {

                    obj[i] = method.Invoke(null, new object[] { obj[i].ToString() });
                    continue;
                }

                if (this.m_fields[i].FieldType.IsGenericType)
                {
                    var parameters = this.m_fields[i].FieldType.GetGenericArguments();

                    switch (parameters.Length)
                    {
                        case 1: // List

                            var elements = (obj[i].ToString()).Split(new char[] { LIST_SPLITER }, StringSplitOptions.RemoveEmptyEntries);

                            var newList = Activator.CreateInstance(typeof(List<>).MakeGenericType(parameters));
                            method = newList.GetType().GetMethod("Add");

                            var desezializeMethod = parameters[0].GetMethod("Deserialize");
                            if (desezializeMethod != null)
                            {
                                elements = (obj[i] as string).Split(new char[] { DICTIONARY_SPLITER }, StringSplitOptions.RemoveEmptyEntries);

                                foreach (var element in elements)
                                    method.Invoke(newList, new object[] { desezializeMethod.Invoke(null, new object[] { element }) });
                            }
                            else
                            {
                                foreach (var element in elements)
                                    method.Invoke(newList, new object[] { Convert.ChangeType(element, parameters[0]) });
                            }

                            obj[i] = newList;
                            continue;

                        case 2: // Dictionary
                            elements = (obj[i] as string).Split(new char[] { DICTIONARY_SPLITER }, StringSplitOptions.RemoveEmptyEntries);

                            var dictionary = new Dictionary<string, List<string>>();
                            foreach (var element in elements)
                            {
                                var args = element.Split(LIST_SPLITER);
                                dictionary.Add(args[0], args.Skip(1).ToList());
                            }

                            var newDictionary = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(parameters));
                            method = newDictionary.GetType().GetMethod("Add");

                            foreach (var pair in dictionary)
                            {
                                var key = Convert.ChangeType(pair.Key, parameters[0]);
                                var value = pair.Value.ConvertAll(element => Convert.ChangeType(element, parameters[1]));

                                method.Invoke(newDictionary, new object[] { key, value });
                            }

                            obj[i] = newDictionary;
                            continue;
                    }
                }

                method = this.m_fields[i].FieldType.GetMethod("Deserialize");
                if (method != null)
                {
                    obj[i] = method.Invoke(null, new object[] { obj[i] });
                    continue;
                }

                try { obj[i] = Convert.ChangeType(obj[i], this.m_fields[i].FieldType); }
                catch 
                { 
                    string exeption = string.Format("Unknown constructor for '{0}', ({1})", this.m_fields[i].FieldType.Name, this.m_fields[i].Name);
                    Console.WriteLine(exeption);
                    throw new Exception(exeption); 
                }
            }
        }
    }
}
