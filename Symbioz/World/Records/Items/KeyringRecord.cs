using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Items
{
    public class KeyringRecord
    {
        public int KeyId;
        public int KeyTimeUse;

        public KeyringRecord(int keyId, int keyItemUse)
        {
            this.KeyId = keyId;
            this.KeyTimeUse = keyItemUse;
        }
    }
}
