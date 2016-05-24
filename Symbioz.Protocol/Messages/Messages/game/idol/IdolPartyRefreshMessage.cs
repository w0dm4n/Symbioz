


















// Generated on 06/04/2015 18:44:48
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class IdolPartyRefreshMessage : Message
{

public const ushort Id = 6583;
public override ushort MessageId
{
    get { return Id; }
}

public Types.PartyIdol partyIdol;
        

public IdolPartyRefreshMessage()
{
}

public IdolPartyRefreshMessage(Types.PartyIdol partyIdol)
        {
            this.partyIdol = partyIdol;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

partyIdol.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

partyIdol = new Types.PartyIdol();
            partyIdol.Deserialize(reader);
            

}


}


}