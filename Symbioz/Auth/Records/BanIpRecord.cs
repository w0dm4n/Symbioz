using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Auth.Records
{
    [Table("BanIp")]
    public class BanIpRecord : ITable
    {
        public static List<BanIpRecord> BanIp = new List<BanIpRecord>();

        public string Ip;

        public BanIpRecord(string ip)
        {
            this.Ip = ip;
        }
        public static void Add(string ip)
        {
            if (!IsBanned(ip))
            {
                new BanIpRecord(ip).AddElement();
            }
        }
        public static bool IsBanned(string ip)
        {
            return BanIp.Find(x => x.Ip == ip) != null;
        }
    }
}
