
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
        private static Timer SaveTaskTimer;
       
        private static Dictionary<Type, List<ITable>> _newElements = new Dictionary<Type, List<ITable>>();
        private static Dictionary<Type, List<ITable>> _updateElements = new Dictionary<Type, List<ITable>>();
        private static Dictionary<Type, List<ITable>> _removeElements = new Dictionary<Type, List<ITable>>();

        public static void Initialize(int seconds)
        {
            SaveTaskTimer = new Timer(seconds * 1000);
            SaveTaskTimer.Elapsed += SaveTaskTimer_Elapsed;
            SaveTaskTimer.AutoReset = true;
            SaveTaskTimer.Start();
        }

        public static void AddElement(ITable element, bool waitingNextWorldSave = true)
        {
            if (waitingNextWorldSave)
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
            }
            else
            {
                Insert(element.GetType(), element);
            }

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

        public static void UpdateElement(ITable element, bool waitingNextWorldSave = true)
        {
            if (waitingNextWorldSave)
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
            else
            {
                Update(element.GetType(), element);
            }
        }

        public static void RemoveElement(ITable element, bool waitingNextWorldSave = true)
        {
            if (element == null)
                return;

            if (waitingNextWorldSave)
            {
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
            }
            else
            {
                Remove(element.GetType(), element);
            }

            RemoveFromList(element);
        }

        static void RemoveFromList(ITable element)
        {
            var field = GetCache(element);
            if (field == null)
            {
                Console.WriteLine("[Remove] Error ! Field unknown for type '" + element.GetType() + "'");
                return;
            }

            var method = field.FieldType.GetMethod("Remove");
            if (method == null)
            {
                Console.WriteLine("[Remove] Error ! Field unknown for type '" + element.GetType() + "'");
                return;
            }

            method.Invoke(field.GetValue(null), new object[] { element });
        }

        private static void SaveTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        { 
            Save(); 
        }

        public static void Save()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            if (OnSaveStarted != null)
                OnSaveStarted();
            SaveTaskTimer.Stop();  
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
                SaveTaskTimer.Start();
                if (OnSaveEnded != null)
                    OnSaveEnded(stopWatch.Elapsed.Seconds);
            }
            catch (Exception e) { Logger.Error("[SAVING WORLD] " + e.Message); }
        }

        #region SingleQueryActions

        public static void Insert(Type type, ITable element)
        {
            try
            {
                Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Add, new ITable[1] { element });
            }
            catch(Exception e)
            {
                Logger.Error("Unable to insert (" + element.GetType() + ") : " + e.Message);
            }
        }

        public static void Update(Type type, ITable element)
        {
            try
            {
                Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Update, new ITable[1] { element });
            }
            catch (Exception e)
            {
                Logger.Error("Unable to update (" + element.GetType() + ") : " + e.Message);
            }
        }

        public static void Remove(Type type, ITable element)
        {
            try
            {
                Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Remove, new ITable[1] { element });
            }
            catch (Exception e)
            {
                Logger.Error("Unable to remove (" + element.GetType() + ") : " + e.Message);
            }
        }

        #endregion

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
