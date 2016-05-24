using Symbioz.Core.Startup;
using Symbioz.Providers.ActorIA.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA
{
    class IAActionsProvider
    {
        public static Dictionary<IAActionsEnum, AbstractIAAction> IAActions = new Dictionary<IAActionsEnum, AbstractIAAction>(); 
        [StartupInvoke("Artificial Intelligence",StartupInvokeType.Internal)]
        public static void Initialize()
        {
            var types = Assembly.GetAssembly(typeof(IAActionsProvider)).GetTypesWithAttribute(typeof(IAAction));
            foreach (var type in types)
            {
                IAAction attribute = type.GetCustomAttribute<IAAction>();
                IAActions.Add(attribute.Action,Activator.CreateInstance(type) as AbstractIAAction);
            }

        }
        public static AbstractIAAction GetAction(IAActionsEnum action)
        {
            return IAActions.FirstOrDefault(x => x.Key == action).Value;
        }
    }
}
