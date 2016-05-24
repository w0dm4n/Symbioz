using Symbioz.Core.Startup;
using Symbioz.World.Models.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.Marks
{
    class InteractionsManager
    {
        static Dictionary<MethodInfo, FighterEventType> EventMethods = new Dictionary<MethodInfo,FighterEventType>();
        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            foreach (var type in Assembly.GetAssembly(typeof(InteractionsManager)).GetTypes())
            {
                if (type.IsSubclassOf(typeof(MarkTrigger)))
                {
                    foreach (var method in type.GetMethods())
                    {
                        var attributes = method.GetCustomAttributes(typeof(InteractionAttribute), false);
                        if (attributes.Length > 0)
                        {
                            EventMethods.Add(method,((InteractionAttribute)attributes[0]).EventType);
                        }
                    }
                }
            }
        }
        public static Dictionary<MethodInfo,FighterEventType> GetEventsMethods(Type markType)
        {
            return EventMethods.ToList().FindAll(x => x.Key.DeclaringType == markType).ToDictionary(x=>x.Key,x=>x.Value);
        }
    }
}
