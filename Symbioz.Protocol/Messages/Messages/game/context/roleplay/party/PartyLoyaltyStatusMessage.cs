


















// Generated on 06/04/2015 18:44:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyLoyaltyStatusMessage : AbstractPartyMessage
{

public const ushort Id = 6270;
public override ushort MessageId
{
    get { return Id; }
}

public bool loyal;
        

public PartyLoyaltyStatusMessage()
{
}

public PartyLoyaltyStatusMessage(uint partyId, bool loyal)
         : base(partyId)
        {
            this.loyal = loyal;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(loyal);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            loyal = reader.ReadBoolean();
            

}


}


}