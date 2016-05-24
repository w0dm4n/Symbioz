


















// Generated on 06/04/2015 18:44:59
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeMultiCraftCrafterCanUseHisRessourcesMessage : Message
{

public const ushort Id = 6020;
public override ushort MessageId
{
    get { return Id; }
}

public bool allowed;
        

public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage()
{
}

public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(bool allowed)
        {
            this.allowed = allowed;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(allowed);
            

}

public override void Deserialize(ICustomDataInput reader)
{

allowed = reader.ReadBoolean();
            

}


}


}