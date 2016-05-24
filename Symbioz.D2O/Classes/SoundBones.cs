// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SoundBones : ID2OClass
    {
        [Cache]
        public static List<SoundBones> SoundBoness = new List<SoundBones>();
        public Int32 id;
        public String[] keys;
        public ArrayList[] values;
        public SoundBones(Int32 id, String[] keys, ArrayList[] values)
        {
            this.id = id;
            this.keys = keys;
            this.values = values;
        }
    }
}
