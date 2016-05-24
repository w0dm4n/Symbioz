// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SoundUi : ID2OClass
    {
        [Cache]
        public static List<SoundUi> SoundUis = new List<SoundUi>();
        public Int32 id;
        public String uiName;
        public String openFile;
        public String closeFile;
        public ArrayList subElements;
        public SoundUi(Int32 id, String uiName, String openFile, String closeFile, ArrayList subElements)
        {
            this.id = id;
            this.uiName = uiName;
            this.openFile = openFile;
            this.closeFile = closeFile;
            this.subElements = subElements;
        }
    }
}
