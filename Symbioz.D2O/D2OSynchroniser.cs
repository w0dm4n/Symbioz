using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Symbioz;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Helper;
using Symbioz.D2O.Classes;
using Symbioz.D2O.InternalClasses;
using System.Reflection;

namespace Symbioz.D2O
{
    /// <summary>
    /// Synchronisateur D2O > SQL Pour Dofus 2.29
    /// </summary>
    public class D2OSynchroniser
    {
        public const char InternalFieldValueDelimitator = '.';
        public static string[] UnauthorizedSQLFieldNames = new string[] { "order", "range", "trigger" ,"triggers","cursor"};
        public static string[] D2IFields = new string[] { "nameId", "descriptionId","theoreticalDescriptionId" };
        static string GetCreateTableQuery(string tableName,List<string> fields)
        {
            string str = "";
            foreach (var field in fields)
            {
                if (fields.Last().Equals(field))
                    str += field + " " + "mediumtext";
                else
                    str += field + " " + "mediumtext" + ",";
            }
            return "CREATE TABLE if not exists " + tableName + " (" + str + ")";
        }
        static string GetInsertQuery(string tablename,string splittedfields,List<string> values)
        {
            string fvalues = string.Empty;
            foreach (var value in values){ fvalues += "'" + value + "',"; }
            fvalues = fvalues.Remove(fvalues.Length-1);
            return "INSERT INTO " + tablename + " (" + splittedfields + ") VALUES (" + fvalues + ")";
        }
        public static D2OSyncQueries[] GetAllD2OsQueries(bool convertlang)
        {
            List<D2OSyncQueries> syncQueries = new List<D2OSyncQueries>();
            foreach (var d2o in D2OActivator.GetD2ONames())
            {
                syncQueries.Add(GetSyncQueries(Type.GetType("Symbioz.D2O.Classes." + d2o),convertlang));
               
            }
            return syncQueries.ToArray();
        }
        public static D2OSyncQueries GetSyncQueries(Type type,bool convertlang)
        {
            string d2o = type.Name;
            List<string> d2oQueries = new List<string>();
            List<string> fields = GetD2OFields(d2o).ToList();
            var values = GetD2OValues(d2o,convertlang);
            d2oQueries.Add(GetCreateTableQuery(d2o, fields));
            foreach (var value in values)
            {
                d2oQueries.Add(GetInsertQuery(d2o, fields.ToSplitedString(), value.ToList()));
            }
            return new D2OSyncQueries(d2o, d2oQueries);
           
            
        }
        public static D2OSyncQueries GetSyncQueries<T>(bool convertlang) where T : ID2OClass
        {
            return GetSyncQueries(typeof(T),convertlang);
        }
       
        /// <summary>
        /// Récupère une liste contenant chaques valeurs du D2O en fonction de ses fields
        /// </summary>
        /// <param name="type">Type de la classe D2O</param>
        /// <returns>Liste des valeurs des fields D2O</returns>
        static List<string[]> GetD2OValues(Type type,bool convertlang)
        {
            List<string[]> result = new List<string[]>();
            var cache = (IEnumerable)GetCache(type).GetValue(null);
            var fields = GetD2OFields(type);
            foreach (var value in cache)
            {
                List<string> fieldsValues = new List<string>();
                foreach (var field in value.GetType().GetFields().ToList().FindAll(x => !x.IsStatic))
                {
                  
                    var fieldValue = field.GetValue(value);
                    if (fieldValue.IsEnumerable() && fieldValue.GetType() != typeof(string))
                    {
                        var str = string.Empty;
                        var fieldValueEnumerable = fieldValue as IEnumerable;
                        foreach (var data in fieldValueEnumerable)
                        {
                            if (data is ID2OInternalClass)
                                str += GetParsedD2OInternalClass((ID2OInternalClass)data)+";";
                            else
                                str += data + ",";
                        }
                        if (str != string.Empty)
                        {
                            str = str.Replace("'", " ");
                            str = str.Remove(str.Length - 1);
                        }
                      
                   
                            fieldsValues.Add(str);
                        

                    }
                    else if (fieldValue is ID2OInternalClass)
                    {
                        string str = GetParsedD2OInternalClass((ID2OInternalClass)fieldValue);
                        str = str = str.Replace("'", " ");
                        fieldsValues.Add(str);
                    }
                    else
                    {
                        if (convertlang && D2IFields.Contains(field.Name))
                        {
                            string lang = D2OActivator.D2IFile.GetText(int.Parse(fieldValue.ToString()));
                            if (lang != null)
                            {
                                lang = lang.Replace("'", " ");
                                fieldsValues.Add(lang);
                            }
                            else
                                fieldsValues.Add("[UNKNOWN_TEXT_ID_" + fieldValue + "]");
                        }
                        else
                        {
                            string str = fieldValue.ToString();
                            str = str.Replace("'", " ");
                            fieldsValues.Add(str);
                        }

                    }
                }

                result.Add(fieldsValues.ToArray());
            }

            return result;
        }
        static List<string[]> GetD2OValues(string d2oName,bool convertlang)
        {
            return GetD2OValues(Type.GetType("Symbioz.D2O.Classes." + d2oName),convertlang);
        }
        static string GetParsedD2OInternalClass(ID2OInternalClass internalClass)
        {
            string str = string.Empty;
            var type = internalClass.GetType();
            var fields =type.GetFields().ToList().FindAll(x => !x.IsStatic);
            foreach (var field in fields)
            {
                str += field.GetValue(internalClass) + InternalFieldValueDelimitator.ToString();
            }
            str = str.Remove(str.Length - 1);
            return str;
        }
        /// <summary>
        ///  Retourne les Fields D2O sous forme d'un tableau de string
        /// </summary>
        /// <param name="type">Nom du D2O</param>
        /// <returns></returns>
        public static string[] GetD2OFields(Type type)
        {
            List<string> fields = new List<string>();
            foreach (var field in type.GetFields().ToList().FindAll(x => !x.IsStatic))
            {
                string futureFieldName;
                if (UnauthorizedSQLFieldNames.Contains(field.Name))
                    futureFieldName = "_" + field.Name;
                else
                    futureFieldName = field.Name;
                fields.Add(futureFieldName);
            }
            return fields.ToArray();

        }
        /// <summary>
        /// Retourne les Fields D2O sous forme d'un tableau de string
        /// </summary>
        /// <param name="d2oName">Nom du D2O</param>
        /// <returns>Fields</returns>
         static string[] GetD2OFields(string d2oName)
        {
            return GetD2OFields(Type.GetType("Symbioz.D2O.Classes." + d2oName));
        }
         static FieldInfo GetCache(Type d2oType)
         {
             return d2oType.GetFields().FirstOrDefault(x => x.GetCustomAttribute(typeof(CacheAttribute)) != null);
         }
    }
}
