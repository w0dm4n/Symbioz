// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SpeakingItemText : ID2OClass
    {
        [Cache]
        public static List<SpeakingItemText> SpeakingItemTexts = new List<SpeakingItemText>();
        public Int32 textId;
        public Double textProba;
        public Int32 textStringId;
        public Int32 textLevel;
        public Int32 textSound;
        public String textRestriction;
        public SpeakingItemText(Int32 textId, Double textProba, Int32 textStringId, Int32 textLevel, Int32 textSound, String textRestriction)
        {
            this.textId = textId;
            this.textProba = textProba;
            this.textStringId = textStringId;
            this.textLevel = textLevel;
            this.textSound = textSound;
            this.textRestriction = textRestriction;
        }
    }
}
