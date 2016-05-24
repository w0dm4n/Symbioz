


















// Generated on 06/04/2015 18:44:37
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyModifiableStatusMessage : AbstractPartyMessage
{

public const ushort Id = 6277;
public override ushort MessageId
{
    get { return Id; }
}

public bool enabled;
        

public PartyModifiableStatusMessage()
{
}

public PartyModifiableStatusMessage(uint partyId, bool enabled)
         : base(partyId)
        {
            this.enabled = enabled;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(enabled);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            enabled = reader.ReadBoolean();
            

}


}


}