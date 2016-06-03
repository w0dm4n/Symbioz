using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Network
{
    class CharactersDisconnected
    {
        private static Dictionary<String, Character> Disconnect_list = new Dictionary<String, Character>();

        public static void add(Character c)
        {
            if (Disconnect_list.ContainsKey(c.Record.Name))
                return;
            Disconnect_list.Add(c.Record.Name, c); 
        }

        public static bool is_disconnected(String key)
        {
            if (Disconnect_list.ContainsKey(key))
                return true;
            return false;
        }

        public static Character get_Character_disconnected(String key)
        {
            if (Disconnect_list.ContainsKey(key))
                return (Disconnect_list[key]);
            return null;
        }

        public static void remove(String key)
        {
            if (Disconnect_list.ContainsKey(key))
                Disconnect_list.Remove(key);
        }
    }
}
