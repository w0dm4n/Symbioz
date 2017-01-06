using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("ShopLogs", true)]
    public class ShopLogsRecord : ITable
    {
        public static List<ShopLogsRecord> ShopLogs = new List<ShopLogsRecord>();
        public int Id;
        public int AccountId;
        public int CharacterId;
        public int ItemId;
        public string ItemName;

        public ShopLogsRecord(int id, int accountId, int characterId, int itemId, string itemName)
        {
            this.Id = id;
            this.AccountId = accountId;
            this.CharacterId = characterId;
            this.ItemId = itemId;
            this.ItemName = itemName;
        }

        public static void AddLogs(int accountId, int characterId, int itemId, string itemName)
        {
            int Id = 1;
            foreach (var tmp in ShopLogs)
                Id++;
            Id++;
            var logs = new ShopLogsRecord(Id, accountId, characterId, itemId, itemName);
            ShopLogs.Add(logs);
            SaveTask.AddElement(logs);
        }
    }
}
