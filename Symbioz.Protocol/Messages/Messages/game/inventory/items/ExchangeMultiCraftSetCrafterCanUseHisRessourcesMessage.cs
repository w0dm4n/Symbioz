


















// Generated on 06/04/2015 18:44:59
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage : Message
{

public const ushort Id = 6021;
public override ushort MessageId
{
    get { return Id; }
}

public bool allow;
        

public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage()
{
}

public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(bool allow)
        {
            this.allow = allow;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(allow);
            

}

public override void Deserialize(ICustomDataInput reader)
{

allow = reader.ReadBoolean();
            

}


}


}