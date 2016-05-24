//using Symbioz.Core;
//using Symbioz.Core.Startup;
//using Symbioz.Network.Servers;
//using Symbioz.SSync;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Timers;

//namespace Symbioz.Network
//{
//    class CyclicConnectionStateTest
//    {
//        static Timer TestTimer = new Timer(100000);

//        [StartupInvoke(StartupInvokeType.Cyclics)]
//        public static void StartTest()
//        {
//            TestTimer.Elapsed += TestTimer_Elapsed;
//            TestTimer.Start();
//        }

//        static void TestTimer_Elapsed(object sender, ElapsedEventArgs e)
//        {
//            SSyncClient client = new SSyncClient();
//            client.OnFailedToConnect += client_OnFailedToConnect;
//            client.Connect(ConfigurationManager.Instance.Host, ConfigurationManager.Instance.AuthPort);
//        }

//        static void client_OnFailedToConnect(Exception ex)
//        {
//            var fileName = Assembly.GetExecutingAssembly().Location;
//            System.Diagnostics.Process.Start(fileName);
//            Environment.Exit(0);
//        }
//    }
//}
