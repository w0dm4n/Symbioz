﻿using Symbioz.Core;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World
{
    class CyclicSaveTask
    {
        [StartupInvoke("CyclicSaveTask", StartupInvokeType.Cyclics)]
        public static void Start()
        {
            SaveTask.OnSaveStarted += Cache_OnSaveStarted;
            SaveTask.OnSaveEnded += Cache_OnSaveEnded;
            SaveTask.Initialize(ConfigurationManager.Instance.CyclicSaveInterval);
        }

        static void Cache_OnSaveStarted()
        {
           Logger.Init("Sauvegarde en cours ...");
            /*if (WorldServer.Instance.ServerState != ServerStatusEnum.ONLINE)
                return;
            WorldServer.Instance.SetServerState(ServerStatusEnum.ONLINE);
            WorldServer.Instance.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 164, new string[0]));
            InitializeSave();*/
            /*WorldServer.Instance.SetServerState(ServerStatusEnum.ONLINE);
            WorldServer.Instance.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 164, new string[0]));*/
            var a = Assembly.GetAssembly(typeof(CyclicSaveTask));
            foreach (var type in a.GetTypes())
            {
                if (type.GetInterface("ITable") != null)
                {
                    foreach (var method in type.GetMethods())
                    {
                        if (method.GetCustomAttribute(typeof(BeforeSave)) != null)
                        {
                            method.Invoke(null, new object[0]);
                        }
                    }
                }
            }
           /* WorldServer.Instance.SetServerState(ServerStatusEnum.ONLINE);
            WorldServer.Instance.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 165, new string[0]));
            */
        }

        static void Cache_OnSaveEnded(int elapsed)
        {
          /*  Logger.Init2("Sauvegarde terminé (" + elapsed + ")s");
            if (WorldServer.Instance.ServerState != ServerStatusEnum.SAVING)
                return;
           
           WorldServer.Instance.SetServerState(ServerStatusEnum.ONLINE);
            WorldServer.Instance.Send(new TextInformationMessage((sbyte)TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 165, new string[0]));*/
        }

        public static void InitializeSave()
        {
            /*var a = Assembly.GetAssembly(typeof(CyclicSaveTask));
            foreach (var type in a.GetTypes())
            {
                if (type.GetInterface("ITable") != null)
                {
                    foreach (var method in type.GetMethods())
                    {
                        if (method.GetCustomAttribute(typeof(BeforeSave)) != null)
                        {
                            method.Invoke(null,new object[0]);
                        }
                    }
                }
            }*/
        }


    }
}
