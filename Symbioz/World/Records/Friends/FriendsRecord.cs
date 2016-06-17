using Symbioz.Core;
using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.Providers;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Friends
{
    [Table("CharactersFriends")]
    public class FriendRecord : ITable
    {
        private static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static List<FriendRecord> CharactersFriends = new List<FriendRecord>();

        [Primary]
        public int Id;
        public int AccountId;
        public int FriendAccountId;

        public FriendRecord(int id, int accountId, int friendAccountId)
        {
            this.Id = id;
            this.AccountId = accountId;
            this.FriendAccountId = friendAccountId;
        }

        public static int PopNextId()
        {
            Locker.EnterReadLock();
            try
            {
                var ids = CharactersFriends.ConvertAll<int>(x => x.Id);
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
