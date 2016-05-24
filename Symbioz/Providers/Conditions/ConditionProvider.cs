using Symbioz.Core.Startup;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.Conditions
{
    class ConditionProvider
    {
        public static Dictionary<string,Type> ConditionsTypes = new Dictionary<string, Type>();

        [StartupInvoke("Criterias",StartupInvokeType.Others)]
        public static void Intialize()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypesWithAttribute(typeof(ConditionAttribute)))
            {
                var attribute = type.GetCustomAttribute(typeof(ConditionAttribute)) as ConditionAttribute;
                ConditionsTypes.Add(attribute.Identifier, type);
            }
        }
        public static bool ParseAndEvaluate(WorldClient client,string conditions)
        {
            foreach (var condition in conditions.Split('&'))
            {
                if (condition == null || condition == string.Empty)
                    return true;
                string conditionIndentifier = new string(condition.Take(2).ToArray());
                var conditionHandler = ConditionsTypes.FirstOrDefault(x => x.Key == conditionIndentifier);
                if (conditionHandler.Value != null)
                {
                    var conditionObj = Activator.CreateInstance(conditionHandler.Value) as Condition;
                    conditionObj.ConditionFull = condition;
                    if (!conditionObj.Eval(client))
                        return false;
                }
                else
                {
                    client.Character.ShowNotification("Unknown condition indentifier: " + conditionIndentifier + ". Skeeping condition");
                    return true;
                }
            }
            return true;
            
        }
    }
}
