
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

        private static bool CanSaveLinkedElements = true;

        private static Dictionary<int, Dictionary<Type, List<ITable>>> _linkedCharactersNewElements = new Dictionary<int, Dictionary<Type, List<ITable>>>();
        private static Dictionary<int, Dictionary<Type, List<ITable>>> _linkedCharactersUpdateElements = new Dictionary<int, Dictionary<Type, List<ITable>>>();
        private static Dictionary<int, Dictionary<Type, List<ITable>>> _linkedCharactersRemoveElements = new Dictionary<int, Dictionary<Type, List<ITable>>>();

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

        #region AddElement

        public static void AddElement(ITable element)
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

            AddToCache(element);
        }

        public static void AddElement(ITable element, int characterId)
        {
            if (characterId > 0)
            {
                lock (_linkedCharactersNewElements)
                {
                    if (_linkedCharactersNewElements.ContainsKey(characterId))
                    {
                        if (_linkedCharactersNewElements[characterId].ContainsKey(element.GetType()))
                        {
                            if (!_linkedCharactersNewElements[characterId][element.GetType()].Contains(element))
                                _linkedCharactersNewElements[characterId][element.GetType()].Add(element);
                        }
                        else
                        {
                            _linkedCharactersNewElements[characterId].Add(element.GetType(), new List<ITable>() { element });
                        }
                    }
                    else
                    {
                        Dictionary<Type, List<ITable>> newDictionary = new Dictionary<Type, List<ITable>>();
                        newDictionary.Add(element.GetType(), new List<ITable>() { element });
                        _linkedCharactersNewElements.Add(characterId, newDictionary);
                    }
                }

                AddToCache(element);
            }
            else
            {
                Logger.Error("Unable to AddElement for characterId '" + characterId + "' !");
            }
        }

        private static void AddToCache(ITable element)
        {
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
        }

        #endregion

        #region UpdateElement

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

        public static void UpdateElement(ITable element, int characterId)
        {
            if (characterId > 0)
            {
                lock (_linkedCharactersUpdateElements)
                {
                    if (_linkedCharactersNewElements.ContainsKey(characterId) &&
                        _linkedCharactersNewElements[characterId].ContainsKey(element.GetType()) &&
                        _linkedCharactersNewElements[characterId][element.GetType()].Contains(element))
                        return;

                    if (_linkedCharactersUpdateElements.ContainsKey(characterId))
                    {
                        if (_linkedCharactersUpdateElements[characterId].ContainsKey(element.GetType()))
                        {
                            if (!_linkedCharactersUpdateElements[characterId][element.GetType()].Contains(element))
                                _linkedCharactersUpdateElements[characterId][element.GetType()].Add(element);
                        }
                        else
                        {
                            _linkedCharactersUpdateElements[characterId].Add(element.GetType(), new List<ITable>() { element });
                        }

                    }
                    else
                    {
                        Dictionary<Type, List<ITable>> newDictionary = new Dictionary<Type, List<ITable>>();
                        newDictionary.Add(element.GetType(), new List<ITable>() { element });
                        _linkedCharactersUpdateElements.Add(characterId, newDictionary);
                    }
                }
            }
            else
            {
                Logger.Error("Unable to UpdateElement for characterId '" + characterId + "' !");
            }
        }

        #endregion

        #region RemoveElement

        public static void RemoveElement(ITable element)
        {
            if (element == null)
                return;

            lock (_removeElements)
            {
                if (_newElements.ContainsKey(element.GetType()) && _newElements[element.GetType()].Contains(element))
                {
                    RemoveFromCache(element);
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

            if (element.GetType().Name == "CharacterSpellRecord")
            {
                Console.WriteLine("RemoveFromCache!");
            }
            RemoveFromCache(element);
        }

        public static void RemoveElement(ITable element, int characterId)
        {
            if (characterId > 0)
            {
                if (element == null)
                    return;

                lock (_linkedCharactersRemoveElements)
                {
                    if (_linkedCharactersNewElements.ContainsKey(characterId) &&
                        _linkedCharactersNewElements[characterId].ContainsKey(element.GetType()) &&
                        _linkedCharactersNewElements[characterId][element.GetType()].Contains(element))
                    {
                        RemoveFromCache(element);
                        _linkedCharactersNewElements[characterId][element.GetType()].Remove(element);
                        return;
                    }

                    if (_linkedCharactersUpdateElements.ContainsKey(characterId) &&
                        _linkedCharactersUpdateElements[characterId].ContainsKey(element.GetType()) &&
                        _linkedCharactersUpdateElements[characterId][element.GetType()].Contains(element))
                        _linkedCharactersUpdateElements[characterId][element.GetType()].Remove(element);

                    if (_linkedCharactersRemoveElements.ContainsKey(characterId))
                    {
                        if (_linkedCharactersRemoveElements[characterId].ContainsKey(element.GetType()))
                        {
                            if (!_linkedCharactersRemoveElements[characterId][element.GetType()].Contains(element))
                                _linkedCharactersRemoveElements[characterId][element.GetType()].Add(element);
                        }
                        else
                        {
                            _linkedCharactersRemoveElements[characterId].Add(element.GetType(), new List<ITable>() { element });
                        }
                    }
                    else
                    {
                        Dictionary<Type, List<ITable>> newDictionary = new Dictionary<Type, List<ITable>>();
                        newDictionary.Add(element.GetType(), new List<ITable>() { element });
                        _linkedCharactersRemoveElements.Add(characterId, newDictionary);
                    }
                }

                RemoveFromCache(element);
            }
            else
            {
                Logger.Error("Unable to RemoveElement for characterId '" + characterId + "' !");
            }
        }

        static void RemoveFromCache(ITable element)
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

        #endregion

        private static void SaveTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        { 
            Save(); 
        }

        public static void Save()
        {
            CanSaveLinkedElements = false;
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

                List<int> charactersIds = new List<int>();
                charactersIds.AddRange(_linkedCharactersNewElements.Keys);
                _linkedCharactersUpdateElements.Keys.ToList().ForEach((x) =>
                {
                    if(!charactersIds.Contains(x))
                    {
                        charactersIds.Add(x);
                    }
                });
                _linkedCharactersRemoveElements.Keys.ToList().ForEach((x) =>
                {
                    if (!charactersIds.Contains(x))
                    {
                        charactersIds.Add(x);
                    }
                });

                foreach(int characterId in charactersIds)
                {
                    SaveCharacter(characterId, true);
                }

                SaveTaskTimer.Start();
                if (OnSaveEnded != null)
                    OnSaveEnded(stopWatch.Elapsed.Seconds);
                CanSaveLinkedElements = true;
            }
            catch (Exception e)
            {
                CanSaveLinkedElements = true;
                Logger.Error("[SAVING WORLD] " + e.Message);
            }
        }

        public static bool SaveCharacter(int characterId, bool forceSave = false)
        {
            bool saved = false;
            if (CanSaveLinkedElements || forceSave)
            {
                Stopwatch stopWatch = Stopwatch.StartNew();
                try
                {
                    if (_linkedCharactersRemoveElements.ContainsKey(characterId))
                    {
                        var types = _linkedCharactersRemoveElements[characterId].Keys.ToList();
                        foreach (var type in types)
                        {
                            if (_linkedCharactersRemoveElements[characterId].ContainsKey(type))
                            {
                                List<ITable> elements;

                                lock (_linkedCharactersRemoveElements[characterId])
                                    elements = _linkedCharactersRemoveElements[characterId][type];

                                try
                                {
                                    var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Remove, elements.ToArray());
                                }
                                catch (Exception e) { Logger.Error(e.Message); }

                                lock (_linkedCharactersRemoveElements[characterId])
                                    _linkedCharactersRemoveElements[characterId][type] = _linkedCharactersRemoveElements[characterId][type].Skip(elements.Count).ToList();
                            }
                        }
                    }

                    if (_linkedCharactersNewElements.ContainsKey(characterId))
                    {
                        var types = _linkedCharactersNewElements[characterId].Keys.ToList();
                        foreach (var type in types)
                        {
                            List<ITable> elements;

                            lock (_linkedCharactersNewElements[characterId])
                                elements = _linkedCharactersNewElements[characterId][type];

                            try
                            {
                                var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Add, elements.ToArray());
                            }
                            catch (Exception e) { Logger.Error(e.ToString()); }

                            lock (_linkedCharactersNewElements[characterId])
                                _linkedCharactersNewElements[characterId][type] = _linkedCharactersNewElements[characterId][type].Skip(elements.Count).ToList();

                        }
                    }

                    if (_linkedCharactersUpdateElements.ContainsKey(characterId))
                    {
                        var types = _linkedCharactersUpdateElements[characterId].Keys.ToList();
                        foreach (var type in types)
                        {
                            List<ITable> elements;
                            lock (_linkedCharactersUpdateElements[characterId])
                                elements = _linkedCharactersUpdateElements[characterId][type];
                            try
                            {
                                var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Update, elements.ToArray());
                            }
                            catch (Exception e) { Logger.Error(e.ToString()); }

                            lock (_linkedCharactersUpdateElements[characterId])
                            {
                                var attribute = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute));

                                if (attribute != null && !attribute.letInUpdateField)
                                    _linkedCharactersUpdateElements[characterId][type] = _linkedCharactersUpdateElements[characterId][type].Skip(elements.Count).ToList();
                            }
                        }
                    }

                    saved = true;
                    Logger.Log(string.Format("Character '{0}' saved in {1}ms !", characterId, stopWatch.ElapsedMilliseconds));
                }
                catch (Exception e)
                {
                    Logger.Error("[SAVING CHARACTER] " + e.Message);
                }
            }
            return saved;
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
