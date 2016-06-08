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
        public static List<FriendRecord> CharactersFriends = new List<FriendRecord>();
        public static int IdToAdd = 0;

        [Primary]
        public int Id;
        public int CharacterId;
        public int FriendCharacterId;

        public FriendRecord(int id, int characterId, int friendCharacterId)
        {
            this.Id = id;
            this.CharacterId = characterId;
            this.FriendCharacterId = friendCharacterId;
        }
    }
}
