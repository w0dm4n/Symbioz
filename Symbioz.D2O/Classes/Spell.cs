// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Spell : ID2OClass
    {
        [Cache]
        public static List<Spell> Spells = new List<Spell>();
        public Int32 id;
        public Int32 nameId;
        public Int32 descriptionId;
        public Int32 typeId;
        public String scriptParams;
        public String scriptParamsCritical;
        public Int32 scriptId;
        public Int32 scriptIdCritical;
        public Int32 iconId;
        public UInt32[] spellLevels;
        public Boolean verbose_cast;
        public Spell(Int32 id, Int32 nameId, Int32 descriptionId, Int32 typeId, String scriptParams, String scriptParamsCritical, Int32 scriptId, Int32 scriptIdCritical, Int32 iconId, UInt32[] spellLevels, Boolean verbose_cast)
        {
            this.id = id;
            this.nameId = nameId;
            this.descriptionId = descriptionId;
            this.typeId = typeId;
            this.scriptParams = scriptParams;
            this.scriptParamsCritical = scriptParamsCritical;
            this.scriptId = scriptId;
            this.scriptIdCritical = scriptIdCritical;
            this.iconId = iconId;
            this.spellLevels = spellLevels;
            this.verbose_cast = verbose_cast;
        }
    }
}
