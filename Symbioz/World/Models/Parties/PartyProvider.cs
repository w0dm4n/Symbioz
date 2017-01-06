using Symbioz.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Parties
{
    public class PartyProvider
    {
        private static UIDProvider idProvider = new UIDProvider();

        public static int GetIdParty()
        {
            return idProvider.Pop();
        }

        public static void Remove(Party party)
        {
            idProvider.Push((int)party.Id);
        }
    }
}
