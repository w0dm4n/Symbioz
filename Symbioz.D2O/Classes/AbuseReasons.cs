// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class AbuseReasons : ID2OClass
    {
        [Cache]
        public static List<AbuseReasons> AbuseReasonss = new List<AbuseReasons>();
        public Int32 _abuseReasonId;
        public Int32 _mask;
        public Int32 _reasonTextId;
        public AbuseReasons(Int32 _abuseReasonId, Int32 _mask, Int32 _reasonTextId)
        {
            this._abuseReasonId = _abuseReasonId;
            this._mask = _mask;
            this._reasonTextId = _reasonTextId;
        }
    }
}
