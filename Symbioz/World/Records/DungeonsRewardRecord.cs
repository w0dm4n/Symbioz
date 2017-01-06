using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("DungeonsReward", true)]
    public class DungeonsRewardRecord : ITable
    {
        public static List<DungeonsRewardRecord> DungeonsReward = new List<DungeonsRewardRecord>();
        public int map_id;
        public string item_id;
        public int gain_kamas;
        public string dungeon_name;
        public DungeonsRewardRecord(int map_id, string item_id, int gain_kamas, string dungeon_name)
        {
            this.map_id = map_id;
            this.item_id = item_id;
            this.gain_kamas = gain_kamas;
            this.dungeon_name = dungeon_name;
        }

        public static DungeonsRewardRecord RewardOnCurrentMap(int mapId)
        {
            foreach (var value in DungeonsRewardRecord.DungeonsReward)
            {
                if (value.map_id == mapId)
                    return value;
            }
            return null;
        }

        public static List<ItemRecord> itemToAdd(int mapId)
        {
            List<ItemRecord> Items = new List<ItemRecord>();
            foreach (var value in DungeonsRewardRecord.DungeonsReward)
            {
                if (value.map_id == mapId)
                {
                    string[] items = value.item_id.Split(',');
                    foreach (var item in items)
                    {
                        var itemRecord = ItemRecord.GetItem(Int32.Parse(item));
                        if (itemRecord != null)
                            Items.Add(itemRecord);
                    }
                }
            }
            return Items;
        }
    }
}