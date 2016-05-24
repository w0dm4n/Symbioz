// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SoundUiHook : ID2OClass
    {
        [Cache]
        public static List<SoundUiHook> SoundUiHooks = new List<SoundUiHook>();
        public Int32 id;
        public String name;
        public SoundUiHook(Int32 id, String name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
