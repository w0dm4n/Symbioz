using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.RawData.Records
{
    [Table("BugReports",false)]
    public class BugReportRecord : ITable
    {
        public string Content;
        public BugReportRecord(string content)
        {
            this.Content = content;
        }
    }
}
