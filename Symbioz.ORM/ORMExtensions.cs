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

        public static void UpdateElement(this ITable table, bool waitingNextWorldSave = true)
        {
            SaveTask.UpdateElement(table, waitingNextWorldSave);
        }
        public static void AddElement(this ITable table, bool waitingNextWorldSave = true)
        {
            SaveTask.AddElement(table, waitingNextWorldSave);
        }
        public static void RemoveElement(this ITable table, bool waitingNextWorldSave = true)
        {
            SaveTask.RemoveElement(table, waitingNextWorldSave);
        }
    }
}
