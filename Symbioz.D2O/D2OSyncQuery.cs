using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O
{
    public class D2OSyncQueries
    {
        public D2OSyncQueries(string d2oname,List<string> queries)
        {
            this.D2OName = d2oname;
            this.Queries = queries;
        }
        public string D2OName { get; set; }
        public List<string> Queries = new List<string>();
    }
}
