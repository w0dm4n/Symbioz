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

        public static void UpdateElement(this ITable table)
        {
            SaveTask.UpdateElement(table);
        }
        public static void AddElement(this ITable table, bool addtolist = true)
        {
            SaveTask.AddElement(table, addtolist);
        }
        public static void RemoveElement(this ITable table, bool removefromlist = true)
        {
            SaveTask.RemoveElement(table, removefromlist);
        }
    }
}
