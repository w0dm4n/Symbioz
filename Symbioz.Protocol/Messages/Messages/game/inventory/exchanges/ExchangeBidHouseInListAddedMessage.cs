


















// Generated on 06/04/2015 18:44:50
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeBidHouseInListAddedMessage : Message
{

public const ushort Id = 5949;
public override ushort MessageId
{
    get { return Id; }
}

public int itemUID;
        public int objGenericId;
        public IEnumerable<Types.ObjectEffect> effects;
        public IEnumerable<uint> prices;
        

public ExchangeBidHouseInListAddedMessage()
{
}

public ExchangeBidHouseInListAddedMessage(int itemUID, int objGenericId, IEnumerable<Types.ObjectEffect> effects, IEnumerable<uint> prices)
        {
            this.itemUID = itemUID;
            this.objGenericId = objGenericId;
            this.effects = effects;
            this.prices = prices;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(itemUID);
            writer.WriteInt(objGenericId);
            writer.WriteUShort((ushort)effects.Count());
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)prices.Count());
            foreach (var entry in prices)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

itemUID = reader.ReadInt();
            objGenericId = reader.ReadInt();
            var limit = reader.ReadUShort();
            effects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 (effects as Types.ObjectEffect[])[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 (effects as Types.ObjectEffect[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            prices = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (prices as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}