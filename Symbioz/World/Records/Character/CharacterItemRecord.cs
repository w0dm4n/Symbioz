using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.ORM;
using Symbioz.Providers;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.World.Models
{
    [Table("CharactersItems",true)]
    public class CharacterItemRecord : ITable
    {
        static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        public static List<CharacterItemRecord> CharactersItems = new List<CharacterItemRecord>();

        [Primary]
        public uint UID;
        [Update]
        public byte Position;

        public ushort GID;
        [Update]
        public int CharacterId;
        [Update]
        public uint Quantity;
        [Ignore]
        List<ObjectEffect> m_realEffect = new List<ObjectEffect>();
        [Update]
        public string Effects;
        public string EffectsLinkedToList { get { return EffectsToString(m_realEffect); } }
        public CharacterItemRecord(uint uid,byte position,ushort gid,int characterid,uint qty,string effects)
        {
            this.UID = uid;
            this.Position = position;
            this.GID = gid;
            this.CharacterId = characterid;
            this.Quantity = qty;
            this.Effects = effects;

            if (effects != string.Empty)
                this.m_realEffect = StringToObjectEffects(Effects);
            else
                this.m_realEffect = new List<ObjectEffect>();
        }
        public CharacterItemRecord(uint uid,byte position,ushort gid, int characterid, uint qty,IEnumerable<ObjectEffect> effects)
        {
            this.UID = uid;
            this.Position = position;
            this.GID = gid;
            this.CharacterId = characterid;
            this.Quantity = qty;
            this.m_realEffect = effects.ToList();
            this.Effects = EffectsToString(m_realEffect);
        }
        public CharacterItemRecord(ObjectItem objitem,int characterid)
        {
            this.UID = objitem.objectUID;
            this.Position = objitem.position;
            this.GID = objitem.objectGID;
            this.CharacterId = characterid;
            this.Quantity = objitem.quantity;
            this.m_realEffect = objitem.effects.ToList();
            this.Effects = EffectsToString(m_realEffect);
        }
        public BankItemRecord GetBankItem(int accountId)
        {
            return new BankItemRecord(UID, GID,accountId, Quantity, EffectsLinkedToList);
        }
        public ObjectItem GetObjectItem()
        {
            return new ObjectItem(Position, GID, m_realEffect, UID, Quantity);
        }
        public void RemoveAllEffect(EffectsEnum effect)
        {
            this.m_realEffect.RemoveAll(x => x.actionId == (ushort)effect);
            this.Effects = EffectsToString(m_realEffect);
            SaveTask.UpdateElement(this);
        }
        public void RemoveAllEffects()
        {
            m_realEffect.Clear();
            this.Effects = string.Empty;
            SaveTask.UpdateElement(this);
        }
        public void SetEffects(List<ObjectEffect> effects)
        {
            this.m_realEffect = effects;
            this.Effects = EffectsToString(m_realEffect);
            SaveTask.UpdateElement(this);
        }
        public T GetFirstEffect<T>(EffectsEnum effect) where T : ObjectEffect
        {
            return (T)m_realEffect.First(x => x.actionId == (ushort)effect);
        }
        public bool ContainEffect(EffectsEnum effect)
        {
            if (this.m_realEffect.Find(x => x.actionId == (ushort)effect) != null)
                return true;
            else
                return false;
        }
        public void AddEffects(List<ObjectEffect> effects)
        {
            this.m_realEffect.AddRange(effects);
            this.Effects = EffectsToString(m_realEffect);
            SaveTask.UpdateElement(this);
        }
        public List<ObjectEffect> GetEffects()
        {
            return m_realEffect;
        }
        public ItemRecord GetTemplate()
        {
            return ItemRecord.GetItem(GID);
        }
        public CharacterItemRecord ToMimicry(int newskinid)
        {
            return ItemEditor.AddEffectsAndClone(this, new List<ObjectEffect>() { new ObjectEffectInteger((ushort)EffectsEnum.Eff_Mimicry, (ushort)newskinid) },1);
        }
        public static List<CharacterItemRecord> GetCharacterItems(int characterid)
        {
            return CharactersItems.FindAll(x => x.CharacterId == characterid);
        }
        public static void RemoveAll(int characterid)
        {
            GetCharacterItems(characterid).ForEach(x => SaveTask.RemoveElement(x));
        }
        public static CharacterItemRecord GetItemByUID(uint uid)
        {
            return CharactersItems.Find(x => x.UID == uid);
        }
        public static uint PopNextUID()
        {
             Locker.EnterReadLock(); 
             try
             {
                 List<uint> uids = CharactersItems.ConvertAll<uint>(x => x.UID);
                 uids.AddRange(BidShopItemRecord.GetAllItemsUIDs());
                 uids.AddRange(BankItemRecord.GetAllItemsUIDs());
                 uids.Sort();
                 if (uids.Count == 0)
                     return 1;
                 return uids.Last() + 1;
             }
             finally
             {
                 Locker.ExitReadLock();
             }
            
        }
        public static string EffectsToString(List<ObjectEffect> effects) 
        {
            string str = string.Empty;
            foreach (var effect in effects)
            {
                if (effect is ObjectEffectInteger)
                {
                    var eff = (ObjectEffectInteger)effect;
                    str += ObjectEffectInteger.Id + "#" + eff.actionId + "#" + eff.value+"|";
                }
                if (effect is ObjectEffectDice)
                {
                    var eff = (ObjectEffectDice)effect;
                    str += ObjectEffectDice.Id + "#" + eff.actionId+"#"+ + eff.diceNum + "#" + eff.diceSide + "#" + eff.diceConst + "|";
                }
               
            }
            return str;
        }
      
        public static List<ObjectEffect> StringToObjectEffects(string str)
        {
            if (str == string.Empty)
                return new List<ObjectEffect>();
            str = str.Substring(0, str.Length - 1);
            List<ObjectEffect> results = new List<ObjectEffect>();
            foreach (var effect in str.Split('|'))
            {
                string[] splited = effect.Split('#');
                var typeId = int.Parse(splited[0]);
                if (typeId == ObjectEffectInteger.Id)
                {
                    results.Add(new ObjectEffectInteger(ushort.Parse(splited[1]), ushort.Parse(splited[2])));
                }
                if (typeId == ObjectEffectDice.Id)
                {
                    results.Add(new ObjectEffectDice(ushort.Parse(splited[1]), ushort.Parse(splited[2]), ushort.Parse(splited[3]), ushort.Parse(splited[4])));
                }
            }
            return results;
        }
       

        public CharacterItemRecord CloneAndGetNewUID()
        {
            return new CharacterItemRecord(PopNextUID(), 63, GID, CharacterId, Quantity, Effects);
        }
        public CharacterItemRecord CloneWithUID()
        {
            return new CharacterItemRecord(UID, 63, GID, CharacterId, Quantity, Effects);
        }
        
    }
}
