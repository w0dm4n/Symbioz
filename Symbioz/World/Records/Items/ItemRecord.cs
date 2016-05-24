using Symbioz.Core;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.ORM;
using Symbioz.World.Models;
using Symbioz.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("Items",true)]
    public class ItemRecord : ITable
    {
        public static List<ItemRecord> Items = new List<ItemRecord>();

        [Primary]
        public int Id;
        public int NameId;
        public string Name;
        public int TypeId;
        [Ignore]
        public ItemTypeEnum Type { get { return (ItemTypeEnum)TypeId; } }
        public int AppearanceId;
        public int Level;
        public int Price;
        public int Weight;
        public string Effects;
        [Ignore]
        public ItemEffectsParser RealEffects;
        public string Criteria;

        public ItemRecord(int id,int nameid,string name,int typeid,int appearanceid,int level,int price,int weight,string effects,string criteria)
        {
            this.Id = id;
            this.NameId = nameid;
            this.TypeId = typeid;
            this.AppearanceId = appearanceid;
            this.Level = level;
            this.Price = price;
            this.Weight = weight;
            this.Effects = effects;
            this.Name = name;
            this.RealEffects = new ItemEffectsParser(Effects);
            this.Criteria = criteria;
            
        }
        public ObjectItem GenerateRandomObjectItem()
        {
            return new ObjectItem(63, (ushort)Id, GenerateRandomEffect(), CharacterItemRecord.PopNextUID(),1);
        }
        public List<ObjectEffect> GenerateRandomEffect()
        {
            List<ObjectEffect> result = new List<ObjectEffect>();
            foreach (var eff in RealEffects.ObjectEffects)
            {
                if (WeaponRecord.WeaponsDamagesEffects.Contains((EffectsEnum)eff.actionId))
                {
                    result.Add(new ObjectEffectDice(eff.actionId, eff.diceNum, eff.diceSide, eff.diceConst));
                    continue;
                }
                if (eff.diceSide == 0 && eff.diceConst == 0)
                {
                    result.Add(new ObjectEffectInteger(eff.actionId, eff.diceNum));
                }
                else if (eff.diceConst != 0)
                {
                    result.Add(new ObjectEffectInteger(eff.actionId, eff.diceConst));
      
                }
                else
                {
                    if (eff.diceNum > eff.diceSide)
                    {
                        result.Add(new ObjectEffectInteger(eff.actionId, (ushort)eff.diceNum));
                    }
                    else
                    {
                        int value = (short)new AsyncRandom().Next(eff.diceNum, eff.diceSide + 1);
                        result.Add(new ObjectEffectInteger(eff.actionId, (ushort)value));
                    }
                }
            }
            return result;
        }
        public static ItemRecord GetItem(int gid)
        {
            return Items.Find(x => x.Id == gid);
        }
        public static List<ItemRecord> GetItemsByType(ItemTypeEnum type)
        {
            return Items.FindAll(x => x.Type == type);
        }
    }
}
