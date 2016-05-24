using Symbioz.RawData.RawMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Helper;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Utils;
using Symbioz.Core.Startup;

namespace Symbioz.RawData
{
    public class RawReceiver
    {
        private static readonly Dictionary<short, Type> Messages = new Dictionary<short, Type>();
        private static readonly Dictionary<short, Func<RawMessage>> Constructors = new Dictionary<short, Func<RawMessage>>();
        [StartupInvoke(StartupInvokeType.Others)]
        public static void Initialize()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(RawReceiver));
            foreach (var type in assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(RawMessage))))
            {
                FieldInfo field = type.GetField("Id");
                if (field != null)
                {
                    short num = (short)field.GetValue(type);
                    if (Messages.ContainsKey(num))
                    {
                        throw new AmbiguousMatchException(string.Format("RawMessageProvider() => {0} item is already in the dictionary, old type is : {1}, new type is  {2}", num, Messages[num], type));
                    }
                    Messages.Add(num, type);
                    ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                    {
                        throw new Exception(string.Format("'{0}' doesn't implemented a parameterless constructor", type));
                    }
                    Constructors.Add(num, constructor.CreateDelegate<Func<RawMessage>>());
                }
            }
        }
        public static RawMessage BuildMessage(short id,BigEndianReader reader)
        {
            if (!Messages.ContainsKey(id))
            {
                return null;
            }
            RawMessage message = Constructors[id]();
            if (message == null)
            {
                return null;
            }
            message.Deserialize(reader);
            return message;
        }

    }
}
