using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Items
{
    public class CharacterKeyring
    {
        public int KeyId;
        public int KeyTimeUse;

        public CharacterKeyring(int keyId, int keyItemUse)
        {
            this.KeyId = keyId;
            this.KeyTimeUse = keyItemUse;
        }
    }
}
