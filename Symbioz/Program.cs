using Symbioz.SSync;
using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Core;
using Symbioz.Core.Startup;
using Symbioz.Enums;
using System.Threading;
using Symbioz.Helper;
using Symbioz.ORM;

using Symbioz.World.Records;
using Symbioz.World.Models.Items;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models;
using Symbioz.Network.Clients;
using System.IO;
using Symbioz.Providers.DataWriter;

namespace Symbioz
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Logger.OnStartup();
                Startup.Initialize();

                if (ConfigurationManager.Instance.SafeRun)
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                Startup.StartServers();
                DataWriterProvider.Instance.Init();

                new Thread(new ThreadStart(SymbiozCommands.HandleCommands)).Start();
            }
            catch (Exception error)
            {
                Logger.Log(error);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SaveTask.Save();
            Thread.Sleep(1000);
            Environment.Exit(1);
        }
    }
}
