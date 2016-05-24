// automatic generation Symbioz.Sync 2015

using Symbioz.D2O.InternalClasses;
using Symbioz.DofusProtocol.D2O;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class SkinPosition : ID2OClass
    {
        [Cache]
        public static List<SkinPosition> SkinPositions = new List<SkinPosition>();
        public Int32 id;
        public TransformData[] transformation;
        public String[] clip;
        public UInt32[] skin;
        public SkinPosition(Int32 id,object[] transformation, String[] clip, UInt32[] skin)
        {
            this.id = id;
            this.transformation = transformation.Cast<TransformData>().ToArray();
            this.clip = clip;
            this.skin = skin;
        }
    }
}
