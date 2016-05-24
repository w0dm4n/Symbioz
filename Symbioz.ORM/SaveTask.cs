
using Symbioz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.ORM 
{
    public static class SaveTask
    {
        public static event Action OnSaveStarted;
        public delegate void OnSaveEndedDel(int elapsed);
        public static event OnSaveEndedDel OnSaveEnded;
        private static Timer _timer;
       
        private static Dictionary<Type, List<ITable>> _newElements = new Dictionary<Type, List<ITable>>();
        private static Dictionary<Type, List<ITable>> _updateElements = new Dictionary<Type, List<ITable>>();
        private static Dictionary<Type, List<ITable>> _removeElements = new Dictionary<Type, List<ITable>>();

        public static void Initialize(int seconds)
        {
            _timer = new Timer(seconds * 1000);
            _timer.Elapsed +=_timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();

        }
        
        public static void AddElement(ITable element,bool addtolist = true)
        {
            lock (_newElements)
            {
                if (_newElements.ContainsKey(element.GetType()))
                {
                    if (!_newElements[element.GetType()].Contains(element))
                        _newElements[element.GetType()].Add(element);
                }
                else
                {
                    _newElements.Add(element.GetType(), new List<ITable> { element });
                }
            }
            if (addtolist)
            {
                #region Add value into array
                var field = GetCache(element);
                if (field == null)
                {
                    Logger.Error("Unable to add record value to the list, static list field wasnt finded");
                    return;
                }

                var method = field.FieldType.GetMethod("Add");
                if (method == null)
                {
                    Console.WriteLine("Unable to add record value to the list, add method wasnt finded");
                    return;
                }

                method.Invoke(field.GetValue(null), new object[] { element });
                #endregion
            }
        }

        public static void UpdateElement(ITable element)
        {
            lock (_updateElements)
            {
                if (_newElements.ContainsKey(element.GetType()) && _newElements[element.GetType()].Contains(element))
                    return;

                if (_updateElements.ContainsKey(element.GetType()))
                {
                    if (!_updateElements[element.GetType()].Contains(element))
                        _updateElements[element.GetType()].Add(element);
                }
                else
                {
                    _updateElements.Add(element.GetType(), new List<ITable> { element });
                }
            }
        }

        public static void RemoveElement(ITable element,bool removefromlist = true)
        {
            if (element == null)
                return;
            lock (_removeElements)
            {
                if (_newElements.ContainsKey(element.GetType()) && _newElements[element.GetType()].Contains(element))
                {
                    RemoveFromList(element);
                    _newElements[element.GetType()].Remove(element);
                    return;
                }

                if (_updateElements.ContainsKey(element.GetType()) && _updateElements[element.GetType()].Contains(element))
                    _updateElements[element.GetType()].Remove(element);

                if (_removeElements.ContainsKey(element.GetType()))
                {
                    if (!_removeElements[element.GetType()].Contains(element))
                        _removeElements[element.GetType()].Add(element);
                }
                else
                {
                    _removeElements.Add(element.GetType(), new List<ITable> { element });
                }
            }
            if (removefromlist)
            {
                RemoveFromList(element);
            }
        }
        static void RemoveFromList(ITable element)
        {
            var field = GetCache(element);
            if (field == null)
            {
                Console.WriteLine("[Remove] Erreur ! Field unknown");
                return;
            }

            var method = field.FieldType.GetMethod("Remove");
            if (method == null)
            {
                Console.WriteLine("[Remove] Erreur ! Field unknown");
                return;
            }

            method.Invoke(field.GetValue(null), new object[] { element });
        }
        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        { 
            Save(); 
        }
        public static void Save()
        {
            Stopwatch w = Stopwatch.StartNew();
            if (OnSaveStarted != null)
                OnSaveStarted();
            _timer.Stop();  
            try
            {
               
                var types = _removeElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;
                    lock (_removeElements)
                        elements = _removeElements[type];

                    try
                    {
                        var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Remove, elements.ToArray());
                    }
                    catch (Exception e) { Logger.Error(e.Message); }

                    lock (_removeElements)
                        _removeElements[type] = _removeElements[type].Skip(elements.Count).ToList();
                  
                }
          
                types = _newElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;

                    lock (_newElements)
                        elements = _newElements[type];

                    try
                    {
                        var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Add, elements.ToArray());
                    }
                    catch (Exception e) { Logger.Error(e.ToString()); }

                    lock (_newElements)
                        _newElements[type] = _newElements[type].Skip(elements.Count).ToList();

                }

                types = _updateElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;
                    lock (_updateElements)
                        elements = _updateElements[type];

                    try
                    {
                        var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Update, elements.ToArray());
                    }
                    catch (Exception e) { Logger.Error(e.ToString()); }

                    lock (_updateElements)
                    {
                        var attribute = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute));

                        if (attribute != null && !attribute.letInUpdateField)
                            _updateElements[type] = _updateElements[type].Skip(elements.Count).ToList();
                    }
                }

               
                _timer.Start();
                if (OnSaveEnded != null)
                    OnSaveEnded(w.Elapsed.Seconds);

            }
            catch (Exception e) { Logger.Error("[SAVING WORLD] " + e.Message); }
        }

        private static FieldInfo GetCache(ITable table)
        {
            var attribute = table.GetType().GetCustomAttribute(typeof(TableAttribute), false);
            if (attribute == null)
                return null;

            var field = table.GetType().GetFields().FirstOrDefault(x => x.Name.ToLower() == (attribute as TableAttribute).tableName.ToLower());
            if (field == null || !field.IsStatic || !field.FieldType.IsGenericType)
                return null;

            return field;
        }
    }
}
