using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Linq;
using System.Linq.Expressions;

namespace Symbioz.DofusProtocol.Messages
{
    public static class MessageReceiver
    {
        [Serializable]
        public class MessageNotFoundException : Exception
        {
            public MessageNotFoundException()
            {
            }
            public MessageNotFoundException(string message)
                : base(message)
            {
            }
            public MessageNotFoundException(string message, Exception inner)
                : base(message, inner)
            {
            }
            protected MessageNotFoundException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }

        private static readonly Dictionary<ushort, Type> Messages = new Dictionary<ushort, Type>();
        private static readonly Dictionary<ushort, Func<Message>> Constructors = new Dictionary<ushort, Func<Message>>();

        public static void Initialize()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(MessageReceiver));
            foreach (var type in assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Message))))
            {
                FieldInfo field = type.GetField("Id");
                if (field != null)
                {
                    ushort num = (ushort)field.GetValue(type);
                    if (MessageReceiver.Messages.ContainsKey(num))
                    {
                        throw new AmbiguousMatchException(string.Format("MessageReceiver() => {0} item is already in the dictionary, old type is : {1}, new type is  {2}", num, MessageReceiver.Messages[num], type));
                    }
                    MessageReceiver.Messages.Add(num, type);
                    ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                    {
                        throw new Exception(string.Format("'{0}' doesn't implemented a parameterless constructor", type));
                    }
                    MessageReceiver.Constructors.Add(num, constructor.CreateDelegate<Func<Message>>());
                }
            }
        }

        public static Message BuildMessage(ushort id, ICustomDataInput reader)
        {
            if (!MessageReceiver.Messages.ContainsKey(id))
            {
                return null;
            }
            Message message = MessageReceiver.Constructors[id]();
            if (message == null)
            {
                return null;
            }
            message.Unpack(reader);
            return message;
        }

        public static Type GetMessageType(ushort id)
        {
            if (!MessageReceiver.Messages.ContainsKey(id))
            {
                return null;
            }
            return MessageReceiver.Messages[id];
        }
        public static int Count()
        {
            return Messages.Count();
        }
    }
    public static class ReflectionHelper
    {
        public static T CreateDelegate<T>(this ConstructorInfo ctor)
        {
            List<ParameterExpression> list = Enumerable.ToList<ParameterExpression>(Enumerable.Select<ParameterInfo, ParameterExpression>((IEnumerable<ParameterInfo>)ctor.GetParameters(), (Func<ParameterInfo, ParameterExpression>)(param => Expression.Parameter(param.ParameterType))));
            return Expression.Lambda<T>((Expression)Expression.New(ctor, (IEnumerable<Expression>)list), (IEnumerable<ParameterExpression>)list).Compile();
        }
    }
}
