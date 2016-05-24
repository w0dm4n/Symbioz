using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using Symbioz.Helper;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace Symbioz
{
    public static class Extensions
    {
        static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static int Percentage(this int value,int percentage)
        {
            return (int)((double)value * (double)((double)percentage / (double)100));
        }
        public static bool Contains<T>(this List<T> list, Predicate<T> predicate)
        {
            return list.Find(predicate) != null;
        }
        public static T Value<T>(this string[] array, int index)
        {
            return (T)Convert.ChangeType(array[index], typeof(T));
        }
        public static int PopNextId<T>(this List<T> list, Converter<T, int> converter)
        {
            Locker.EnterReadLock();
            try
            {
                List<int> ids = list.ConvertAll<int>(converter);
                ids.Sort();
                if (ids.Count() == 0)
                    return 1;
                return ids.Last() + 1;
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }
        public static Type[] GetTypesWithAttribute(this Assembly assembly, Type attributeType)
        {
            return assembly.GetTypes().ToList().FindAll(x => x.HasAttribute(attributeType)).ToArray();
        }
        public static bool HasAttribute(this Type type, Type attributeType)
        {
            return type.GetCustomAttribute(attributeType) != null;
        }
        public static List<T> ListCast<TList, T>(this List<TList> list)
        {
            return list.ConvertAll<T>(x => (T)Convert.ChangeType(x, typeof(T)));
        }
        public static bool IsEnumerable(this object obj)
        {
            return obj.GetType().GetInterfaces().Contains(typeof(IEnumerable));
        }
        public static bool IsNull(this object obj)
        {
            if (obj == null)
                return true;
            else
                return false;
        }
        public static string ToSplitedString(this IList list)
        {
            string str = string.Empty;
            foreach (var value in list)
            {
                str += value.ToString() + ",";
            }
            str= str.Remove(str.Length - 1);
            return str;
        }
        public static T Random<T>(this List<T> list)
        {
           
            if (list.Count > 0)
            {
                return list[new AsyncRandom().Next(0, list.Count)];
            }
            else
                return default(T);
        }
        public static List<T> ToSplitedList<T>(this string str, char separator = ',')
        {
            var list = new List<T>();
            foreach (var value in str.Split(separator))
            {
                list.Add((T)Convert.ChangeType(value, typeof(T)));
            }
            return list;
        }
        public static bool ScramEqualList<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first.Count() != second.Count())
                return false;
            int i = 0;
            foreach (var data in first)
            {
                if (!second.ElementAt(i).Equals(data))
                    return false;
                i++;
            }
            return true;
        }
        public static bool ScramEqualDictionary<T, T2>(this Dictionary<T, T2> first, Dictionary<T, T2> second)
        {
            if (first.Count != second.Count)
                return false;
            foreach (var data in first)
            {
                if (second.ContainsKey(data.Key))
                {
                    var value = second.First(x => x.Key.Equals(data.Key));
                    if (!value.Value.Equals(data.Value))
                        return false;
                }
                else
                    return false;
            }
            return true;
        }
        public static string AsIpString(this IPEndPoint endpoint)
        {
            return endpoint.Address + ":" + endpoint.Port;
        }
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type.FindInterfaces(new TypeFilter(FilterByName), interfaceType).Length > 0;
        }
        private static bool FilterByName(Type typeObj, object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }
        public static T[] ParseCollection<T>(string str, Func<string, T> converter)
        {
            T[] result;
            if (string.IsNullOrEmpty(str))
            {
                result = new T[0];
            }
            else
            {
                int num = 0;
                int num2 = str.IndexOf(',', 0);
                if (num2 == -1)
                {
                    result = new T[]
					{
						converter(str)
					};
                }
                else
                {
                    T[] array = new T[str.CountOccurences(',', num, str.Length - num) + 1];
                    int num3 = 0;
                    while (num2 != -1)
                    {
                        array[num3] = converter(str.Substring(num, num2 - num));
                        num = num2 + 1;
                        num2 = str.IndexOf(',', num);
                        num3++;
                    }
                    array[num3] = converter(str.Substring(num, str.Length - num));
                    result = array;
                }
            }
            return result;
        }
        public static Delegate CreateDelegate(this MethodInfo method, params Type[] delegParams)
        {
            Type[] array = (
                from p in method.GetParameters()
                select p.ParameterType).ToArray<Type>();
            if (delegParams.Length != array.Length)
            {
                throw new Exception("Method parameters count != delegParams.Length");
            }
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, null, new Type[]
			{
				typeof(object)
			}.Concat(delegParams).ToArray<Type>(), true);
            ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
            if (!method.IsStatic)
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(method.DeclaringType.IsClass ? OpCodes.Castclass : OpCodes.Unbox, method.DeclaringType);
            }
            for (int i = 0; i < delegParams.Length; i++)
            {
                iLGenerator.Emit(OpCodes.Ldarg, i + 1);
                if (delegParams[i] != array[i])
                {
                    if (!array[i].IsSubclassOf(delegParams[i]) && !HasInterface(array[i], delegParams[i]))
                    {
                        throw new Exception(string.Format("Cannot cast {0} to {1}", array[i].Name, delegParams[i].Name));
                    }
                    iLGenerator.Emit(array[i].IsClass ? OpCodes.Castclass : OpCodes.Unbox, array[i]);
                }
            }
            iLGenerator.Emit(OpCodes.Call, method);
            iLGenerator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(Expression.GetActionType(new Type[]
			{
				typeof(object)
			}.Concat(delegParams).ToArray<Type>()));
        }
    }
}
