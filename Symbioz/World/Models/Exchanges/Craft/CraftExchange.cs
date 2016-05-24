using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Helper;
using System.Timers;
using Symbioz.World.Models.Exchanges.Craft.Replay;

namespace Symbioz.World.Models.Exchanges.Craft
{

    public class CraftExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.CRAFT;
            }
        }
        /// <summary>
        /// Officiel = 20
        /// </summary>
        public const int XpRatio = 20;

        public CraftReplayEngine ReplayEngine { get; set; }

        public List<CharacterItemRecord> CraftedItems = new List<CharacterItemRecord>();

        public WorldClient Client { get; set; }

        uint SkillId { get; set; }

        sbyte JobId { get; set; }

        public int ReplayCount = 1;

        public CraftExchange(WorldClient client, uint skillid, sbyte jobid)
        {
            this.Client = client;
            this.SkillId = skillid;
            this.JobId = jobid;
        }
        public void OpenPanel()
        {
            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = ExchangeType;
            Client.Send(new ExchangeStartOkCraftWithInformationMessage(SkillId));
        }
        public override void CancelExchange()
        {
            if (ReplayEngine != null)
                ReplayEngine.Dispose();
            ReplayEngine = null;
            CraftedItems = null;
            Client.Character.CraftInstance = null;
            Client = null;

        }
        public void PerformCraft(RecipeRecord currentRecipe)
        {
            var obj = new ObjectItem((byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, currentRecipe.ResultId, ItemRecord.GetItem(currentRecipe.ResultId).GenerateRandomEffect(), CharacterItemRecord.PopNextUID(), 1);
            Client.Character.Inventory.Add(new CharacterItemRecord(obj, Client.Character.Id));
            Client.Send(new ExchangeCraftResultWithObjectDescMessage(2, new ObjectItemNotInContainer(currentRecipe.ResultId, obj.effects, obj.objectUID, 1)));
            Client.Character.AddJobXp(JobId, (ulong)(currentRecipe.ResultLevel * XpRatio));
        }
        public override void Ready(bool ready, ushort step)
        {
            var crafterLevel = CharacterJobRecord.GetJob(Client.Character.Id, JobId).JobLevel;
            var currentRecipe = RecipeRecord.GetRecipe(CraftedItems, (ushort)SkillId);
            if (currentRecipe.IsNull())
            {
                Client.Send(new ExchangeCraftResultMessage((sbyte)CraftResultEnum.CRAFT_IMPOSSIBLE));
                return;
            }
            else if (currentRecipe.ResultLevel > crafterLevel)
            {
                Client.Character.Reply("Vous n'avez pas le niveau pour effectuer ce craft!");
                return;
            }
            ReplayEngine = new CraftReplayEngine(this, currentRecipe);
            ReplayEngine.Start();

        }
        public void SetCraftRecipe(ushort resultid)
        {
            var recipe = RecipeRecord.GetRecipe(resultid);
            foreach (var item in recipe.IngredientsWithQuantities)
            {
                var obj = Client.Character.Inventory.Items.Find(x => x.GID == item.Key);
                AddItemToPanel(obj, item.Value);
            }
        }
        public override void MoveItem(uint uid, int quantity)
        {
            CharacterItemRecord obj;
            if (quantity > 0)
            {
                obj = Client.Character.Inventory.GetItem(uid);
                AddItemToPanel(obj, (uint)quantity);
            }
            else
            {
                obj = CraftedItems.Find(x => x.UID == uid);
                RemoveItemFromPanel(obj, quantity);
            }
        }
        public override void RemoveItemFromPanel(CharacterItemRecord obj, int quantity)
        {
            if (obj.IsNull())
                return;
            if (obj.Quantity == (uint)-quantity)
            {
                Client.Send(new ExchangeObjectRemovedMessage(false, obj.UID));
                CraftedItems.Remove(obj);
                return;
            }
            else
            {
                obj.Quantity = (uint)(obj.Quantity + quantity);
                Client.Send(new ExchangeObjectModifiedMessage(false, obj.GetObjectItem()));
            }
        }
        public void SetReplay(int count)
        {
            ReplayCount = count;
            Client.Send(new ExchangeReplayCountModifiedMessage(ReplayCount));
        }
        public override void AddItemToPanel(CharacterItemRecord obj, uint quantity)
        {
            if (obj.IsNull())
                return;
            if (quantity == obj.Quantity)  // on ajoute toute la quantité d'items
            {
                var addedItem = obj.CloneWithUID();
                Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                CraftedItems.Add(addedItem);
                return;
            }
            else // on doit cut
            {
                var existing = CraftedItems.ExistingItem(obj);
                if (existing != null)
                {
                    existing.Quantity += (uint)quantity;
                    Client.Send(new ExchangeObjectModifiedMessage(false, existing.GetObjectItem()));
                    return;
                }
                else
                {
                    var addedItem = obj.CloneWithUID();
                    addedItem.Quantity = (uint)quantity;
                    Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                    CraftedItems.Add(addedItem);
                    return;
                }
            }
        }


    }
}
