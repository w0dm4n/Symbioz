// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Url : ID2OClass
    {
        [Cache]
        public static List<Url> Urls = new List<Url>();
        public Int32 id;
        public Int32 browserId;
        public String url;
        public String param;
        public String method;
        public Url(Int32 id, Int32 browserId, String url, String param, String method)
        {
            this.id = id;
            this.browserId = browserId;
            this.url = url;
            this.param = param;
            this.method = method;
        }
    }
}
