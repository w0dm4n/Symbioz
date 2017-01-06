
using Shader.SSync;
using Symbioz;
using Symbioz.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.ORM
{
    public static class SaveTask
    {
        public static event Action OnSaveStarted;
        public delegate void OnSaveEndedDel(int elapsed);
        public static event OnSaveEndedDel OnSaveEnded;
        private static System.Timers.Timer SaveTaskTimer;

        private static bool CanSaveLinkedElements = true;

        private static ConcurrentDictionary<int, Dictionary<Type, List<ITable>>> _linkedCharactersNewElements = new ConcurrentDictionary<int, Dictionary<Type, List<ITable>>>();
        private static ConcurrentDictionary<int, Dictionary<Type, List<ITable>>> _linkedCharactersUpdateElements = new ConcurrentDictionary<int, Dictionary<Type, List<ITable>>>();
        private static ConcurrentDictionary<int, Dictionary<Type, List<ITable>>> _linkedCharactersRemoveElements = new ConcurrentDictionary<int, Dictionary<Type, List<ITable>>>();

        //MYSQL LIST :
        //list INSERT
        private static ConcurrentDictionary<Type, List<ITable>> _newElements = new ConcurrentDictionary<Type, List<ITable>>();
        //list UPDATE
        private static ConcurrentDictionary<Type, List<ITable>> _updateElements = new ConcurrentDictionary<Type, List<ITable>>();
        //list temporaire durant une save des UPDATES
        private static ConcurrentDictionary<Type, List<ITable>> _updateElementsOnSave = new ConcurrentDictionary<Type, List<ITable>>();
        //list DELETE
        private static ConcurrentDictionary<Type, List<ITable>> _removeElements = new ConcurrentDictionary<Type, List<ITable>>();



        public static SelfRunningTaskPool IOTask;
        private static bool onSave = false;
        private static Object locker = new Object();

        public static void Initialize(int seconds)
        {
            SaveTaskTimer = new System.Timers.Timer(seconds * 1000);
            SaveTaskTimer.Elapsed += SaveTaskTimer_Elapsed;
            SaveTaskTimer.AutoReset = true;
            SaveTaskTimer.Start();
            SaveTask.IOTask = new SelfRunningTaskPool(50, "IO Task Pool");

        }

        #region AddElement

        public static void AddElement(ITable element)
        {
            if (element != null)
            {
                List<ITable> elements = new List<ITable>() { element };
                Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(element.GetType()), DatabaseAction.Add, elements.ToArray());
                AddToCache(element);
            }
            /*Monitor.Enter(_newElements);
            try
            {
                if (_newElements.ContainsKey(element.GetType()))
                {
                    if (!_newElements[element.GetType()].Contains(element))
                        _newElements[element.GetType()].Add(element);
                }
                else
                {
                    _newElements.TryAdd(element.GetType(), new List<ITable> { element });
                }


                AddToCache(element);
            }
            finally
            {
                Monitor.Exit(_newElements);
            }*/
        }

        public static void AddElement(ITable element, int characterId)
        {
            if (element != null)
            {
                List<ITable> elements = new List<ITable>() { element };
                Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(element.GetType()), DatabaseAction.Add, elements.ToArray());
                AddToCache(element);
            }
            /*if (characterId > 0)
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
                        _linkedCharactersNewElements.TryAdd(characterId, newDictionary);
                    }
                

                AddToCache(element);
            }
            else
            {
                Logger.Error("Unable to AddElement for characterId '" + characterId + "' !");
            }*/
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
            //pas de save
            if (onSave == false)
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
                    _updateElements.TryAdd(element.GetType(), new List<ITable> { element });
                }
            }
            else // save en cours
            {
                if (_newElements.ContainsKey(element.GetType()) && _newElements[element.GetType()].Contains(element))
                    return;

                //Ajout dans la list temporaire
                if (_updateElementsOnSave.ContainsKey(element.GetType()))
                {
                    if (!_updateElementsOnSave[element.GetType()].Contains(element))
                        _updateElementsOnSave[element.GetType()].Add(element);
                }
                else
                {
                    _updateElementsOnSave.TryAdd(element.GetType(), new List<ITable> { element });
                }
            }
        }

        public static void UpdateElement(ITable element, int characterId)
        {
            SaveTask.UpdateElement(element);
            return;
            /*if (characterId > 0)
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
                        _linkedCharactersUpdateElements.TryAdd(characterId, newDictionary);
                    }
                
            }
            else
            {
                Logger.Error("Unable to UpdateElement for characterId '" + characterId + "' !");
            }*/
        }

        #endregion

        #region RemoveElement

        public static void RemoveElement(ITable element)
        {
            if (element != null)
            {
                List<ITable> elements = new List<ITable>() { element };
                Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(element.GetType()), DatabaseAction.Remove, elements.ToArray());
                RemoveFromCache(element);
            }


            /*if (_newElements.ContainsKey(element.GetType()) && _newElements[element.GetType()].Contains(element))
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
                    _removeElements.TryAdd(element.GetType(), new List<ITable> { element });
                }
            

            RemoveFromCache(element);*/
        }

        public static void RemoveElement(ITable element, int characterId)
        {
            SaveTask.RemoveElement(element);
            return;
            /*if (characterId > 0)
            {
                if (element == null)
                    return;

                
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
                        _linkedCharactersRemoveElements.TryAdd(characterId, newDictionary);
                    }
                

                RemoveFromCache(element);
            }
            else
            {
                Logger.Error("Unable to RemoveElement for characterId '" + characterId + "' !");
            }*/
        }

        public static void RemoveElementWithoutDelay(ITable element)
        {
            if (element != null)
            {
                List<ITable> elements = new List<ITable>() { element };
                Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(element.GetType()), DatabaseAction.Remove, elements.ToArray());
                RemoveFromCache(element);
            }
        }

        public static void AddElementWithoutDelay(ITable element)
        {
            if (element != null)
            {
                List<ITable> elements = new List<ITable>() { element };
                Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(element.GetType()), DatabaseAction.Add, elements.ToArray());
                RemoveFromCache(element);
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
            onSave = true;
            SaveTaskTimer.Stop();
            try
            {
                var types = _updateElements.Keys.ToList();
                foreach (var type in types)
                {
                    List<ITable> elements;
                    elements = _updateElements[type];
                    try
                    {
                        var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Update, elements.ToArray());
                    }
                    catch (Exception e) { Logger.Error(e.ToString()); }


                    var attribute = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute));

                    if (attribute != null && !attribute.letInUpdateField)
                        _updateElements[type] = _updateElements[type].Skip(elements.Count).ToList();
                }

                SaveTaskTimer.Start();
                if (OnSaveEnded != null)
                    OnSaveEnded(stopWatch.Elapsed.Seconds);
                CanSaveLinkedElements = true;
                //Avant de terminé la save on vide le tableau des updates.
                _updateElements.Clear();
                onSave = false;
                _updateElements
                //Save terminé on remais toute la list temporaire dans la bonne liste

            }
            catch (Exception e)
            {
                CanSaveLinkedElements = true;
                Logger.Error("[SAVING WORLD] " + e.Message);
            }
        }

        public static void SaveCharacter(int characterId, bool forceSave = false)
        {
            
          /*  if (CanSaveLinkedElements || forceSave)
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

                                    elements = _linkedCharactersRemoveElements[characterId][type];

                                try
                                {
                                    var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Remove, elements.ToArray());
                                }
                                catch (Exception e) { Logger.Error(e.Message); }

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

                                elements = _linkedCharactersNewElements[characterId][type];

                            try
                            {
                                var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Add, elements.ToArray());
                            }
                            catch (Exception e) { Logger.Error(e.ToString()); }

                                _linkedCharactersNewElements[characterId][type] = _linkedCharactersNewElements[characterId][type].Skip(elements.Count).ToList();

                        }
                    }

                    if (_linkedCharactersUpdateElements.ContainsKey(characterId))
                    {
                        var types = _linkedCharactersUpdateElements[characterId].Keys.ToList();
                        foreach (var type in types)
                        {
                            List<ITable> elements;
                                elements = _linkedCharactersUpdateElements[characterId][type];
                            try
                            {
                                var writer = Activator.CreateInstance(typeof(DatabaseWriter<>).MakeGenericType(type), DatabaseAction.Update, elements.ToArray());
                            }
                            catch (Exception e) { Logger.Error(e.ToString()); }

                            
                                var attribute = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute));

                                if (attribute != null && !attribute.letInUpdateField)
                                    _linkedCharactersUpdateElements[characterId][type] = _linkedCharactersUpdateElements[characterId][type].Skip(elements.Count).ToList();
                            
                        }
                    }

                    Logger.Log(string.Format("Character '{0}' saved in {1}ms !", characterId, stopWatch.ElapsedMilliseconds));
                }
                catch (Exception e)
                {
                    Logger.Error("[SAVING CHARACTER] " + e.Message);
                }
            }
            */
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
