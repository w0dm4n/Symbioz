
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Helper;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Core.Startup
{
    class Startup 
    {
       
        public static void Initialize()
        {
            Logger.Init2("-- Initialisation --");
            Stopwatch watch = Stopwatch.StartNew();
            Assembly assembly = Assembly.GetAssembly(typeof(Startup));
            foreach (var pass in Enum.GetValues(typeof(StartupInvokeType)))
            {
                foreach (var item in assembly.GetTypes())
                {
                    var methods = item.GetMethods().ToList().FindAll(x => x.GetCustomAttribute(typeof(StartupInvoke), false) != null);
                    var attributes = methods.ConvertAll<KeyValuePair<StartupInvoke, MethodInfo>>(x => new KeyValuePair<StartupInvoke, MethodInfo>(x.GetCustomAttribute(typeof(StartupInvoke), false) as StartupInvoke, x));

                    var concerned = attributes.FindAll(x => x.Key.Type == (StartupInvokeType)pass);
                    foreach (var data in concerned)
                    {
                        if (!data.Key.Hided)
                        {
                            Logger.Log("[" + pass + "] Loading " + data.Key.Name + " ...");
                        }
                        Delegate del = Delegate.CreateDelegate(typeof(Action), data.Value);
                        try
                        {
                            del.DynamicInvoke();
                     
                            
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e.InnerException.Message);
                            Logger.Init("Unable to start Symbioz! Press any touch to view full error");
                            Console.ReadKey();
                            Logger.Error(e.InnerException.ToString());
                            Logger.Init("Press any key to exit!");
                            Console.ReadKey();
                            Environment.Exit(0);
                            return;
                        }
                    }


                }
            }
            watch.Stop();
            Logger.Init2("-- Initialisation Complete (" + watch.Elapsed.Seconds + "s) --");
        }
        [StartupInvoke("Messages",StartupInvokeType.Internal)]
        public static void RegisterMessages()
        {
            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();
        }
        public static void StartServers()
        {
            Singleton<AuthServer>.Instance.Start();
            Singleton<WorldServer>.Instance.Start();
        }
        
    }
}
