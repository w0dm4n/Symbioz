


















// Generated on 06/04/2015 18:44:58
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeTypesExchangerDescriptionForUserMessage : Message
{

public const ushort Id = 5765;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<uint> typeDescription;
        

public ExchangeTypesExchangerDescriptionForUserMessage()
{
}

public ExchangeTypesExchangerDescriptionForUserMessage(IEnumerable<uint> typeDescription)
        {
            this.typeDescription = typeDescription;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)typeDescription.Count());
            foreach (var entry in typeDescription)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            typeDescription = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (typeDescription as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}