using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.ORM;

namespace Symbioz.World.Records.Ignored
{
    [Table("CharactersIgnored")]
    public class IgnoredRecord : ITable
    {
        public static List<IgnoredRecord> CharactersIgnored = new List<IgnoredRecord>();
        public static int IdToAdd = 0;

        [Primary]
        public int Id;
        public int AccountId;
        public int IgnoredAccountId;

        public IgnoredRecord(int id, int accountId, int ignoredAccountId)
        {
            this.Id = id;
            this.AccountId = accountId;
            this.IgnoredAccountId = ignoredAccountId;
        }
    }
}
