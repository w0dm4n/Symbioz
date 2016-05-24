using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.ORM;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Helper;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Provider;
using Symbioz.World.Records.Companions;
using Symbioz.Providers.Conditions;
using Symbioz.World.Records.Items;
using Symbioz.World.Models.Items;

namespace Symbioz.World.Models
{
    public class Inventory
    {
        public const short PET_SIZE = 80;

        public static CharacterInventoryPositionEnum[] DOFUS_POSITIONS = new CharacterInventoryPositionEnum[]
        {
            CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1,
            CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2,
            CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3,
            CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4,
            CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5,
            CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6,
        };

        Character Character { get; set; }
        public List<CharacterItemRecord> Items = new List<CharacterItemRecord>();
        public Inventory(Character character)
        {
            this.Character = character;
            this.Items = CharacterItemRecord.GetCharacterItems(Character.Id);
            this.Initialize();
        }
        public void Initialize()
        {
            #region Companions
            var item = GetItemByPosition(CharacterInventoryPositionEnum.INVENTORY_POSITION_COMPANION);
            if (item != null)
            {
                var companionEffect= item.GetFirstEffect<ObjectEffectInteger>(EffectsEnum.Eff_Companion);
                Character.EquipedCompanion = CompanionRecord.GetCompanion(companionEffect.value);
            }
            #endregion
        }
        public void Add(BankItemRecord item)
        {
            var newItem = new CharacterItemRecord(item.UID, 63, item.GID, Character.Id, item.Quantity, item.GetEffects());
            Add(newItem);
        }
        public void Add(BankItemRecord item, uint quantity)
        {
            var newItem = new CharacterItemRecord(item.UID, 63, item.GID, Character.Id, quantity, item.GetEffects());
            Add(newItem);
        }
        public void Add(BidShopItemRecord item)
        {
            var newItem = new CharacterItemRecord(item.UID, 63, item.GID, Character.Id, item.Quantity, item.GetEffects());
            Add(newItem);
        }
        public void Add(CharacterItemRecord item, bool refresh = true)
        {

            var existingItem = Items.ExistingItem(item);
            if (existingItem == null)
            {
                Items.Add(item);
                if (!CharacterItemRecord.CharactersItems.Contains(item))
                    SaveTask.AddElement(item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;
                SaveTask.UpdateElement(existingItem);
            }
            if (refresh)
            {
                Refresh();
                Character.RefreshShortcuts();
            }
        }
        public CharacterItemRecord Add(ushort gid, uint quantity, bool notif = true, bool refresh = true)
        {
            ItemRecord template = ItemRecord.GetItem(gid);
            if (template == null)
            {
                Character.Reply("L'item n'éxiste pas");
                return null;
            }
            var newObjitem = template.GenerateRandomObjectItem();
            newObjitem.quantity = quantity;
            CharacterItemRecord newItem = new CharacterItemRecord(newObjitem, Character.Id);
            ItemCustomEffects.Instance.Init(newItem);
            Add(newItem, refresh);
            if (notif)
                Character.Reply("Vous avez obtenu " + quantity + " " + template.Name);
            return newItem;
        }
        public CharacterItemRecord EquipedItem(byte position)
        {
            return Items.Find(x => x.Position == position);
        }
        void RemoveItemSkin(CharacterItemRecord record, ItemRecord template)
        {
            if (template.AppearanceId != 0)
            {
                switch (template.Type)
                {
                    case ItemTypeEnum.PET:
                        Character.Look.subentities.RemoveAll(x => x.bindingPointCategory == (sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_PET);
                        return;
                    case ItemTypeEnum.PETSMOUNT:
                        Character.Look = Character.Look.RiderToCharacter();
                        break;
                    default:
                        if (record.ContainEffect(EffectsEnum.Eff_Mimicry))
                        {
                            ObjectEffectInteger mimicryEffect = record.GetFirstEffect<ObjectEffectInteger>(EffectsEnum.Eff_Mimicry);
                            ushort mimicryAppId = (ushort)ItemRecord.GetItem(mimicryEffect.value).AppearanceId;

                            Character.Look.RemoveSkin(mimicryAppId);
                        }
                        else
                        {
                            Character.Look.RemoveSkin((ushort)template.AppearanceId);
                        }
                        break;
                }

            }
        }
        void AddItemSkin(CharacterItemRecord record, ItemRecord template)
        {
            if (template.AppearanceId != 0)
            {
                switch (template.Type)
                {
                    case ItemTypeEnum.PET:
                        Character.Look.subentities.Add(new SubEntity((sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_PET,
                       0, ContextActorLook.SimpleBonesLook((ushort)template.AppearanceId, PET_SIZE).ToEntityLook()));
                        return;
                    case ItemTypeEnum.PETSMOUNT:
                        Character.Look = Character.Look.CharacterToRider((ushort)template.AppearanceId, new List<ushort>(), Character.Look.indexedColors.Take(3).ToList(), 100);
                        return;
                    default:
                        if (record.ContainEffect(EffectsEnum.Eff_Mimicry))
                        {
                            var mimicryEffect = record.GetFirstEffect<ObjectEffectInteger>(EffectsEnum.Eff_Mimicry);
                            var mimicryAppId = (ushort)ItemRecord.GetItem(mimicryEffect.value).AppearanceId;
                            Character.Look.AddSkin(mimicryAppId);
                        }
                        else
                        {
                            Character.Look.AddSkin((ushort)template.AppearanceId);
                        }
                        break;
                }
            }

        }
        public bool HasItem(ushort gid)
        {
            var item = Items.Find(x => x.GID == gid);
            if (item == null)
                return false;
            else
                return true;
        }
        public CharacterItemRecord GetItemByPosition(CharacterInventoryPositionEnum position)
        {
            return Items.Find(x => x.Position == (byte)position);
        }
        bool CheckRingStacks(CharacterItemRecord item, byte newposition)
        {
            var pos = (CharacterInventoryPositionEnum)newposition;
            if (pos == CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT)
            {
                var current = GetItemByPosition(CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT);
                if (current != null)
                {
                    if (current.GID == item.GID)
                    {
                        return true;
                    }
                }
            }
            if (pos == CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT)
            {
                var current = GetItemByPosition(CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT);
                if (current != null)
                {
                    if (current.GID == item.GID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool CheckDofusStacks(CharacterItemRecord item, byte newposition)
        {
            return false; // TODO
        }
        public void EquipItem(CharacterItemRecord item, ItemRecord template, byte newposition, uint quantity)
        {
            if (!ConditionProvider.ParseAndEvaluate(Character.Client, template.Criteria))
            {
                Character.Reply("Vous n'avez pas les critères nessessaire pour équiper cet objet");
                return;
            }
            if (CheckRingStacks(item, newposition))
            {
                Character.Reply("Vous avez déja équipé cet anneau!");
                return;
            }
            if (CheckDofusStacks(item, newposition))
            {
                Character.Reply("Vous avez déja équipé ce dofus");
                return;
            }
            if (DOFUS_POSITIONS.Contains((CharacterInventoryPositionEnum)item.Position) && DOFUS_POSITIONS.Contains((CharacterInventoryPositionEnum)newposition))
                return;
            if ((CharacterInventoryPositionEnum)newposition == CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD)
            {
                var weapon = GetEquipedWeapon();
                if (weapon != null)
                {
                    if (WeaponRecord.GetWeapon(weapon.GID).TwoHanded)
                    {
                        Character.Reply("Vous devez deséquiper votre arme pour équiper le bouclier.");
                        return;
                    }
                }
            }
            if ((CharacterInventoryPositionEnum)newposition == CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)
            {
                var shield = GetItemByPosition(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD);
                if (WeaponRecord.GetWeapon(item.GID).TwoHanded)
                {
                    Character.Reply("Vous devez enlevé votre bouclier pour équiper votre arme.");
                    return;
                }
            }
            var equiped = EquipedItem(newposition);
            if (equiped != null)
            {
                UnequipItem(equiped, 63, equiped.GetTemplate(), quantity);
                SaveTask.UpdateElement(equiped);
            }
            if (item.Quantity == 1)
            {
                item.Position = newposition;
                SaveTask.UpdateElement(item);
                AddItemSkin(item, template);
                ItemEffectsProvider.AddEffects(Character.Client, item.GetEffects());

            }
            else
            {
                var items = ItemCut.Cut(item, quantity, newposition);
                Add(items.newItem);
                ItemEffectsProvider.AddEffects(Character.Client, items.BaseItem.GetEffects());
                AddItemSkin(item, template);
            }
            Character.RefreshGroupInformations();

        }
        public void UnequipItem(CharacterItemRecord item, byte newposition, ItemRecord template, uint quantity)
        {
            var existing = Items.ExistingItem(item);
            if (existing == null)
            {
                item.Position = newposition;
                SaveTask.UpdateElement(item);
                ItemEffectsProvider.RemoveEffects(Character.Client, item.GetEffects());
                RemoveItemSkin(item, template);
            }
            else
            {
                if (item.UID != existing.UID)
                {
                    existing.Quantity += quantity;
                    RemoveItem(item.UID, item.Quantity);
                    ItemEffectsProvider.RemoveEffects(Character.Client, item.GetEffects());
                    RemoveItemSkin(item, template);
                }
                else
                {
                    Character.NotificationError("Spamming ItemMove!");
                }
            }
            Character.RefreshGroupInformations();
        }
        public CharacterItemRecord GetEquipedWeapon()
        {
            return Items.Find(x => x.Position == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
        }
        public void MoveItem(uint uid, byte newposition, uint quantity)
        {
            if (Character.IsFighting)
            {
                Character.Reply("Impossible en de combat", true);
                return;
            }
            var item = GetItem(uid);
            var template = ItemRecord.GetItem(item.GID);
            if (newposition != 63)
            {
                if (Character.Record.Level >= template.Level)
                {
                    EquipItem(item, template, newposition, quantity);
                }
                else
                {
                    Character.Reply("Vous n'avez pas le niveau pour équiper cet objet");
                    return;
                }
            }
            else
            {
                UnequipItem(item, newposition, template, quantity);

            }
            Character.Client.Send(new ObjectMovementMessage(uid, newposition));
            Character.RefreshOnMapInstance();
            Character.RefreshStats();
            Refresh();
        }
        public void RemoveItem(uint id, uint quantity, bool refresh = true)
        {
            var item = GetItem(id);
            if (item == null)
            {
                Character.NotificationError("Impossible de retirer l'item, il n'éxiste pas...");
                return;
            }

            if (quantity == item.Quantity)
            {
                Items.Remove(item);
                SaveTask.RemoveElement(item);
                CharacterItemRecord.CharactersItems.Remove(item);
                Character.Client.Send(new ObjectDeletedMessage(item.UID));
            }
            else if (quantity < item.Quantity)
            {
                item.Quantity -= (uint)quantity;
                SaveTask.UpdateElement(item);
            }
            if (refresh)
                Refresh();
            Character.RefreshShortcuts();

        }
        public List<ObjectItem> ConvertAllObjectItems()
        {
            return Items.ConvertAll<ObjectItem>(x => x.GetObjectItem());
        }
        public List<ItemRecord> ConvertAllToTemplates()
        {
            List<ItemRecord> records = new List<ItemRecord>();
            foreach (var item in Items)
            {
                records.Add(ItemRecord.GetItem(item.GID));
            }
            return records;
        }
        public List<CharacterItemRecord> GetEquipedItems()
        {
            return Items.FindAll(x => x.Position != (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
        }
        public CharacterItemRecord GetItem(uint uid)
        {
            return Items.Find(x => x.UID == uid);
        }
        public uint GetWeight()
        {
            uint actual = 0;
            foreach (var item in Items)
            {
                var template = ItemRecord.GetItem(item.GID);
                for (int i = 0; i < item.Quantity; i++)
                {
                    actual += (uint)template.Weight;
                }

            }
            return actual;
        }
        public void InitializeForSaveTask()
        {
            Items.ForEach(x => SaveTask.UpdateElement(x));
        }
        public void Refresh()
        {
            Character.Client.Send(new InventoryContentMessage(ConvertAllObjectItems(), (uint)Character.Record.Kamas));
            Character.Client.Send(new InventoryWeightMessage(GetWeight(), 5000));
        }
    }
}
