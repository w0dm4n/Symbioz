


















// Generated on 06/04/2015 18:44:58
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeTypesItemsExchangerDescriptionForUserMessage : Message
{

public const ushort Id = 5752;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.BidExchangerObjectInfo> itemTypeDescriptions;
        

public ExchangeTypesItemsExchangerDescriptionForUserMessage()
{
}

public ExchangeTypesItemsExchangerDescriptionForUserMessage(IEnumerable<Types.BidExchangerObjectInfo> itemTypeDescriptions)
        {
            this.itemTypeDescriptions = itemTypeDescriptions;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)itemTypeDescriptions.Count());
            foreach (var entry in itemTypeDescriptions)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            itemTypeDescriptions = new Types.BidExchangerObjectInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (itemTypeDescriptions as Types.BidExchangerObjectInfo[])[i] = new Types.BidExchangerObjectInfo();
                 (itemTypeDescriptions as Types.BidExchangerObjectInfo[])[i].Deserialize(reader);
            }
            

}


}


}