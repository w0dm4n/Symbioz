﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Timers;
using System.Threading;

namespace Symbioz.ORM
{
    public class DatabaseWriter<T>
        where T : ITable
    {
        // FIELDS
        private const short MAX_ADDING_LINES = 250;
        private const string ADD_ELEMENTS = "INSERT INTO {0} VALUES {1}";
        private const string UPDATE_ELEMENTS = "UPDATE {0} SET {1} WHERE {2} = {3}";
        private const string REMOVE_ELEMENTS = "DELETE FROM {0} WHERE {1} = {2}";

        private const string LIST_SPLITTER = ",";
        private const string DICTIONARY_SPLITTER = ";";

        private string m_tableName;
        private MySqlCommand m_command;

        private List<FieldInfo> m_fields;
        private List<MethodInfo> m_methods;

        // PROPERTIES

        // CONSTRUCTORS
        public DatabaseWriter(DatabaseAction action, params ITable[] elements)
        {
            this.Initialize(action);
            switch (action)
            {
                case DatabaseAction.Add:
                    this.AddElements(elements);
                    return;

                case DatabaseAction.Update:
                    this.UpdateElements(elements);
                    return;
                case DatabaseAction.Remove:
                    this.DeleteElements(elements);
                    return;
            }
        }

        private void Initialize(DatabaseAction action)
        {
            if (action == DatabaseAction.Add)
                this.m_fields = GetAddFields(typeof(T));
            if (action == DatabaseAction.Update)
                this.m_fields = GetUpdateFields(typeof(T));

            this.m_methods = typeof(T).GetMethods().Where(method => method.GetCustomAttribute(typeof(TranslationAttribute), false) != null &&
                !(method.GetCustomAttribute(typeof(TranslationAttribute), false) as TranslationAttribute).readingMode).ToList();

            this.m_tableName = (typeof(T).GetCustomAttribute(typeof(TableAttribute)) as TableAttribute).tableName;

            if (action != DatabaseAction.Add)
                this.GetPrimaryField();
        }

       /* private void TryToExecute(string query, Timer actionTimer)
        {
            var Provider = DatabaseManager.GetNonQueryProvider();
            if (Provider.State != ConnectionState.Fetching &&
                Provider.State != ConnectionState.Executing)
            {
                Provider.ExecuteAsync(query);
                actionTimer.Stop();
            }
        }
        */

        private void ExecuteAction(string command)
        {
            Monitor.Enter(DatabaseManager.GetNonQueryProvider());
            try
            {
                DatabaseManager.GetNonQueryProvider().Execute(command);
            }
            finally
            {
                Monitor.Exit(DatabaseManager.GetNonQueryProvider());
            }
        }

        private void AddElements(ITable[] elements)
        {
            var values = new List<string>();

            var str = string.Empty;

            for (var i = 0; i < elements.Length / MAX_ADDING_LINES + 1; i++)
            {
                str = string.Empty;

                for (var j = i * MAX_ADDING_LINES; j < (i + 1) * MAX_ADDING_LINES; j++)
                {
                    if (str != string.Empty && elements.Length > j)
                        str += ",\n";

                    if (elements.Length <= j)
                        break;

                    str += string.Format("({0})", this.CreateElement(elements[j]));
                }

                if (str != string.Empty)
                    values.Add(string.Format("{0};", str));
            }

            foreach (var element in values)
            {
                var command = string.Format(ADD_ELEMENTS, this.m_tableName, element);

                try
                {
                    //Console.WriteLine(command);
                    //var connection = DatabaseManager.GetSqlConnection();
                    //connection.Execute(command);
                    //this.m_command = new MySqlCommand(command, DatabaseManager.GetNonQueryProvider());
                    //this.m_command.ExecuteNonQuery();
                    //Console.WriteLine(command);
                    this.ExecuteAction(command);
                }
                catch (Exception ex)
                {
                    Logger.Log("Error (AddElements) : " + ex.ToString());
                }
            }
        }

        private void UpdateElements(ITable[] elements)
        {
            foreach (var element in elements)
            {
                    var values = this.m_fields.ConvertAll<string>(field => string.Format("{0} = {1}", field.Name, this.GetFieldValue(field, element)));
                    if (values.Count > 0)
                    {
                        var command = string.Format(UPDATE_ELEMENTS, this.m_tableName, string.Join(", ", values), this.GetPrimaryField().Name, this.GetPrimaryField().GetValue(element));

                        try
                        {
                            //this.m_command = new MySqlCommand(command, DatabaseManager.GetNonQueryProvider());
                            //this.m_command.ExecuteNonQuery();
                            this.ExecuteAction(command);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Error (UpdateElements) : " + ex.ToString());
                        }
                    }
                    else
                    {
                        Logger.Error("Missing [UpdateAttribute] for table '" + this.m_tableName + "' !");
                    }
            }
        }

        private void DeleteElements(ITable[] elements)
        {
            foreach (var element in elements)
            {
                    var command = string.Format(REMOVE_ELEMENTS, this.m_tableName, this.GetPrimaryField().Name, this.GetPrimaryField().GetValue(element));
                    var sw = System.Diagnostics.Stopwatch.StartNew();

                    try
                    {
                        //this.m_command = new MySqlCommand(command, DatabaseManager.GetNonQueryProvider());
                        //this.m_command.ExecuteNonQuery();
                        this.ExecuteAction(command);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error (DeleteElements) : " + ex.ToString());
                    }
            }
        }

        private string CreateElement(ITable element)
        {

            var values = this.m_fields.ConvertAll<string>(field => this.GetFieldValue(field, element));
            return string.Join(", ", values);
        }

        private string GetFieldValue(FieldInfo field, ITable element)
        {
            var value = field.GetValue(element);
            if (field.GetCustomAttribute(typeof(TranslationAttribute), false) != null)
            {
                var method = field.FieldType.GetMethod("ToString");

                value = method.Invoke(field.GetValue(element), new object[] { });
            }
            else if (field.FieldType == typeof(DateTime))
            {
                value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (this.m_methods.FirstOrDefault(x => x.IsStatic && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == field.FieldType) != null)
            {
                var method = this.m_methods.FirstOrDefault(x => x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == field.FieldType);

                value = method.Invoke(null, new object[] { field.GetValue(element) });
            }
            else if (field.FieldType.GetMethod("Deserialize") != null)
            {
                value = field.FieldType.GetMethod("ToString").Invoke(field.GetValue(element), new object[] { });
            }
            else
            {
                if (field.FieldType.IsGenericType)
                {
                    var arguments = field.FieldType.GetGenericArguments();

                    switch (arguments.Length)
                    {
                        case 1: // List
                            var values = (IList)field.GetValue(element);

                            if (arguments[0].GetMethod("Deserialize") != null)
                            {
                                var method = arguments[0].GetMethod("ToString");

                                var newValues = new List<string>();

                                foreach (var ele in values)
                                {
                                    newValues.Add((string)method.Invoke(ele, new object[] { }));
                                }

                                value = string.Join(DICTIONARY_SPLITTER, newValues);
                                break;
                            }

                            value = "";

                            var array = new List<string>();

                            foreach (var ele in values)
                                array.Add(ele.ToString());

                            value = string.Join(LIST_SPLITTER, array);
                            break;

                        case 2:
                            values = new List<string>();

                            //foreach (var pair in (IDictionary)value)
                            //{
                            //    values.Add(string.Format("{0},{1}", pair
                            //}
                            break;
                    }
                }
            }

            if (value != null && value.ToString() != null)
                return string.Format("'{0}'", value.ToString().Replace(@"\", @"\\").Replace("'", @"\'"));
            else
                return string.Format("'{0}'", value);
        }

        private FieldInfo GetPrimaryField()
        {
            var fields = typeof(T).GetFields().Where(field => field.GetCustomAttribute(typeof(PrimaryAttribute), false) != null);

            if (fields.Count() != 1)
            {
                if (fields.Count() == 0)
                    throw new Exception(string.Format("The Table '{0}' hasn't got a primary field", typeof(T).FullName));

                if (fields.Count() > 1)
                    throw new Exception(string.Format("The Table '{0}' has too much primary fields", typeof(T).FullName));
            }
            return fields.First();
        }

        public static List<FieldInfo> GetUpdateFields(Type type)
        {
            return type.GetFields().Where(field => !field.IsStatic && field.GetCustomAttribute(typeof(IgnoreAttribute), false) == null && field.GetCustomAttribute(typeof(UpdateAttribute), false) != null).OrderBy(x => x.MetadataToken).ToList();
        }

        public static List<FieldInfo> GetAddFields(Type type)
        {
            return type.GetFields().Where(field => !field.IsStatic && field.GetCustomAttribute(typeof(IgnoreAttribute), false) == null).OrderBy(x => x.MetadataToken).ToList();
        }
    }

    public enum DatabaseAction
    {
        Add,
        Update,
        Remove
    }
}
