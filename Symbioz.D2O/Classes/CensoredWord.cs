// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class CensoredWord : ID2OClass
    {
        [Cache]
        public static List<CensoredWord> CensoredWords = new List<CensoredWord>();
        public Int32 id;
        public Int32 listId;
        public String language;
        public String word;
        public Boolean deepLooking;
        public CensoredWord(Int32 id, Int32 listId, String language, String word, Boolean deepLooking)
        {
            this.id = id;
            this.listId = listId;
            this.language = language;
            this.word = word;
            this.deepLooking = deepLooking;
        }
    }
}
