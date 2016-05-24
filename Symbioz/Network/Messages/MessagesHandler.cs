using System.Diagnostics;
using System.Threading;
using System.Reflection.Emit;
using System.Linq.Expressions;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Symbioz.Core.Startup;
using Symbioz.Network.Clients;
using Symbioz.Helper;
using Symbioz.Utils;

namespace Symbioz.Network.Messages
{
    public class MessagesHandler
    {
        static Type[] HandlerMethodParameterTypes = new Type[] { typeof(Message), typeof(DofusClient) };

        static Dictionary<uint, Delegate> Handlers = new Dictionary<uint, Delegate>();

        public static void Handle(Message message, DofusClient client)
        {

            if (message.IsNull() && !client.IsNull())
            {
                Logger.Init2("[Rcv] Client " + client.SSyncClient.Ip + " send unknown datas, they wont be handled");
                return;
            }
            var handler = Handlers.FirstOrDefault(x => x.Key == message.MessageId);
            if (!handler.Value.IsNull())
            {
                {
                    if (ConfigurationManager.Instance.ShowProtocolMessages)
                        Logger.Write("[Rcv] Message: " + message.ToString(), ConsoleColor.Gray);
                    try
                    {

                        handler.Value.DynamicInvoke(null, message, client);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(string.Format("Unable to handle message {0} {1} : '{2}'", message.ToString(), handler.Value.Method.Name, ex.InnerException.ToString()));
                        ErrorLogsManager.AddLog(ex.InnerException.ToString(), client.SSyncClient.Ip);
                        client.SendRaw("forcedbugreport");
                    }
                }
            }
            else
            {
                if (ConfigurationManager.Instance.ShowProtocolMessages)
                    Logger.Log(string.Format("[Rcv] No Handler: ({0}) {1}", message.MessageId, message.ToString()));
            }
        }
        [StartupInvoke("Messages Handler", StartupInvokeType.Internal)]
        public static void Initialize()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(MessagesHandler));
            foreach (var item in assembly.GetTypes())
            {
                foreach (var subItem in item.GetMethods())
                {
                    var attribute = subItem.GetCustomAttribute(typeof(MessageHandlerAttribute));
                    if (attribute != null)
                    {
                        ParameterInfo[] parameters = subItem.GetParameters();
                        Type methodParameters = subItem.GetParameters()[0].ParameterType;
                        if (methodParameters.BaseType != null)
                        {
                            try
                            {
                                Delegate target = subItem.CreateDelegate(HandlerMethodParameterTypes);
                                FieldInfo field = methodParameters.GetField("Id");
                                Handlers.Add((ushort)field.GetValue(null), target);
                            }
                            catch
                            {
                                Logger.Error("Cannot register " + subItem.Name + " has message handler...");
                            }

                        }
                    }

                }
            }
        }
        public static Message BuildMessage(byte[] buffer)
        {
            var reader = new CustomDataReader(buffer);
            var messagePart = new MessagePart(false);

            if (messagePart.Build(reader))
            {
                Message message;
                try
                {
                    message = MessageReceiver.BuildMessage((ushort)messagePart.MessageId.Value, reader);
                    return message;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while building Message :" + ex.Message);
                    return null;
                }
                finally
                {
                    reader.Dispose();
                }
            }
            else
                return null;

        }
    }
}