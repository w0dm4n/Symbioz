using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("BankItems", true)]
    public class BankItemRecord : ITable
    {
        public static List<BankItemRecord> BankItems = new List<BankItemRecord>();
        [Primary]
        public uint UID;
        public ushort GID;
        public int AccountId;
        [Update]
        public uint Quantity;
        [Ignore]
        List<ObjectEffect> m_realEffect = new List<ObjectEffect>();
        public string Effects;
        [Ignore]
        public string EffectsLinkedToList { get { return CharacterItemRecord.EffectsToString(m_realEffect); } }

        public BankItemRecord(uint uid,ushort gid,int accountid,uint quantity,string effects)
        {
            this.UID = uid;
            this.GID = gid;
            this.AccountId = accountid;
            this.Quantity = quantity;
            this.Effects = effects;
            if (effects != string.Empty)
                this.m_realEffect = CharacterItemRecord.StringToObjectEffects(Effects);
            else
                this.m_realEffect = new List<ObjectEffect>();
        }
        public List<ObjectEffect> GetEffects()
        {
            return m_realEffect;
        }
        public ObjectItem GetObjectItem()
        {
            return new ObjectItem(63, GID, m_realEffect, UID, Quantity);
        }
        public static List<BankItemRecord> GetCharacterItems(int accountid)
        {
            return BankItems.FindAll(x => x.AccountId == accountid);
        }
        public static List<uint> GetAllItemsUIDs()
        {
            return BankItems.ConvertAll<uint>(x => x.UID);
        }

    }
}
