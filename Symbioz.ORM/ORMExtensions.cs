using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz
{
    public static class ORMExtensions
    {

        public static void AddElement(this ITable table)
        {
            SaveTask.AddElement(table);
        }

        public static void AddElement(this ITable table, int characterId)
        {
            SaveTask.AddElement(table, characterId);
        }

        public static void UpdateElement(this ITable table)
        {
            SaveTask.UpdateElement(table);
        }

        public static void UpdateElement(this ITable table, int characterId)
        {
            SaveTask.UpdateElement(table, characterId);
        }

        public static void RemoveElement(this ITable table)
        {
            SaveTask.RemoveElement(table);
        }

        public static void RemoveElement(this ITable table, int characterId)
        {
            SaveTask.RemoveElement(table, characterId);
        }

        public static void RemoveElementWithoutDelay(this ITable table)
        {
            SaveTask.RemoveElementWithoutDelay(table);
        }
    }
}
