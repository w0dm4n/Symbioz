using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.ORM;
using System.Threading;

namespace Symbioz.World.Records.Ignored
{
    [Table("CharactersIgnored")]
    public class IgnoredRecord : ITable
    {
        private static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static List<IgnoredRecord> CharactersIgnored = new List<IgnoredRecord>();

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

        public static int PopNextId()
        {
            Locker.EnterReadLock();
            try
            {
                var ids = CharactersIgnored.ConvertAll<int>(x => x.Id);
                ids.Sort();
                return ids.Count == 0 ? 1 : ids.Last() + 1;
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }
    }
}
