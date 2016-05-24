using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Symbioz
{
    public static class InternalExtensions
    {
        public static List<ExtendedSpellEffect> FindWhere(this List<ExtendedSpellEffect> effects, EffectsEnum effect)
        {
            return effects.FindAll(x => x.BaseEffect.EffectType == effect);
        }
        public static List<Fighter> Alives(this List<Fighter> fighters)
        {
            return fighters.FindAll(x => !x.Dead);
        }
        public static string XMLSerialize(this object obj)
        {
            YAXSerializer serializer = new YAXSerializer(obj.GetType());
            return serializer.Serialize(obj);
        }
        public static object XMLDeserialize(this Type objType, string content)
        {
            YAXSerializer serializer = new YAXSerializer(typeof(Configuration));
            return Convert.ChangeType(serializer.Deserialize(content), objType);
        }
        public static T XMLDeserialize<T>(this string content)
        {
            return (T)XMLDeserialize(typeof(T), content);
        }
        public static void SendTo(this IEnumerable<WorldClient> clients,Message message)
        {
            foreach (var client in clients)
            {
                client.Send(message);
            }
        }
    }
}
