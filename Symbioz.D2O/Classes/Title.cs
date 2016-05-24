// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Title : ID2OClass
    {
        [Cache]
        public static List<Title> Titles = new List<Title>();
        public Int32 id;
        public Int32 nameMaleId;
        public Int32 nameFemaleId;
        public Boolean visible;
        public Int32 categoryId;
        public Title(Int32 id, Int32 nameMaleId, Int32 nameFemaleId, Boolean visible, Int32 categoryId)
        {
            this.id = id;
            this.nameMaleId = nameMaleId;
            this.nameFemaleId = nameFemaleId;
            this.visible = visible;
            this.categoryId = categoryId;
        }
    }
}
