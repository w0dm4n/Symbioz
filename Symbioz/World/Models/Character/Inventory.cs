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
using Symbioz.World.Records.Alliances.Prisms.Modules;
using Symbioz.World.Records.Tracks;

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
                var companionEffect = item.GetFirstEffect<ObjectEffectInteger>(EffectsEnum.Eff_Companion);
                Character.EquipedCompanion = CompanionRecord.GetCompanion(companionEffect.value);
            }
            #endregion
        }
        public void Add(PrismModuleRecord item)
        {
            var newItem = new CharacterItemRecord((uint)item.UID, 63, (ushort)item.GID, Character.Id, 1, item.GetEffects());
            Add(newItem);
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
                {
                    SaveTask.AddElement(item, this.Character.Id);
                }
            }
            else
            {
                existingItem.Quantity += item.Quantity;
                SaveTask.UpdateElement(existingItem, this.Character.Id);
            }
            if (refresh)
            {
                Refresh();
                Character.RefreshShortcuts();
            }
        }

        public void AddItemRecordWithQuantity(CharacterItemRecord item, uint quantity, bool refresh = true, int characterId = 0)
        {
            var existingItem = Items.ExistingItem(item);
            item.CharacterId = characterId;
            item.Quantity = quantity;
            if (existingItem == null)
            {
                Items.Add(item);
                SaveTask.AddElement(item, this.Character.Id);
            }
            else
            {
                existingItem.Quantity += item.Quantity;
                SaveTask.UpdateElement(existingItem, this.Character.Id);
            }
            if (refresh)
            {
                ItemRecord template = ItemRecord.GetItem(item.GID);
                if (template != null)
                    Character.Reply("Vous avez obtenu " + quantity + " " + template.Name + " !");
                else
                    Character.Reply("Vous avez obtenu " + quantity + " " + WeaponRecord.GetWeapon(item.GID).Name + " !");
                Refresh();
                Character.RefreshShortcuts();
            }
        }

        public CharacterItemRecord Add(ushort gid, uint quantity, bool notif = true, bool refresh = true)
        {
            ItemRecord template = ItemRecord.GetItem(gid);
            if (template == null)
            {
                Character.Reply("L'item n'existe pas");
                return null;
            }
            var newObjitem = template.GenerateRandomObjectItem();
            newObjitem.quantity = quantity;
            CharacterItemRecord newItem = new CharacterItemRecord(newObjitem, Character.Id);
            ItemCustomEffects.Instance.Init(newItem);
            Add(newItem, refresh);
            if (notif)
                Character.Reply("Vous avez obtenu " + quantity + " " + template.Name + " !");
            return newItem;
        }

        public CharacterItemRecord AddWeapon(ushort gid, uint quantity, bool notif = true, bool refresh = true)
        {
            WeaponRecord template = WeaponRecord.GetWeapon(gid);
            if (template == null)
            {
                //Character.Reply("L'arme n'existe pas");
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

        void RemoveWeaponSkin(CharacterItemRecord record, WeaponRecord template)
        {
            if (template.AppearenceId != 0)
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
                            Character.Look.RemoveSkin((ushort)template.AppearenceId);
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


        void AddWeaponSkin(CharacterItemRecord record, WeaponRecord template)
        {
            if (template.AppearenceId != 0)
            {
                switch (template.Type)
                {
                    /*case ItemTypeEnum.PET:
                        Character.Look.subentities.Add(new SubEntity((sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_PET,
                       0, ContextActorLook.SimpleBonesLook((ushort)template.AppearenceId, PET_SIZE).ToEntityLook()));
                        return;
                    case ItemTypeEnum.PETSMOUNT:
                        Character.Look = Character.Look.CharacterToRider((ushort)template.AppearenceId, new List<ushort>(), Character.Look.indexedColors.Take(3).ToList(), 100);
                        return;*/
                    default:
                        if (record.ContainEffect(EffectsEnum.Eff_Mimicry))
                        {
                            var mimicryEffect = record.GetFirstEffect<ObjectEffectInteger>(EffectsEnum.Eff_Mimicry);
                            var mimicryAppId = (ushort)ItemRecord.GetItem(mimicryEffect.value).AppearanceId;
                            Character.Look.AddSkin(mimicryAppId);
                        }
                        else
                        {
                            Character.Look.AddSkin((ushort)template.AppearenceId);
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
            var record = ItemRecord.GetItem(item.GID);
            if (record.TypeId == 23 || record.TypeId == 151)
            {
                var items = this.GetEquipedItems();
                foreach (var tmp in items)
                    if (tmp.GID == item.GID)
                        return true;
            }
            return false;
        }
        
        bool CheckPosition(CharacterItemRecord item, byte newposition)
        {
            ItemRecord record = null;
            WeaponRecord recordWeapon = null;
            var items = this.GetEquipedItems();
            foreach (var tmp in items)
            {
                record = ItemRecord.GetItem(tmp.GID);
                if (record != null)
                {
                    if (tmp.Position == newposition)
                    {
                        return true;
                    }
                }
                else
                {
                    recordWeapon = WeaponRecord.GetWeapon(tmp.GID);
                    if (tmp.Position == newposition)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool CheckItemPosition(CharacterItemRecord item, byte newposition)
        {
            var record = ItemRecord.GetItem(item.GID);
            var weapon = WeaponRecord.GetWeapon(item.GID);
            if (record != null)
            {
                switch (record.TypeId)
                {
                    case 16:
                    case 113:
                        if (newposition == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT)
                            return false;
                        break;

                    case 9:
                        if (newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT ||
                            newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT)
                            return false;
                        break;

                    case 17:
                        if (newposition == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE)
                            return false;
                        break;

                    case 121:
                    case 18:
                        if (newposition == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS)
                            return false;
                        break;

                    case 1:
                        if (newposition == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET)
                            return false;
                        break;

                    case 82:
                        if (newposition == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD)
                            return false;
                        break;

                    case 10:
                        if (newposition == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT)
                            return false;
                        break;

                    case 11:
                        if (newposition == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS)
                            return false;
                        break;

                    case 169:
                        if (newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_COMPANION)
                            return false;
                        break;

                    case 23:
                    case 151:
                        if (newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1
                        || newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2
                        || newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3
                        || newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4
                        || newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5
                        || newposition == (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6)
                            return false;
                        break;

                    default:
                        break;
                }
            }
            else if (weapon != null)
            {
                if (newposition == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)
                    return false;
            }
            return true;
        }

        public void EquipItem(CharacterItemRecord item, ItemRecord template, byte newposition, uint quantity)
        {
            if (!ConditionProvider.ParseAndEvaluate(Character.Client, template.Criteria))
            {
                Character.ReplyError("Vous n'avez pas les critères nessessaire pour équiper cet objet");
                return;
            }
            if (CheckRingStacks(item, newposition))
            {
                Character.ReplyError("Vous avez déja équipé cet anneau !");
                return;
            }
            if (CheckDofusStacks(item, newposition))
            {
                Character.ReplyError("Impossible d'équipé cet objet !");
                return;
            }
            if (CheckPosition(item, newposition))
            {
                Character.ReplyError("Impossible d'équipé cet objet !");
                return;
            }

            if (CheckItemPosition(item, newposition))
            {
                Character.ReplyError("Impossible car la catégorie est incorrect.");
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
                SaveTask.UpdateElement(equiped, this.Character.Id);
            }
            if (item.Quantity == 1)
            {
                item.Position = newposition;
                SaveTask.UpdateElement(item, this.Character.Id);
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

        }

        public void EquipWeapon(CharacterItemRecord item, WeaponRecord template, byte newposition, uint quantity)
        {
            if (CheckPosition(item, newposition))
            {
                Character.ReplyError("Impossible d'équipé cet objet !");
                return;
            }

            if (CheckItemPosition(item, newposition))
            {
                Character.ReplyError("Impossible car la catégorie est incorrect.");
                return;
            }
            if (!ConditionProvider.ParseAndEvaluate(Character.Client, template.Criteria))
            {
                Character.Reply("Vous n'avez pas les critères nessessaire pour équiper cet arme");
                return;
            }
            if ((CharacterInventoryPositionEnum)newposition == CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)
            {
                var shield = GetItemByPosition(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD);
                if (shield != null && template.TwoHanded)
                {
                    Character.Reply("Vous devez enlevé votre bouclier pour équiper votre arme.");
                    return;
                }
            }
            var equiped = EquipedItem(newposition);
            if (equiped != null)
            {
                UnequipWeapon(equiped, 63, equiped.GetWeaponTemplate(), quantity);
                SaveTask.UpdateElement(equiped, this.Character.Id);
            }
            if (item.Quantity == 1)
            {
                item.Position = newposition;
                SaveTask.UpdateElement(item, this.Character.Id);
                ItemEffectsProvider.AddEffects(Character.Client, item.GetEffects());
                AddWeaponSkin(item, template);
            }
            else
            {
                var items = ItemCut.Cut(item, quantity, newposition);
                Add(items.newItem);
                ItemEffectsProvider.AddEffects(Character.Client, items.BaseItem.GetEffects());
                AddWeaponSkin(item, template);
            }
        }

        public void UnequipItem(CharacterItemRecord item, byte newposition, ItemRecord template, uint quantity, bool removeEffect = true)
        {
            var existing = Items.ExistingItem(item);
            if (existing == null)
            {
                item.Position = newposition;
                SaveTask.UpdateElement(item, this.Character.Id);
                if (removeEffect)
                    ItemEffectsProvider.RemoveEffects(Character.Client, item.GetEffects());
                if (template != null)
                    RemoveItemSkin(item, template);
            }
            else
            {
                if (item.UID != existing.UID)
                {
                    existing.Quantity += quantity;
                    RemoveItem(item.UID, item.Quantity);
                    if (removeEffect)
                        ItemEffectsProvider.RemoveEffects(Character.Client, item.GetEffects());
                    RemoveItemSkin(item, template);
                }
            }
        }

        public void UnequipWeapon(CharacterItemRecord item, byte newposition, WeaponRecord template, uint quantity)
        {
            var existing = Items.ExistingItem(item);
            if (existing == null)
            {
                item.Position = newposition;
                SaveTask.UpdateElement(item, this.Character.Id);
                ItemEffectsProvider.RemoveEffects(Character.Client, item.GetEffects());
                if (template != null)
                    RemoveWeaponSkin(item, template);
            }
            else
            {
                if (item.UID != existing.UID)
                {
                    existing.Quantity += quantity;
                    RemoveItem(item.UID, item.Quantity);
                    ItemEffectsProvider.RemoveEffects(Character.Client, item.GetEffects());
                    RemoveWeaponSkin(item, template);
                }
            }
        }
    
        public CharacterItemRecord GetEquipedWeapon()
        {
            return Items.Find(x => x.Position == (byte)CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
        }

        public void MoveItem(uint uid, byte newposition, uint quantity)
        {
            if (Character.IsFighting)
            {
                Character.Reply("Impossible en combat", true);
                return;
            }
            var item = GetItem(uid);
            var template = ItemRecord.GetItem(item.GID);
            var templateWeapon = WeaponRecord.GetWeapon(item.GID);
            if (newposition != 63)
            {
                if (template != null)
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
                else if (templateWeapon != null)
                {
                    if (Character.Record.Level >= templateWeapon.Level)
                    {
                        EquipWeapon(item, templateWeapon, newposition, quantity);
                    }
                    else
                    {
                        Character.Reply("Vous n'avez pas le niveau pour équiper cet arme");
                        return;
                    }
                }
            }
            else
            {
                if (template != null)
                    UnequipItem(item, newposition, template, quantity);
                else if (templateWeapon != null)
                    UnequipWeapon(item, newposition, templateWeapon, quantity);
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
                //Character.NotificationError("Impossible de retirer l'item, il n'existe pas...");
                return;
            }

           if (item.GID == 7400) // Parchemin lié
                TrackRecord.DeleteTrackedByItemUID((int)item.UID);
            if (quantity == item.Quantity)
            {
                Items.Remove(item);
                SaveTask.RemoveElement(item, this.Character.Id);
                CharacterItemRecord.CharactersItems.Remove(item);
                Character.Client.Send(new ObjectDeletedMessage(item.UID));
            }
            else if (quantity < item.Quantity)
            {
                item.Quantity -= (uint)quantity;
                SaveTask.UpdateElement(item, this.Character.Id);
            }
            if (refresh)
                Refresh();
            Character.RefreshShortcuts();
        }

        public void RemoveItemNoTrack(uint id, uint quantity, bool refresh = true)
        {
            var item = GetItem(id);
            if (item == null)
            {
                //Character.NotificationError("Impossible de retirer l'item, il n'existe pas...");
                return;
            }
            if (quantity == item.Quantity)
            {
                Items.Remove(item);
                SaveTask.RemoveElement(item, this.Character.Id);
                CharacterItemRecord.CharactersItems.Remove(item);
                Character.Client.Send(new ObjectDeletedMessage(item.UID));
            }
            else if (quantity < item.Quantity)
            {
                item.Quantity -= (uint)quantity;
                SaveTask.UpdateElement(item, this.Character.Id);
            }
            if (refresh)
                Refresh();
            Character.RefreshShortcuts();
        }

        public List<ObjectItem> ConvertAllObjectItems()
        {
            List<ObjectItem> ItemsList = new List<ObjectItem>();
            foreach (var Item in Items)
            {
                if (!CharactersMerchantsRecord.InMerchantList((int)Item.UID))
                    ItemsList.Add(Item.GetObjectItem());
                else
                {
                    var itemObject = Item.GetObjectItem();
                    itemObject.quantity -= (uint)CharactersMerchantsRecord.GetQuantityFromUID((int)itemObject.objectUID);
                    if (itemObject.quantity > 0)
                        ItemsList.Add(itemObject);
                }
            }
            return ItemsList;
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
                if (!CharactersMerchantsRecord.InMerchantList((int)item.UID))
                {
                    var template = ItemRecord.GetItem(item.GID);
                    WeaponRecord weapon = null;
                    if (template == null)
                        weapon = WeaponRecord.GetWeapon(item.GID);
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        if (template != null)
                            actual += (uint)template.Weight;
                        else if (weapon != null)
                            actual += (uint)weapon.RealWeight;
                    }
                }
            }
            return actual;
        }

        public void SaveItems()
        {
            Items.ForEach(x => SaveTask.UpdateElement(x, this.Character.Id));
        }

        public void Refresh()
        {
            Character.Client.Send(new InventoryContentMessage(ConvertAllObjectItems(), (uint)Character.Record.Kamas));
            Character.Client.Send(new InventoryWeightMessage(GetWeight(), 5000));
        }

        public List<CharacterItemRecord> GetAllItems()
        {
            List<CharacterItemRecord> AllItems = new List<CharacterItemRecord>();
            foreach (var Record in CharacterItemRecord.CharactersItems)
            {
                if (Record.CharacterId == this.Character.Record.Id)
                    AllItems.Add(Record);
            }
            return (AllItems);
        }
    }
}