using Symbioz.Enums;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Messages;
using Symbioz.World.Records;
using System.Timers;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models.Exchanges.Craft.Replay;

namespace Symbioz.World.Models.Exchanges.Craft
{
    /*
      voila la formule : 50 + X/2 (édit)
 X étant le jet max de l'item dans l'élément que l'on souhaite forgemager .

 Je précise qu'en forgemagie on ne peut pas dépasser 100 si le jet ne le dépasse pas deja a la base et qu'au dessus de 100 on ne peut espérer qu'atteindre le jet max. Je précise aussi que ma formule ne s'applique semblerait-il uniquement dans le cadre de l'overmaxage d'un seul jet


 Prenons un dora bora :
 50 + 80/2
 50 + 40
 90

 Prenons une Ortiz
 50 + 50/2
 50 + 25
 75

 On peut donc forgemager au max un bora a 90 Intelligence ou Force, et une ortiz a 75 Inteligence !!!

     */
    public class SmithMagicExchange : Exchange
    {
        public CharacterItemRecord UsedItem { get; set; }

        public CharacterItemRecord UsedRune { get; set; }

        public WorldClient Client { get; set; }

        public uint SkillId { get; set; }

        public int ReplayCount = 1;

        public SmithMagicReplayEngine ReplayEngine { get; set; }

        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.RUNES_TRADE;
            }
        }
        public SmithMagicExchange(WorldClient client, uint skillid)
        {
            this.Client = client;
            this.SkillId = skillid;
        }
        public void OpenPanel()
        {
            Client.Character.CurrentDialogType = DialogTypeEnum.DIALOG_EXCHANGE;
            Client.Character.ExchangeType = ExchangeType;
            Client.Send(new ExchangeStartOkCraftWithInformationMessage(SkillId));
        }
        public override void MoveItem(uint uid, int quantity)
        {
            CharacterItemRecord obj = null;
            if (quantity > 0)
            {
                obj = Client.Character.Inventory.GetItem(uid);
                AddItemToPanel(obj, (uint)quantity);
            }
            else
            {
                if (UsedItem != null && UsedItem.UID == uid)
                {
                    obj = UsedItem;
                }
                else if (UsedRune != null && UsedRune.UID == uid)
                {
                    obj = UsedRune;

                }
                RemoveItemFromPanel(obj, quantity);

            }
        }
        public void PerformCraft()
        {
           
        }
        public override void AddItemToPanel(CharacterItemRecord obj, uint quantity)
        {
            #region BackgroundCheck
            ItemRecord template = ItemRecord.GetItem(obj.GID);
            if (obj.IsNull())
                return;
            #endregion

            if (template.Type == ItemTypeEnum.SMITHMAGIC_RUNE)
            {
                #region Ajout de Rune
                if (UsedRune != null) // bizzare de devoir faire ça, le client devrait s'en charger
                {
                    if (obj.Quantity - UsedRune.Quantity <= 0)
                        return;
                }
                if (quantity == obj.Quantity)
                {
                    var addedItem = obj.CloneWithUID();
                    Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                    UsedRune = addedItem;
                    return;
                }
                else
                {
                    if (UsedRune != null)
                    {
                        UsedRune.Quantity += (uint)quantity;
                        Client.Send(new ExchangeObjectModifiedMessage(false, UsedRune.GetObjectItem()));
                        return;
                    }
                    else
                    {
                        var addedItem = obj.CloneWithUID();
                        addedItem.Quantity = (uint)quantity;
                        Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                        UsedRune = addedItem;
                        return;
                    }
                }
                #endregion
            }
            else
            {
                if (quantity == obj.Quantity)
                {
                    var addedItem = obj.CloneWithUID();
                    Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                    UsedItem = addedItem;
                    return;
                }
                else
                {
                    if (UsedItem == null)
                    {
                        var addedItem = obj.CloneWithUID();
                        addedItem.Quantity = (uint)quantity;
                        Client.Send(new ExchangeObjectAddedMessage(false, addedItem.GetObjectItem()));
                        UsedItem = addedItem;
                        return;
                    }
                    else
                    {
                        Client.Send(new ExchangeObjectRemovedMessage(false, UsedItem.UID));
                        UsedItem = null;
                        AddItemToPanel(obj, quantity);
                    }
                }
            }


        }
        public override void RemoveItemFromPanel(CharacterItemRecord obj, int quantity)
        {
            if (obj.IsNull())
                return;
            if (obj.Quantity == (uint)-quantity)
            {
                Client.Send(new ExchangeObjectRemovedMessage(false, obj.UID));
                obj = null;
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
            if (count == -1)
            {
                ReplayCount = (int)UsedRune.Quantity;
            }
            else
            {
                ReplayCount = count;
                Client.Send(new ExchangeReplayCountModifiedMessage(ReplayCount));
            }
        }
        public override void Ready(bool ready, ushort step)
        {
            if (ready)
            {
            ReplayEngine = new SmithMagicReplayEngine(this);
            ReplayEngine.Start();
            }
        }
        public List<ObjectEffect> GetRuneEffects()
        {
            return UsedRune.GetEffects();
        }
        public override void CancelExchange()
        {
            if (ReplayEngine != null)
                ReplayEngine.Dispose();
            ReplayEngine = null;
            Client.Character.SmithMagicInstance = null;
            Client = null;
        }
    }
}
